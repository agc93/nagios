using System;
using System.Collections.Generic;
using System.Windows;
using System.Data;
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

        private DataTable _tables;

        private List<string> _tableList;

        internal Hosts host {
            get { return _host; }
            set { _host = value; }
        }
        public List<string> TableList {
            get { return _tableList; }
            set {
                _tableList = value;
                lstHosts.ItemsSource = _tableList;
            }
        }
        public DataTable Tables {
            get { return _tables; }
            set { _tables = value;
            lstHosts.DataContext = _tables.DefaultView;
            lstHosts.ItemsSource = _tables.DefaultView;
            }
        }
        
        

        public ManageHosts() {
            InitializeComponent();
        }

        internal ManageHosts(Hosts host) {
            this.host = host;
            InitializeComponent();
            Tables = host.ListTableDetails();
            Console.WriteLine("Tables listed");
        }

        async private void btnInfo_Click(object sender, RoutedEventArgs e) {
            if (lstHosts.SelectedItem.ToString() != null) {
                GroupInfo gi = new GroupInfo();
                gi.hostsTable = host.GetTableFromName(lstHosts.SelectedItem.ToString());
                gi.Show();
                gi.Activate();
            } else {
                await this.ShowMessageAsync(String.Empty, "Choose a host group to preview first");
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
                
                MessageDialogResult result = await this.ShowMessageAsync(String.Empty, String.Format("Are you sure you want to delete the '{0}' group?", Tables.Rows[lstHosts.SelectedIndex].ItemArray[0].ToString()), MessageDialogStyle.AffirmativeAndNegative);
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