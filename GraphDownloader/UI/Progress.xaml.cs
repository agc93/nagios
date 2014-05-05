using System;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Text;
using System.Windows;
using GraphDownloader.Shared;
using MahApps.Metro.Controls;

namespace GraphDownloader.UI
{
    /// <summary>
    /// Interaction logic for Progress.xaml
    /// </summary>
    public partial class Progress : MetroWindow
    {
        private DataTable _hostsTable;
        private string _uri;
        private string _path;
        private MainWindow mw;
        private BackgroundWorker bw = new BackgroundWorker();

        public string dlUri {
            get { return _uri; }
            set { _uri = value; }
        }

        public DataTable hostsTable {
            get { return _hostsTable; }
            set { _hostsTable = value; }
        }

        public string folderPath {
            get { return _path; }
            set { _path = value; }
        }

        public Progress() {
            mw = (MainWindow)this.Owner;
            InitializeComponent();
            btnOK.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e) {
            mw.Activate();
            this.Close();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e) {
            btnStart.IsEnabled = false;
            launchThread();
        }

        private void launchThread() {
            setUpWorker(bw);
            bw.RunWorkerAsync();
        }

        private void setUpWorker(BackgroundWorker bw) {
            bw.WorkerReportsProgress = true;
            bw.DoWork += bw_DoWork;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Error == null) {
                btnOK.Visibility = System.Windows.Visibility.Visible;
                addMessage("All downloads completed");
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            string[] progress = (string[])e.UserState;
            string message = progress[0] + "Completed";
            addMessage(message);
            int total = Convert.ToInt32(progress[1]);
            prgTotal.Value = (((double)e.ProgressPercentage / total) * 100);
        }

        private void addMessage(string message) {
            lstFiles.Text += Environment.NewLine + message;
            lstFiles.Focus();
            scrlViewer.ScrollToBottom();
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e) {
            try {
                int current = 0;
                foreach (DataRow row in hostsTable.Rows) {
                    string host = row.ItemArray[0].ToString();
                    string destFile = String.Format("{0}/{1}-graph.png", folderPath, host);
                    Uri fullUri = new Uri(dlUri + host);
                    current++;
                    try {
                        WebClient wc = new WebClient();
                        wc = AddAuthentication(wc);
                        wc.DownloadFile(fullUri, destFile);
                        string[] progReport = new string[2];
                        progReport[0] = host;
                        progReport[1] = hostsTable.Rows.Count.ToString();
                        bw.ReportProgress(current, progReport);
                    }
                    catch (WebException ex) {
                        Common.MsgAdvanced(Properties.Resources.appError, Properties.Resources.dlError, (ex.InnerException + Environment.NewLine + ex.Message), Properties.Resources.settingsErrorTitle);
                        btnOK.Content = "Close";
                        break;
                    }
                    catch (Exception ex) {
                        Common.MsgAdvanced(Properties.Resources.appError, Properties.Resources.unknownError, (ex.InnerException.ToString() + Environment.NewLine + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString()), Properties.Resources.settingsErrorTitle);
                        btnOK.Content = "Close";
                        break;
                    }
                }
                return;
            }
            catch (Exception ex) {
                Common.MsgAdvanced(Properties.Resources.appError, Properties.Resources.unknownError, (ex.InnerException.ToString() + Environment.NewLine + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString()), Properties.Resources.settingsErrorTitle);
                this.Close();
            }
        }

        private static WebClient AddAuthentication(WebClient wc) {
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(Properties.Settings.Default.UserName + ":" + Properties.Settings.Default.Password));
            wc.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            return wc;
        }
    }
}