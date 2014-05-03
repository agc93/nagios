using System;
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
        public static string[] hostArray = null;

        public MainWindow() {
            if (Common.checkDataFile()) {
                InitializeComponent();
                Properties.Settings.Default.FolderPath = null;
            } else {
                Application.Current.Shutdown();
            }

            //startUp();
        }

        private void startUp() {
            RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\NET Framework Setup\NDP");
            string[] version_names = installed_versions.GetSubKeyNames();
            double Framework = Convert.ToDouble(version_names[version_names.Length - 1].Remove(0, 1));
            if (Framework < 4) {
                MessageBox.Show(Properties.Resources.fwError);
                Application.Current.Shutdown();
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e) {
            Common.ChooseFolder();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e) {
            if (Properties.Settings.Default.FolderPath != null) {
                if ((checkFields(pkrStartDate.SelectedDate)) && (checkFields(pkrEndDate.SelectedDate)) && (pkrStartDate.SelectedDate < pkrEndDate.SelectedDate)) {
                    if (cmbHostGrp.SelectedIndex != 2) {
                        startDownload((DateTime)pkrStartDate.SelectedDate, (DateTime)pkrEndDate.SelectedDate, Properties.Settings.Default.FolderPath.ToString());
                    } else {
                        //pick another host group
                        Common.taskDialogAdv(Properties.Resources.settingsError, Properties.Resources.settingsErrorText, Properties.Resources.settingsErrorHosts, Properties.Resources.settingsErrorTitle);
                    }
                } else {
                    //wrong dates
                    Common.taskDialogAdv(Properties.Resources.settingsError, Properties.Resources.settingsErrorText, Properties.Resources.settingsErrorDate, Properties.Resources.settingsErrorTitle);
                }
            } else {
                //pick a folder
                Common.taskDialogAdv(Properties.Resources.settingsError, Properties.Resources.settingsErrorText, Properties.Resources.settingsErrorFolder, Properties.Resources.settingsErrorTitle);
            }
        }

        private void startDownload(DateTime start, DateTime stop, string folder) {
            string[] array = null;
            double startStamp = Common.toTimestamp(start);
            double endStamp = Common.toTimestamp(stop);
            string ip = Properties.Settings.Default.IPAddress;
            string uri = (String.Format("http://{0}/cgi-bin/nagios3/trends.cgi?createimage&t1={1}&t2={2}&assumeinitialstates=yes&assumestatesduringnotrunning=yes&initialassumedhoststate=0&initialassumedservicestate=0&assumestateretention=yes&includesoftstates=no&host={3}&backtrack=4&zoom=4", ip, startStamp, endStamp, "{0}"));
            switch (cmbHostGrp.SelectedIndex) {
                case 0:
                    //yellow cabs
                    array = Common.yellowCabs; //formerly Common.hostArray
                    break;
                case 1:
                    //BAC
                    array = Common.bneAirport;
                    break;
                case 3:
                    CustomHostGrp custom = new CustomHostGrp();
                    custom.ShowDialog();
                    array = hostArray;
                    hostArray = null;
                    break;
                case 2:
                //thats a separator, you twit.
                default:
                    //pick something, you twit.
                    MessageBox.Show("Choose a host group before continuing.");
                    break;
            }
            if (array != null) {
                Progress operation = new Progress(array, uri, Properties.Settings.Default.FolderPath);
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
    }
}