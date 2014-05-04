using System.Data;
using System.Windows;
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

        public CustomHostGrp() {
            InitializeComponent();
            btnOK.IsEnabled = false;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e) {
            string[] selectedHosts = Common.ChooseFile();
            if (selectedHosts != null) {
                foreach (string host in selectedHosts) {
                    lstHosts.Items.Add(host);
                }
                btnOK.IsEnabled = true;
                customHosts = selectedHosts;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e) {
            if (txtGrpName.Text != "" && txtGrpName.Text != null) {
                if (customHosts != null) {
                    DataTable dTable = new DataTable(txtGrpName.Text.ToString());
                    dTable.Columns.Add("hostname");
                    dTable = ReadArrayToTable(customHosts, dTable);
                    this.hostTable = dTable;
                    this.Close();
                } else {
                    Common.taskDialogSimple(Properties.Resources.hostFileError, Properties.Resources.settingsErrorTitle, string.Empty)
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