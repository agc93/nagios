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

namespace GraphDownloader
{
    /// <summary>
    /// Interaction logic for CustomHostGrp.xaml
    /// </summary>
    public partial class CustomHostGrp
    {
        public static string[] customHosts = null;
        public CustomHostGrp()
        {
            InitializeComponent();
            btnOK.IsEnabled = false;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            string[] selectedHosts = Common.ChooseFile();
            if (selectedHosts != null)
            {
                foreach (string host in selectedHosts)
                {
                    lstHosts.Items.Add(host);
                }
                btnOK.IsEnabled = true;
                customHosts = selectedHosts;
            }
            
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (customHosts != null)
            {
                MainWindow.hostArray = customHosts;
                this.Close();
            }
            else
            {
                MessageBox.Show(Properties.Resources.hostFileError);
            }
        }
    }
}
