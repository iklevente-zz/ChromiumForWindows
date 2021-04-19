using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.IO;

namespace ChromiumForWindows_Updater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly string chromiumPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium";

        public MainWindow()
        {
            CheckChromiumDir();
            CheckBuild();
            CheckVersion();
            InitializeComponent();

            // If it's already updating in the background, don't allow running it again.
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void CheckChromiumDir()
        {
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(chromiumPath))
                {
                    Output.WriteLine("Chromium directory exists.");
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(chromiumPath);
                Output.WriteLine("Chromium directory was created successfully at " + Directory.GetCreationTime(chromiumPath));
            }
            catch (Exception e)
            {
                Output.WriteLine("The process failed: " + e.ToString());
                System.Windows.Application.Current.Shutdown();
            }
            finally { }
        }

        private void CheckBuild()
        {
            if (System.IO.File.Exists(chromiumPath + "\\settings.json"))
            {
                Output.WriteLine("settings.json is already exists!");
            }
            else
            {
                // If there is no versioninfo.txt file, create one:
                AppConfig.SaveDefaultSettings();
            }

            // Reads in the local build
            AppConfig.LoadSettings();
        }

        // Checks current version with the newest version: 
        public static string localVersion = null;
        public static string latestVersion = null;
        public static string webRequestUrl = null;
        void CheckVersion()
        {
            if (System.IO.File.Exists(chromiumPath + "\\versioninfo.txt"))
            {
                Output.WriteLine("versioninfo.txt is already exists!");
            }
            else
            {
                // If there is no versioninfo.txt file, create one:
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium\\versioninfo.txt", "NULL");
            }

            // Checks the local version
            localVersion = System.IO.File.ReadAllText(chromiumPath + "\\versioninfo.txt");
            Output.WriteLine(localVersion + " is the current local version.");

            // Decides where to create a web request
            if (AppConfig.content.Contains("\"chromiumBuild\": \"Hibbiki\""))
            {
                webRequestUrl = "https://github.com/Hibbiki/chromium-win64/releases/latest/";
            }
            else if (AppConfig.content.Contains("\"chromiumBuild\": \"Marmaduke\""))
            {
                webRequestUrl = "https://github.com/macchrome/winchrome/releases/latest/";
                // Marmaduke's release file names are always changing so we need to regex it and put it in a string --> GetFileVersion.GetVersionInfo(); but only after latestVersion is requested below
            }

            // Checks the version from the website. It will use the choosen link above, which will redirect to the latest version. string latestVersion will be equal to the redirected URL.
            WebRequest request = WebRequest.Create(webRequestUrl);
            request.Method = "HEAD"; // Use a HEAD request because we don't need to download the response body
            try
            {
                // Asking for latestVersion
                using (WebResponse response = request.GetResponse())
                {
                    latestVersion = response.ResponseUri.ToString();
                }

                // If Marmaduke build, get letestVersion from the always changing filename
                if (AppConfig.content.Contains("\"chromiumBuild\": \"Marmaduke\""))
                {
                    GetFileVersion.GetVersionInfo();
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    latestVersion = "No response from download server. Check your internet connection!";
                }
            }

            // Here the program decides, if it needs to be updated
            if (latestVersion == "No response from download server. Check your internet connection!")
            {
                CloseUpdater();
                return;
            }
            else if (localVersion != latestVersion)
            {
                StartAndWaitForUpdate();
            }
            else
            {
                Output.WriteLine("Local version is the same as latest version!");
                Output.WriteLine("Exiting!");
                CloseUpdater();
            }
        }

        private async void StartAndWaitForUpdate()
        {
            Output.WriteLine("Waiting for the update to be completed.");
            await Task.Run(() => Update.StartUpdate());
            Output.WriteLine("Update completed.");
            CloseUpdater();
        }

        static void CloseUpdater()
        {
            Environment.Exit(0);
        }
    }
}
