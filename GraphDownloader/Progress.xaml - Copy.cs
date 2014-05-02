using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;

namespace GraphDownloader
{
    /// <summary>
    /// Interaction logic for Progress.xaml
    /// </summary>
    public partial class Progress : MetroWindow
    {
        public static string[] array = null;
        public static string uri = null;
        public static string folderPath = null;
        public Progress()
        {
            InitializeComponent();
        }

        public Progress(String[] c_array, string c_uri, string c_folderPath)
        {
            InitializeComponent();
            btnOK.Visibility = System.Windows.Visibility.Hidden;
            array = c_array;
            uri = c_uri;
            folderPath = c_folderPath;

            
        }

        private void startDownload(string[] array, string uri, string folderPath)
        {
            int current = 0;
            int count = array.Length;
            foreach (string host in array)
            {
                string destFile = String.Format("{0}/{1}-graph.png", folderPath, host);
                Uri fullUri = new Uri(String.Format(uri, host));
                current++;
                var notifier = new AutoResetEvent(false);
                prgTotal.Value = (current / count) * 100;
                WebClient wc = new WebClient();
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("nagios" + ":" + "bpc.0101"));
                //wc.Credentials = new NetworkCredential("nagios", "bpc.0101");
                wc.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
                wc.DownloadFileCompleted += delegate(object sender, AsyncCompletedEventArgs e)
                {
                    
                    prgSingle.Value = 0;
                    string[] row = { host, "Completed" };
                    lstFiles.Items.Add(row);
                    notifier.Set();
                };
                wc.DownloadProgressChanged += delegate(object sender, DownloadProgressChangedEventArgs e)
                {
                    prgSingle.Value = e.ProgressPercentage;
                };
                wc.DownloadFileAsync(fullUri, destFile);
                notifier.WaitOne();
                
            }
            btnOK.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            startDownload(array, uri, folderPath);
        }
    }
}
