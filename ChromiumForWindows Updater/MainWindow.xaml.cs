using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.IO;
using Microsoft.Win32;

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
            // If it's already updating in the background, don't allow running it again.
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            CheckChromiumDir();
            CheckAutorun();
            CheckBuild();
            CheckVersion();
            InitializeComponent();
        }

        private void CheckChromiumDir()
        {
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(chromiumPath))
                {
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(chromiumPath);
            }
            catch (Exception)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void CheckAutorun()
        {
            // The path to the key where Windows looks for startup applications
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (!IsStartupItem())
                // Add the value in the registry so that the application runs at startup
                rkApp.SetValue("Chromium Updater", chromiumPath + "\\ChromiumForWindows Updater.exe");
        }
        public static bool IsStartupItem()
        {

            // The path to the key where Windows looks for startup applications
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (rkApp.GetValue("Chromium Updater") == null)
                // The value doesn't exist, the application is not set to run at startup
                return false;
            else
                // The value exists, the application is set to run at startup
                return true;
        }

        private void CheckBuild()
        {
            if (!System.IO.File.Exists(chromiumPath + "\\settings.json"))
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
        //public static string webRequestUrl = null;
        public static int editorIndex;
        void CheckVersion()
        {
            if (!System.IO.File.Exists(chromiumPath + "\\versioninfo.txt"))
            {
                // If there is no versioninfo.txt file, create one:
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium\\versioninfo.txt", "NULL");
            }

            // Checks the local version
            localVersion = System.IO.File.ReadAllText(chromiumPath + "\\versioninfo.txt");

            
            // Decides where to create a web request
            if (AppConfig.content.Contains("\"chromiumBuild\": \"Hibbiki"))
            {
                editorIndex = 0;
                ApiRequest.GetApiData();
                latestVersion = ApiRequest.installerDownloadLink;
            }
            else if (AppConfig.content.Contains("\"chromiumBuild\": \"Marmaduke\""))
            {
                editorIndex = 1;
                ApiRequest.GetApiData();
                latestVersion = ApiRequest.installerDownloadLink;
            }

            // Here the program decides, if it needs to be updated
            if (latestVersion == "No response from download server. Check your internet connection!")
            {
                CloseUpdater();
                return;
            }
            else if (localVersion != latestVersion)
            {
                StartAndWaitForUpdate(); // I know only this method to run it asynchronously so the UI won't freeze
            }
            else
            {
                CloseUpdater();
            }
        }

        private async void StartAndWaitForUpdate()
        {
            await Task.Run(() => Update.StartUpdate());
            CloseUpdater();
        }

        static void CloseUpdater()
        {
            Environment.Exit(0);
        }
    }
}
