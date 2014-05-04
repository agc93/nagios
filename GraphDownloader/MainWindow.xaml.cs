using System;
using System.Data;
using System.Windows;
using GraphDownloader.Shared;
using GraphDownloader.UI;
using Microsoft.Win32;

namespace GraphDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string _folderPath;
        private Hosts host;

        public string folderPath {
            get { return _folderPath; }
            set {
                _folderPath = value;
                lblFolder.Content = _folderPath;
            }
        }

        public MainWindow() {
            host = new Hosts();
            if (host.CheckDataFile()) {
                InitializeComponent();
                Properties.Settings.Default.FolderPath = null;
                refreshCombo();
            } else {
                Application.Current.Shutdown();
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e) {
            folderPath = Common.ChooseFolderTd();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e) {
            if (Properties.Settings.Default.FolderPath != null) {
                if ((checkFields(pkrStartDate.SelectedDate)) && (checkFields(pkrEndDate.SelectedDate)) && (pkrStartDate.SelectedDate < pkrEndDate.SelectedDate)) {
                    startDownload((DateTime)pkrStartDate.SelectedDate, (DateTime)pkrEndDate.SelectedDate, Properties.Settings.Default.FolderPath.ToString());

                    //pick another host group
                    //Common.taskDialogAdv(Properties.Resources.settingsError, Properties.Resources.settingsErrorText, Properties.Resources.settingsErrorHosts, Properties.Resources.settingsErrorTitle);
                } else {
                    //wrong dates
                    Common.MsgAdvanced(Properties.Resources.settingsErrorText, Properties.Resources.settingsError, Properties.Resources.settingsErrorDate, Properties.Resources.settingsErrorTitle);
                }
            } else {
                //pick a folder
                Common.MsgAdvanced(Properties.Resources.settingsError, Properties.Resources.settingsErrorText, Properties.Resources.settingsErrorFolder, Properties.Resources.settingsErrorTitle);
            }
        }

        private void startDownload(DateTime start, DateTime stop, string folder) {
            Hosts host = new Hosts();
            double startStamp = Common.toTimestamp(start);
            double endStamp = Common.toTimestamp(stop);
            string ip = Properties.Settings.Default.IPAddress;
            string uri = (String.Format("http://{0}/cgi-bin/nagios3/trends.cgi?createimage&t1={1}&t2={2}&assumeinitialstates=yes&assumestatesduringnotrunning=yes&initialassumedhoststate=0&initialassumedservicestate=0&assumestateretention=yes&includesoftstates=no&host={3}&backtrack=4&zoom=4", ip, startStamp, endStamp, "{0}"));
            // well this won't work anymore
            if (!String.IsNullOrEmpty(cmbHostGrp.SelectedItem.ToString())) {
                Progress operation = new Progress();
                operation.dlUri = uri;
                operation.folderPath = folder;
                operation.hostsTable = host.GetTableFromName(cmbHostGrp.SelectedItem.ToString());
                operation.Show();
            }
        }

        private bool checkFields(DateTime? date) {
            if (date != null) {
                return true;
            } else {
                return false;
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e) {
            Settings settings = new Settings();
            settings.Show();
        }

        private void refreshCombo() {
            cmbHostGrp.ItemsSource = null;
            cmbHostGrp.Items.Clear();
            cmbHostGrp.ItemsSource = host.ListTableNames();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            folderPath = string.Empty;
            refreshCombo();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e) {
            CustomHostGrp custom = new CustomHostGrp();
            custom.ShowDialog();
            host.AddTable(custom.hostTable);
            refreshCombo();
        }

        private void btnManage_Click(object sender, RoutedEventArgs e) {
            ManageHosts manage = new ManageHosts(host);
            manage.Show();
        }
    }
}