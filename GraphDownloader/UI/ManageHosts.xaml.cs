using System;
using System.Collections.Generic;
using System.Windows;
using GraphDownloader.Shared;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace GraphDownloader.UI
{
    /// <summary>
    /// Interaction logic for ManageHosts.xaml
    /// </summary>
    public partial class ManageHosts : MetroWindow
    {
        private Hosts _host;

        internal Hosts host {
            get { return _host; }
            set { _host = value; }
        }

        private List<string> _tableList;

        public List<string> TableList {
            get { return _tableList; }
            set {
                _tableList = value;
                lstHosts.ItemsSource = _tableList;
            }
        }

        public ManageHosts() {
            InitializeComponent();
        }

        internal ManageHosts(Hosts host) {
            this.host = host;
            InitializeComponent();
            TableList = host.ListTableNames();
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e) {
            if (lstHosts.SelectedItem.ToString() != null) {
                GroupInfo gi = new GroupInfo();
                gi.hostsTable = host.GetTableFromName(lstHosts.SelectedItem.ToString());
                gi.Show();
                gi.Activate();
            } else {
                //pick something first
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e) {
            CustomHostGrp custom = new CustomHostGrp();
            custom.ShowDialog();
            host.AddTable(custom.hostTable);
            TableList = host.ListTableNames();
            custom.Close();
        }

        async private void btnDelete_Click(object sender, RoutedEventArgs e) {
            if (lstHosts.SelectedItem.ToString().Length > 0) {
                MessageDialogResult result = await this.ShowMessageAsync(String.Empty, String.Format("Are you sure you want to delete the '{0}' group?", lstHosts.SelectedItem.ToString()), MessageDialogStyle.AffirmativeAndNegative);
                if (result == MessageDialogResult.Affirmative) {
                    host.DeleteTableName(lstHosts.SelectedItem.ToString());
                    TableList = host.ListTableNames();
                }
            } else {
                //pick something first
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}