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
        public static string chromiumPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium";
        public MainWindow()
        {
            InitializeComponent();
            CheckChromiumDir();
            CheckBuild();
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

        public static string localBuild = "";
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
            /*// Saves current state of versioninfo.txt
            localBuild = System.IO.File.ReadAllText(chromiumPath + "\\versioninfo.txt");*/

            System.IO.File.Delete(chromiumPath + "\\ChromiumLauncher.exe");
            System.IO.File.Delete(chromiumPath + "\\ChromiumLauncher.deps.json");
            System.IO.File.Delete(chromiumPath + "\\ChromiumLauncher.dll");
            System.IO.File.Delete(chromiumPath + "\\ChromiumLauncher.pdb");
            System.IO.File.Delete(chromiumPath + "\\ChromiumLauncher.runtimeconfig.dev.json");
            System.IO.File.Delete(chromiumPath + "\\ChromiumLauncher.runtimeconfig.json");
            System.IO.File.Delete(chromiumPath + "\\Interop.IWshRuntimeLibrary.dll");
            System.IO.File.Delete(chromiumPath + "\\MaterialDesignColors.dll");
            System.IO.File.Delete(chromiumPath + "\\MaterialDesignThemes.Wpf.dll");

            // This try catch exception mess can delete both directories (Application and Resources) independently. I needed to put in a try catch exception cycle because it would crash if one of the directiories does not exist.
            try
            {
                System.IO.Directory.Delete(chromiumPath + "\\Application", true);
                System.IO.Directory.Delete(chromiumPath + "\\Resources", true);
            }
            catch (Exception)
            {
                Output.WriteLine("Application and/or Resources directory is successfully deleted.");

                try
                {
                    System.IO.Directory.Delete(chromiumPath + "\\Resources", true);
                }
                catch (Exception)
                {
                    // Big brain move here if both directories does not exist
                }
            }

            /*// Recreates the deleted versioninfo.txt
            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Chromium\\versioninfo.txt", localBuild);*/
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BuildComboBox.SelectedIndex == 0)
            {
                Cleanup();

                // This is where it is failing right now
                ExtractFiles.Extract("ChromiumForWindows_Settings", chromiumPath, "ChromiumBuilds\\Hibbiki", "ChromiumLauncher.exe");
                Output.WriteLine("Chromium extraction completed.");
            }
        }
    }
}
