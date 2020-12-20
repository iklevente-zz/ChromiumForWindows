using System;
/*using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;*/
using System.Windows;
using System.Windows.Controls;
/*using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;*/
using System.IO;
using System.Threading.Tasks;

namespace ChromiumForWindows_Settings
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isInitializationCompleted = false;
        public static string chromiumPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium";
        public static string latestLocalChromiumBuild = null;
        public MainWindow()
        {
            CheckChromiumDir();
            Download.CheckAvalibility();
            InitializeComponent();
            CheckBuild();
            isInitializationCompleted = true;
        }

        // Creates/checks Chromium directory if exists
        static void CheckChromiumDir()
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

        public static string localBuild = null;
        private async void CheckBuild()
        {
            if (System.IO.File.Exists(chromiumPath + "\\versioninfo.txt"))
            {
                Output.WriteLine("versioninfo.txt is already exists!");
            }
            else
            {
                // If there is no versioninfo.txt file, create one:
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium\\versioninfo.txt", "NULL");
                Output.WriteLine("Created versioninfo.txt");
            }

            // Checks the local version
            localBuild = System.IO.File.ReadAllText(chromiumPath + "\\versioninfo.txt");
            Output.WriteLine(localBuild + " is the current local version.");

            // Sets the ComboBox to the inspected/checked Chromium build
            if (localBuild.Contains("Hibbiki"))
            {
                BuildComboBox.SelectedIndex = 0;
                latestLocalChromiumBuild = "Hibbiki";
            }
            else if (localBuild.Contains("macchrome"))
            {
                BuildComboBox.SelectedIndex = 1;
                latestLocalChromiumBuild = "Marmaduke";
            }
            else
            {
                // This happends at the default Comobox Setting (first time initilaization) or if somehow you mess up the versioninfo.txt file
                // This will install the default Chromium build which is Hibbiki's build.

                BuildComboBox.SelectedIndex = 0;


                ApplyingChangesText.Visibility = Visibility.Visible;
                ApplyingChangesProgressBar.Visibility = Visibility.Visible;

                Cleanup();
                await Task.Run(() => Download.StartHibbikiDownload());

                ApplyingChangesText.Visibility = Visibility.Hidden;
                ApplyingChangesProgressBar.Visibility = Visibility.Hidden;


                latestLocalChromiumBuild = "Hibbiki";
            }
        }

        void Cleanup()
        {
            // Delete files
            System.IO.File.Delete(chromiumPath + "\\ChromiumLauncher.exe");
            System.IO.File.Delete(chromiumPath + "\\ChromiumLauncher.deps.json");
            System.IO.File.Delete(chromiumPath + "\\ChromiumLauncher.dll");
            System.IO.File.Delete(chromiumPath + "\\ChromiumLauncher.pdb");
            System.IO.File.Delete(chromiumPath + "\\ChromiumLauncher.runtimeconfig.dev.json");
            System.IO.File.Delete(chromiumPath + "\\ChromiumLauncher.runtimeconfig.json");
            System.IO.File.Delete(chromiumPath + "\\Interop.IWshRuntimeLibrary.dll");
            System.IO.File.Delete(chromiumPath + "\\MaterialDesignColors.dll");
            System.IO.File.Delete(chromiumPath + "\\MaterialDesignThemes.Wpf.dll");
            System.IO.File.Delete(chromiumPath + "\\mini_installer.sync.exe");
            System.IO.File.Delete(chromiumPath + "\\latest_ungoogled_chromium.7z");

            // Delete folders (if exists cycle because it would throw an exception)
            if (Directory.Exists(chromiumPath + "\\Application"))
            {
                System.IO.Directory.Delete(chromiumPath + "\\Application", true);
            }
            if (Directory.Exists(chromiumPath + "\\Resources"))
            {
                System.IO.Directory.Delete(chromiumPath + "\\Resources", true);
            }

            // Deletes remaining temp and ungooegled directories.
            var uselsessTempDirs = Directory.GetDirectories(chromiumPath, "*.tmp");
            foreach (var directory in uselsessTempDirs)
            {
                Directory.Delete(directory, true);
            }
            var uselsessDirs = Directory.GetDirectories(chromiumPath, "ungoogled-*");
            foreach (var directory in uselsessDirs)
            {
                Directory.Delete(directory, true);
            }
        }


        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BuildComboBox.SelectedIndex == 0)
            {
                if (isInitializationCompleted == false)
                {
                    // Do nothing (because it would extract Hibbiki build at startup, when the user did not even change anything, since void ComboBox_SelectionChanged() calls sooner than void CheckBuild()). This bool is a workaround not to let it happen.
                }
                else
                {
                    if (Download.latestUpdaterVersion == null)
                    {
                        MessageBox.Show("No response from download server. Check your internet connection!");
                    }
                    else
                    {
                        ApplyingChangesText.Visibility = Visibility.Visible;
                        ApplyingChangesProgressBar.Visibility = Visibility.Visible;

                        Cleanup();
                        await Task.Run(() => Download.StartHibbikiDownload());

                        ApplyingChangesText.Visibility = Visibility.Hidden;
                        ApplyingChangesProgressBar.Visibility = Visibility.Hidden;

                        System.IO.File.Delete(chromiumPath + "\\versioninfo.txt");
                        System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium\\versioninfo.txt", "Hibbiki");
                        Output.WriteLine("Saved choosen build.");
                    }
                }
            }
            if (BuildComboBox.SelectedIndex == 1)
            {
                if (isInitializationCompleted == false)
                {
                    // Do nothing (because it would extract Marmaduke build at startup, when the user did not even change anything, since void ComboBox_SelectionChanged() calls sooner than void CheckBuild()). This bool is a workaround not to let it happen.
                }
                else
                {
                    if (Download.latestUpdaterVersion == null)
                    {
                        MessageBox.Show("No response from download server. Check your internet connection!");
                    }
                    else
                    {
                        ApplyingChangesText.Visibility = Visibility.Visible;
                        ApplyingChangesProgressBar.Visibility = Visibility.Visible;

                        Cleanup();
                        await Task.Run(() => Download.StartMarmadukeDownload());

                        ApplyingChangesText.Visibility = Visibility.Hidden;
                        ApplyingChangesProgressBar.Visibility = Visibility.Hidden;

                        System.IO.File.Delete(chromiumPath + "\\versioninfo.txt");
                        System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium\\versioninfo.txt", "macchrome");
                        Output.WriteLine("Saved choosen build.");
                    }
                }
            }
        }
    }
}
