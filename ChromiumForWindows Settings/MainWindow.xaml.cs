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
using System.IO;

namespace ChromiumForWindows_Settings
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isInitializationCompleted = false;
        public static string chromiumPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium";
        public MainWindow()
        {
            CheckChromiumDir();
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
        void CheckBuild()
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
            if (localBuild == "NULL")
            {
                BuildComboBox.SelectedIndex = 0;
            }
            else if (localBuild.Contains("Hibbiki"))
            {
                BuildComboBox.SelectedIndex = 0;
            }
            else if (localBuild.Contains("macchrome"))
            {
                BuildComboBox.SelectedIndex = 1;
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

            // Delete folders (if exists cycle because it would throw an exception)
            if (Directory.Exists(chromiumPath + "\\Application"))
            {
                System.IO.Directory.Delete(chromiumPath + "\\Application", true);
            }
            if (Directory.Exists(chromiumPath + "\\Resources"))
            {
                System.IO.Directory.Delete(chromiumPath + "\\Resources", true);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BuildComboBox.SelectedIndex == 0)
            {
                if (isInitializationCompleted == false)
                {
                    // Do nothing (because it would extract Hibbiki, since void ComboBox_SelectionChanged() calls sooner than void CheckBuild())
                }
                else
                {
                    Cleanup();

                    ExtractFiles.Extract("ChromiumForWindows_Settings", chromiumPath, "Hibbiki", "ChromiumLauncher.deps.json");
                    ExtractFiles.Extract("ChromiumForWindows_Settings", chromiumPath, "Hibbiki", "ChromiumLauncher.dll");
                    ExtractFiles.Extract("ChromiumForWindows_Settings", chromiumPath, "Hibbiki", "ChromiumLauncher.exe");
                    ExtractFiles.Extract("ChromiumForWindows_Settings", chromiumPath, "Hibbiki", "ChromiumLauncher.pdb");
                    ExtractFiles.Extract("ChromiumForWindows_Settings", chromiumPath, "Hibbiki", "ChromiumLauncher.runtimeconfig.dev.json");
                    ExtractFiles.Extract("ChromiumForWindows_Settings", chromiumPath, "Hibbiki", "ChromiumLauncher.runtimeconfig.json");
                    ExtractFiles.Extract("ChromiumForWindows_Settings", chromiumPath, "Hibbiki", "Interop.IWshRuntimeLibrary.dll");
                    ExtractFiles.Extract("ChromiumForWindows_Settings", chromiumPath, "Hibbiki", "MaterialDesignColors.dll");
                    ExtractFiles.Extract("ChromiumForWindows_Settings", chromiumPath, "Hibbiki", "MaterialDesignThemes.Wpf.dll");

                    Output.WriteLine("Hibbiki Chromium extraction completed.");

                    System.IO.File.Delete(chromiumPath + "\\versioninfo.txt");
                    System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium\\versioninfo.txt", "Hibbiki");
                    Output.WriteLine("Saved choosen build.");
                }
            }
            if (BuildComboBox.SelectedIndex == 1)
            {
                Cleanup();

                /*
                 * New plan: Instead of including all the files in this Settings UI it will be uploaded to ChromiumForWindows's release site and the Settings manager will download and extract it from there.
                 * It will be more efficient and way more smaller in file size. This also means that you don't have to come back regularly because the Settings Manager will update the Chromium updaters.
                 * I will start working on it in the next few days.
                */
            }
        }
    }
}
