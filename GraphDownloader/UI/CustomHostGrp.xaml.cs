using System;
using System.Collections.Generic;
using System.Data;
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
using GraphDownloader.Shared;
using MahApps.Metro.Controls;

namespace GraphDownloader.UI
{
    /// <summary>
    /// Interaction logic for CustomHostGrp.xaml
    /// </summary>
    public partial class CustomHostGrp : MetroWindow
    {
        public static string[] customHosts = null;
        public DataTable hostTable { get; set; }
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
            if (txtGrpName.Text != "" && txtGrpName.Text != null) {
                if (customHosts != null)
                {
                    DataTable dTable = new DataTable(txtGrpName.Text.ToString());
                    dTable.Columns.Add("hostname");
                    dTable = ReadArrayToTable(customHosts, dTable);
                    this.hostTable = dTable;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(Properties.Resources.hostFileError);
                }
            } else {
                //error message
            }
        }

        private DataTable ReadArrayToTable(string[] selectedHosts, DataTable dTable) {
            foreach (string item in selectedHosts) {
                dTable.Rows.Add(item);
            }
            return dTable;
        }
    }
}
