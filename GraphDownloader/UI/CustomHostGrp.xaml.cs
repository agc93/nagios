﻿using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows;
using GraphDownloader.Shared;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace GraphDownloader.UI
{
    /// <summary>
    /// Interaction logic for CustomHostGrp.xaml
    /// </summary>
    public partial class CustomHostGrp : MetroWindow
    {
        public static string[] customHosts = null;
        private BackgroundWorker bw = new BackgroundWorker();
        private Hosts host;

        public DataTable hostTable { get; set; }

        public CustomHostGrp() {
            InitializeComponent();
            btnOK.IsEnabled = false;
        }

        internal CustomHostGrp(Hosts host) {
            InitializeComponent();
            btnOK.IsEnabled = false;
            this.host = host;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e) {
            string[] selectedHosts = Common.ChooseFile();
            if (selectedHosts != null) {
                foreach (string host in selectedHosts) {
                    lstHosts.Items.Add(host);
                }
                customHosts = selectedHosts;
            }
            this.Activate();
        }

        async private void btnOK_Click(object sender, RoutedEventArgs e) {
            if (!string.IsNullOrEmpty(txtGrpName.ToString())) {
                if (!bw.IsBusy) {
                    if (customHosts != null) {
                        DataTable dTable = new DataTable(txtGrpName.Text.ToString());
                        dTable.Columns.Add("hostname");
                        dTable = ReadArrayToTable(customHosts, dTable);
                        this.hostTable = dTable;
                        this.Close();
                    } else {
                        Common.MsgDialog(this, Properties.Resources.hostFileError);
                    }
                } else {
                    await this.ShowMessageAsync(string.Empty, "Operation in progress: please wait", MessageDialogStyle.Affirmative);
                }
            } else {
                await this.ShowMessageAsync(string.Empty, "You must provide a name for the host group", MessageDialogStyle.Affirmative);
            }
        }

        private DataTable ReadArrayToTable(string[] selectedHosts, DataTable dTable) {
            foreach (string item in selectedHosts) {
                dTable.Rows.Add(item);
            }
            return dTable;
        }

        private void txtGrpName_LostFocus(object sender, RoutedEventArgs e) {
            if (!string.IsNullOrEmpty(txtGrpName.Text)) {
                //do some checking
                launchChecker();
            }
        }

        private void txtGrpName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
            if (!string.IsNullOrEmpty(txtGrpName.Text)) {
                //do some checking
                launchChecker();
            }
        }

        private void launchChecker() {
            SetUpWorker(bw);
            btnCross.Visibility = System.Windows.Visibility.Hidden;
            btnTick.Visibility = System.Windows.Visibility.Hidden;
            prgRing.IsActive = true;
            bw.RunWorkerAsync(txtGrpName.Text.ToString());
        }
        #region thread/bw work
        private void SetUpWorker(BackgroundWorker bw) {
            bw.WorkerReportsProgress = false;

            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.DoWork += bw_DoWork;
        }

        private async void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            prgRing.IsActive = false;
            if ((bool)e.Result) {
                //table already exists
                //await this.ShowMessageAsync(String.Empty, "A group with this name already exists");
                btnCross.Visibility = System.Windows.Visibility.Visible;
                btnCross.ToolTip = "A group with this name already exists";
                btnOK.IsEnabled = false;
            } else {
                //table is new
                btnTick.Visibility = System.Windows.Visibility.Visible;
                btnOK.IsEnabled = true;
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e) {
            string name = (string)e.Argument;
            try {
                DataTable table = host.GetTableFromName(name);
                if (table != null) {
                    e.Result = true;
                    return;
                } else {
                    e.Result = false;
                    return;
                }
            }
            catch (InvalidDataException) {
                e.Result = false;
                return;
            }
        }

        #endregion
    }
}