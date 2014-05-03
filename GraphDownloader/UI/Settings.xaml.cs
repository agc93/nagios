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
using System.Windows.Shapes;

namespace GraphDownloader.UI
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings
    {
        public Settings()
        {
            InitializeComponent();
            txtSrvAddr.Text = Properties.Settings.Default.IPAddress;
            txtUserName.Text = Properties.Settings.Default.UserName;
            txtPassword.Password = Properties.Settings.Default.Password;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.IPAddress = txtSrvAddr.ToString();
            Properties.Settings.Default.UserName = txtUserName.ToString();
            Properties.Settings.Default.Password = txtPassword.Password.ToString();
            this.Close();
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPassword.Password = "";
        }
    }
}
