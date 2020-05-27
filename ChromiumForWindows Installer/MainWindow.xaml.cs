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

namespace ChromiumForWindowsInstaller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

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
    }
}
