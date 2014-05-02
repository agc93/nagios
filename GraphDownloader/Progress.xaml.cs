using MahApps.Metro.Controls;
using System;
using System.Net;
using System.Text;
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
            try
            {
                int current = 0;
                int count = array.Length;
                foreach (string host in array)
                {
                    System.Threading.Thread.Sleep(250);
                    string destFile = String.Format("{0}/{1}-graph.png", folderPath, host);
                    Uri fullUri = new Uri(String.Format(uri, host));
                    current++;
                    prgTotal.Value = (current / count) * 100;
                    try
                    {
                        WebClient wc = new WebClient();
                        string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(Properties.Settings.Default.UserName + ":" + Properties.Settings.Default.Password));
                        wc.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
                        wc.DownloadFile(fullUri, destFile);
                        string[] row = { host, "Completed" };
                        lstFiles.Items.Add(row[0]);
                    }
                    catch (WebException ex)
                    {
                        Common.taskDialogAdv(Properties.Resources.appError, Properties.Resources.dlError, (ex.InnerException + Environment.NewLine + ex.Message), Properties.Resources.settingsErrorTitle);
                        btnOK.Content = "Close";
                        break;
                    }
                    catch (Exception ex)
                    {
                        Common.taskDialogAdv(Properties.Resources.appError, Properties.Resources.unknownError, (ex.InnerException.ToString() + Environment.NewLine + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString()), Properties.Resources.settingsErrorTitle);
                        btnOK.Content = "Close";
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.taskDialogAdv(Properties.Resources.appError, Properties.Resources.unknownError, (ex.InnerException.ToString() + Environment.NewLine + ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString()), Properties.Resources.settingsErrorTitle);
                this.Close();
            }
            
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.Visibility = System.Windows.Visibility.Hidden;
            startDownload(array, uri, folderPath);
            btnOK.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
