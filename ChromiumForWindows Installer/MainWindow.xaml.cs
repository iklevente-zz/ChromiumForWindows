using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace ChromiumForWindowsInstaller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int countinueButton_counter { get; set; } = 0;

        public MainWindow()
        {
            InitializeComponent();

            //Starts CounterWatchdog with the required starting parameter (countinueButton_counter = 1)
            CounterWatchdog();
        }

        void CounterWatchdog()
        {
            if (MainWindow.countinueButton_counter == 0)
            {
                LicenseAgreement.Visibility = Visibility.Visible;
                PleaseRead.Visibility = Visibility.Visible;
                Agreement.Visibility = Visibility.Visible;
                AcceptPipe.Visibility = Visibility.Visible;
                CountinueButton.Visibility = Visibility.Visible;
                BackButton.Visibility = Visibility.Collapsed;
                InstallationIsReadyText.Visibility = Visibility.Collapsed;
                ClickCountinueText.Visibility = Visibility.Collapsed;
                InstallingChromiumText.Visibility = Visibility.Collapsed;
            }
            else if (MainWindow.countinueButton_counter == 1)
            {
                LicenseAgreement.Visibility = Visibility.Collapsed;
                PleaseRead.Visibility = Visibility.Collapsed;
                Agreement.Visibility = Visibility.Collapsed;
                AcceptPipe.Visibility = Visibility.Collapsed;
                CountinueButton.Visibility = Visibility.Visible;
                BackButton.Visibility = Visibility.Visible;
                InstallationIsReadyText.Visibility = Visibility.Visible;
                ClickCountinueText.Visibility = Visibility.Visible;
                InstallingChromiumText.Visibility = Visibility.Collapsed;
            }
            else if (MainWindow.countinueButton_counter == 2)
            {
                LicenseAgreement.Visibility = Visibility.Collapsed;
                PleaseRead.Visibility = Visibility.Collapsed;
                Agreement.Visibility = Visibility.Collapsed;
                AcceptPipe.Visibility = Visibility.Collapsed;
                CountinueButton.Visibility = Visibility.Visible;
                BackButton.Visibility = Visibility.Visible;
                InstallationIsReadyText.Visibility = Visibility.Collapsed;
                ClickCountinueText.Visibility = Visibility.Collapsed;
                InstallingChromiumText.Visibility = Visibility.Visible;

                CountinueButton.IsEnabled = false;
                BackButton.IsEnabled = false;
            }
        }

        // Makes the countinue button blured/disabled, if pipe is not ticked
        private void AcceptPipe_Click(object sender, RoutedEventArgs e)
        {
            if (CountinueButton.IsEnabled == false)
            {
                CountinueButton.IsEnabled = true;
            }
            else if (CountinueButton.IsEnabled == true)
            {
                CountinueButton.IsEnabled = false;
            }
        }

        private void CountinueButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.countinueButton_counter == 0)
            {
                MainWindow.countinueButton_counter = 1;
                CounterWatchdog();
            }
            else if (MainWindow.countinueButton_counter == 1)
            {
                MainWindow.countinueButton_counter = 2;
                CounterWatchdog();
            }
            else if (MainWindow.countinueButton_counter == 2)
            {
                MainWindow.countinueButton_counter = 3;
                CounterWatchdog();
            }
        }

        //Sets the counter's value 1 less than it was before and starts CounterWatchdog() with that parameter
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.countinueButton_counter = MainWindow.countinueButton_counter - 1;
            CounterWatchdog();
        }
    }
}
