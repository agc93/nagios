using System.Data;
using System.Windows;
using MahApps.Metro.Controls;

namespace GraphDownloader.UI
{
    /// <summary>
    /// Interaction logic for GroupInfo.xaml
    /// </summary>
    public partial class GroupInfo : MetroWindow
    {
        private DataTable _hostsTable;

        public DataTable hostsTable {
            get { return _hostsTable; }
            set {
                _hostsTable = value;
                lstHosts.ItemsSource = _hostsTable.DefaultView;
            }
        }

        public GroupInfo() {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}