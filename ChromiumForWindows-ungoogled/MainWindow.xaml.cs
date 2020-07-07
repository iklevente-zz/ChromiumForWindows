using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ChromiumForWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string chromiumPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium";

        public MainWindow()
        {
            CheckChromiumDir();
            CheckVersion();

            InitializeComponent();
            
            // If it's already updating in the background, don't allow running it again.
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        static void CheckChromiumDir()
        {
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(chromiumPath))
                {
                    Console.WriteLine("Chromium directory exists.");
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(chromiumPath);
                Console.WriteLine("Chromium directory was created successfully at {0}.", Directory.GetCreationTime(chromiumPath));
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                MessageBox.Show("The process failed: {0}", e.ToString());
                System.Windows.Application.Current.Shutdown();
            }
            finally { }
        }

        // Checks current version with the newest version: 
        public static string localVersion = null;
        public static string latestVersion = null;
        void CheckVersion()
        {
            if (System.IO.File.Exists(chromiumPath + "\\versioninfo.txt"))
            {
                Console.WriteLine("versioninfo.txt is already exists!");
            }
            else
            {
                // If there is no versioninfo.txt file, create one:
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium\\versioninfo.txt", "NULL");
            }

            // Checks the local version
            localVersion = System.IO.File.ReadAllText(chromiumPath + "\\versioninfo.txt");
            Console.WriteLine(localVersion + " is the current local version.");


            // Checks the version from the GitHub Release website. It will use the link below, which will redirect to the latest version. string latestVersion will be equal to the redirected URL.
            WebRequest request = WebRequest.Create("https://github.com/macchrome/winchrome/releases/latest/");
            request.Method = "HEAD"; // Use a HEAD request because we don't need to download the response body
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    latestVersion = response.ResponseUri.ToString();
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    latestVersion = "No response from download server";
                    MessageBox.Show("No response from download server");
                }
            }

            // Here the program decides, if it needs to be updated
            if (localVersion != latestVersion)
            {
                StartAndWaitForUpdate();
            }
            else if (latestVersion == "No response from download server")
            {
                StartChromium();
                CloseUpdater();
                return;
            }
            else if (localVersion == latestVersion)
            {
                Console.WriteLine("Local version is the same as latest version!");
                Console.WriteLine("Exiting!");
                StartChromium();
                CloseUpdater();
            }
        }

        private async void StartAndWaitForUpdate()
        {
            Console.WriteLine("Waiting for the update to be completed.");
            await Task.Run(() => Update.StartUpdate());
            Console.WriteLine("Update completed.");
            Console.WriteLine("Waiting for Shortcuts to be created.");
            await Task.Run(() => FixShortcut.MakeShortcuts());
            Console.WriteLine("Shortcuts are done!");
            StartChromium();
            CloseUpdater();
        }

        public static void StartChromium()
        {
            //Get the unique GitHub release folder name to be able to get te path for chrome.exe
            GetFileVersion.GetVersionInfo();

            string ungoogledPath = chromiumPath + "\\ungoogled-chromium-" + GetFileVersion.finalregexresult + "-1_windows";
            System.Diagnostics.Process.Start(System.IO.Path.Combine(ungoogledPath + "\\chrome.exe"));
        }

        static void CloseUpdater()
        {
            Environment.Exit(0);
        }
    }
}
