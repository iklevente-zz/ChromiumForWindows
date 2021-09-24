using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media;
using System.Diagnostics;
using ChromiumForWindows_Settings.Properties;
using System.Net;
using System.Linq;

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
            // If it's already running, don't allow running it again.
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            CheckChromiumDir();
            InitializeComponent();
            CheckBuild();
            CheckSelfUpdate();
            isInitializationCompleted = true;
        }

        // Creates/checks Chromium directory if exists
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

        private void CheckBuild()
        {
            if (!System.IO.File.Exists(chromiumPath + "\\settings.json"))
            {
                // If there is no versioninfo.txt file, create one:
                AppConfig.SaveDefaultSettings();
            }

            // Reads in everythong from settings.json (Now the local build is needed)
            AppConfig.LoadSettings();

            // Sets the ComboBox to the inspected/checked Chromium build
            if (AppConfig.content.Contains("\"chromiumBuild\": \"Hibbiki\""))
            {
                BuildComboBox.SelectedIndex = 0;

                // UI Changes for selected build at startup
                HibbikiUI();
            }
            else if (AppConfig.content.Contains("\"chromiumBuild\": \"Marmaduke\""))
            {
                BuildComboBox.SelectedIndex = 1;

                // UI Changes for selected build at startup
                MarmadukeUI();
            }
            else if (AppConfig.content.Contains("\"chromiumBuild\": \"RobRich\""))
            {
                BuildComboBox.SelectedIndex = 2;

                // UI Changes for selected build at startup
                RobRichUI();
            }
            else if (AppConfig.content.Contains("\"chromiumBuild\": \"official\""))
            {
                BuildComboBox.SelectedIndex = 3;

                // UI Changes for selected build at startup
                officialUI();
            }
            else
            {
                // This happends at the default Comobox Setting (first time initilaization) or if somehow you mess up the settings.json file
                // This will install the default Chromium build which is Hibbiki's build.
                BuildComboBox.SelectedIndex = 0;
            }
        }

        public static string unsavedChromiumBuild = null;
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BuildComboBox.SelectedIndex == 0)
            {
                if (isInitializationCompleted == false)
                {
                    // Do nothing because it would crash. This bool is a workaround not to let it happen.
                }
                else
                {
                    unsavedChromiumBuild = "Hibbiki";
                    AppConfig.SaveSettings();

                    // UI Changes for selected build
                    HibbikiUI();
                }
            }
            if (BuildComboBox.SelectedIndex == 1)
            {
                if (isInitializationCompleted == false)
                {
                    // Do nothing because it would crash. This bool is a workaround not to let it happen.
                }
                else
                {
                    unsavedChromiumBuild = "Marmaduke";
                    AppConfig.SaveSettings();

                    // UI Changes for selected build
                    MarmadukeUI();
                }
            }
            if (BuildComboBox.SelectedIndex == 2)
            {
                if (isInitializationCompleted == false)
                {
                    // Do nothing because it would crash. This bool is a workaround not to let it happen.
                }
                else
                {
                    unsavedChromiumBuild = "RobRich";
                    AppConfig.SaveSettings();

                    // UI Changes for selected build
                    RobRichUI();
                }
            }
            if (BuildComboBox.SelectedIndex == 3)
            {
                if (isInitializationCompleted == false)
                {
                    // Do nothing because it would crash. This bool is a workaround not to let it happen.
                }
                else
                {
                    unsavedChromiumBuild = "official";
                    AppConfig.SaveSettings();

                    // UI Changes for selected build
                    officialUI();
                }
            }
        }

        // Check CromiumForWindows update
        // Loads the saved local version
        public static string localVersion = Settings.Default["localVersion"].ToString();
        public static string latestVersion = null;
        public static readonly string webRequestUrl = "https://github.com/iklevente/ChromiumForWindows/releases/latest/";
        private void CheckSelfUpdate()
        {
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
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    latestVersion = "No response";
                }
            }

            // If there is no internet connection
            if (latestVersion == "No response")
            {
                noResponseText.Visibility = Visibility.Visible;
                // Don't allow to switch versions because it could not download
                BuildComboBox.IsEnabled = false;
                applyButton.IsEnabled = false;
            }
            else if (localVersion != latestVersion)
            {
                updateAvalibleLink.Visibility = Visibility.Visible;
            }
            // Else Local version is the same as latest version!
        }

        // If there is a new update and hyperlink is visible...
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://github.com/iklevente/ChromiumForWindows/releases/latest/";
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });

            // If user clicks on it, it is not gonna be there anymore, until a new update so it won't brother anyone
            Settings.Default["localVersion"] = latestVersion;
            Settings.Default.Save();
        }


        // UI event handle
        public void HibbikiUI()
        {
            allcodecsChip.Width = 83; // default 83
            allcodecsChip.Content = "all-codecs";

            avxChip.Width = 50; // default 50
            avxChip.Content = "AVX";

            // Change chip colors
            var bc = new BrushConverter();
            syncChip.Background = (Brush)bc.ConvertFrom("#FF76F399"); // green, default #FFE8E8E8 grey
            ungoogledChip.Background = (Brush)bc.ConvertFrom("#FFE8E8E8"); // grey, default #FFE8E8E8 (grey)
            privacyorientedChip.Background = (Brush)bc.ConvertFrom("#FFE8E8E8"); // grey, default #FFE8E8E8 (grey)
            modifiedChip.Background = (Brush)bc.ConvertFrom("#FFE8E8E8"); // grey, default #FFE8E8E8 (grey)
            widevineChip.Background = (Brush)bc.ConvertFrom("#FF76F399"); // green, default #FFE8E8E8 (grey)

            descriptionText.Visibility = Visibility.Visible;
            descriptionText.Text = "Hibbiki's Chromium builds. Google services are integrated. https://github.com/Hibbiki/chromium-win64";
        }
        public void MarmadukeUI()
        {
            allcodecsChip.Width = 90; // default 83
            allcodecsChip.Content = "all-codecs+";

            avxChip.Width = 50; // default 50
            avxChip.Content = "AVX";

            var bc = new BrushConverter();
            syncChip.Background = (Brush)bc.ConvertFrom("#FFE8E8E8"); // default #FFE8E8E8 grey
            ungoogledChip.Background = (Brush)bc.ConvertFrom("#FF76F399"); // green, default #FFE8E8E8 (grey)
            privacyorientedChip.Background = (Brush)bc.ConvertFrom("#FF76F399"); // green, default #FFE8E8E8 (grey)
            modifiedChip.Background = (Brush)bc.ConvertFrom("#FFE8E8E8"); // grey, default #FFE8E8E8 (grey)
            widevineChip.Background = (Brush)bc.ConvertFrom("#FF76F399"); // green, default #FFE8E8E8 (grey)

            descriptionText.Visibility = Visibility.Visible;
            descriptionText.Text = "Marmaduke's Chromium builds. Ungoogled, plus has more codecs than usual. https://github.com/macchrome/winchrome";
        }
        public void RobRichUI()
        {
            allcodecsChip.Width = 83; // default 83
            allcodecsChip.Content = "all-codecs";

            avxChip.Width = 50; // default 50
            avxChip.Content = "AVX2";

            // Change chip colors
            var bc = new BrushConverter();
            syncChip.Background = (Brush)bc.ConvertFrom("#FFE8E8E8"); // grey, default #FFE8E8E8 grey
            ungoogledChip.Background = (Brush)bc.ConvertFrom("#FFE8E8E8"); // grey, default #FFE8E8E8 (grey)
            privacyorientedChip.Background = (Brush)bc.ConvertFrom("#FFE8E8E8"); // grey, default #FFE8E8E8 (grey)
            modifiedChip.Background = (Brush)bc.ConvertFrom("#FF76F399"); // green, default #FFE8E8E8 (grey)
            widevineChip.Background = (Brush)bc.ConvertFrom("#FF76F399"); // green, default #FFE8E8E8 (grey)

            descriptionText.Visibility = Visibility.Visible;
            descriptionText.Text = "RobRich's Chromium builds. Contains AVX2 and FMA processor instruction set. Additional features of the builds include modified compiler and linker optimizations via build configuration modifications. https://github.com/RobRich999/Chromium_Clang";
        }
        public void officialUI()
        {
            allcodecsChip.Width = 83; // default 83
            allcodecsChip.Content = "all-codecs";

            avxChip.Width = 50; // default 50
            avxChip.Content = "AVX";

            var bc = new BrushConverter();
            syncChip.Background = (Brush)bc.ConvertFrom("#FFE8E8E8"); // grey, default #FFE8E8E8 grey
            ungoogledChip.Background = (Brush)bc.ConvertFrom("#FFE8E8E8"); // grey, default #FFE8E8E8 (grey)
            privacyorientedChip.Background = (Brush)bc.ConvertFrom("#FFE8E8E8"); // grey, default #FFE8E8E8 (grey)
            modifiedChip.Background = (Brush)bc.ConvertFrom("#FFE8E8E8"); // grey, default #FFE8E8E8 (grey)
            widevineChip.Background = (Brush)bc.ConvertFrom("#FFE8E8E8"); // grey, default #FFE8E8E8 (grey)

            descriptionText.Visibility = Visibility.Visible;
            descriptionText.Text = "Official Chromium development build by The Chromium Authors. https://www.chromium.org/getting-involved/download-chromium";
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            startButton.IsEnabled = false;
            notInstalledText.Visibility = Visibility.Hidden;

            // Need to uninstall old build so the new can apply (doing this by starting chrome with the uninstall argument)
            try
            {
                GetFileVersion.GetOldVersionInfo();
                var uninstaller = System.Diagnostics.Process.Start(System.IO.Path.Combine(chromiumPath + "\\Application\\" + GetFileVersion.finalregexresult + "\\Installer\\setup.exe"), "-uninstall");
                uninstaller.WaitForExit(); // Wait till the old version is uninstalled, after that check if user didn't click cancel, only then apply the new build
                
                if (File.Exists(System.IO.Path.Combine(chromiumPath + "\\Application\\" + GetFileVersion.finalregexresult + "\\Installer\\setup.exe")))
                {
                    notUninstalledText.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    notUninstalledText.Visibility = Visibility.Hidden;
                    System.IO.File.Delete(chromiumPath + "\\versioninfo.txt");
                }
            }
            catch (Exception)
            {
                // Exception happens when the Chromium cannot be found. This exception should only happen if Chrome & CFW is installed the very first time. No worries, just go on without crashing
            }

            // Apply new build
            var updater = System.Diagnostics.Process.Start(System.IO.Path.Combine(chromiumPath + "\\ChromiumForWindows Updater.exe"));
            updater.WaitForExit();

            startButton.IsEnabled = true;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(System.IO.Path.Combine(chromiumPath + "\\Application\\chrome.exe"));
            }
            catch (Exception)
            {
                notInstalledText.Visibility = Visibility.Visible;
            }
        }


        // Mouse enter/mouse leave code for description text
        private void uptodateChip_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Text = "This Chromium build is up-to-date. It means Chromium browser and Google Chrome are based on the same latest stable version and security updates of the Chromium source code.";
            descriptionText.Visibility = Visibility.Visible;
        }
        private void uptodateChip_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Visibility = Visibility.Hidden;
        }

        private void stableChip_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Text = "Chromium's version is the same as the stable release of Google Chrome. No need to worry about instability, bugs and random crashes.";
            descriptionText.Visibility = Visibility.Visible;
        }
        private void stableChip_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Visibility = Visibility.Hidden;
        }

        private void widevineChip_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Text = "Chromium compiled with the enabled Widevine support. After install of this plugin by yourself (because this is not an open-source software), Chromium will be able to play DRM content (on Netflix, for example).";
            descriptionText.Visibility = Visibility.Visible;
        }
        private void widevineChip_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Visibility = Visibility.Hidden;
        }

        private void syncChip_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Text = "Chromium with defined Google API keys. So Google services (Chrome Sync...) work. You will be able to log in with your Google account and synchronize your data.";
            descriptionText.Visibility = Visibility.Visible;
        }
        private void syncChip_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Visibility = Visibility.Hidden;
        }

        private void ungoogledChip_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Text = "Chromium without Google integration and enhanced privacy, based on the \"ungoogled - chromium\" project from Eloston.";
            descriptionText.Visibility = Visibility.Visible;
        }
        private void ungoogledChip_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Visibility = Visibility.Hidden;
        }

        private void win64Chip_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Text = "Chromium for 64-bit Windows.";
            descriptionText.Visibility = Visibility.Visible;
        }
        private void win64Chip_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Visibility = Visibility.Hidden;
        }

        private void modifiedChip_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Text = "Features compiler optimizations via build configuration modifications.";
            descriptionText.Visibility = Visibility.Visible;
        }
        private void modifiedChip_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Visibility = Visibility.Hidden;
        }

        private void privacyorientedChip_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Text = "A lightweight approach to removing Google web service dependency. https://github.com/Eloston/ungoogled-chromium";
            descriptionText.Visibility = Visibility.Visible;
        }
        private void privacyorientedChip_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Visibility = Visibility.Hidden;
        }

        private void trustedChip_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Text = "This build is proven to be trusted, due to clear VirusTotal results and countless effort of building Chromium binaries for years from the creator.";
            descriptionText.Visibility = Visibility.Visible;
        }
        private void trustedChip_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Visibility = Visibility.Hidden;
        }

        private void fmaChip_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Text = "Chromium for a recent computer (higher than the year 2012) having a processor with the support of FMA instructions.";
            descriptionText.Visibility = Visibility.Visible;
        }
        private void fmaChip_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Visibility = Visibility.Hidden;
        }

        // More problematic chips
        private void avxChip_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (BuildComboBox.SelectedIndex == 2)
            {
                descriptionText.Text = "Chromium for a recent computer (higher than the year 2013) having a processor with the support of AVX2 instructions.";
            }
            else
            {
                descriptionText.Text = "Chromium for a recent computer (higher than the year 2011) having a processor with the support of AVX instructions.";
            }
            descriptionText.Visibility = Visibility.Visible;
        }
        private void avxChip_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Visibility = Visibility.Hidden;
        }

        private void allcodecsChip_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (BuildComboBox.SelectedIndex == 2)
            {
                descriptionText.Text = "Chromium compiled with open-source audio/video codecs + H.264 + H.265 + AAC + MPEG-4 + AMR";
            }
            else
            {
                descriptionText.Text = "Chromium compiled with open-source audio/video codecs + H.264 + AAC";
            }
            descriptionText.Visibility = Visibility.Visible;
        }
        private void allcodecsChip_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            descriptionText.Visibility = Visibility.Hidden;
        }
    }
}