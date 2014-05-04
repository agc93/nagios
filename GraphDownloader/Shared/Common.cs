using System;
using System.Windows;
using GraphDownloader.UI;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using win = Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.Win32;
using winForms = System.Windows.Forms;

namespace GraphDownloader.Shared
{
    public static class Common
    {
        public static void MsgSimple(string message) {
            /* MessageBox.Show(message); */
            MessageWindow mw = new MessageWindow();
            mw.Title = "";
            mw.BodyMessage = message;
            mw.panelDetails.Visibility = Visibility.Hidden;
            mw.DataContext = mw;
            mw.ShowDialog();
            return;
        }

        async public static void MsgDialog(object sender, string message) {
            MetroWindow mw = sender as MetroWindow;
            await mw.ShowMessageAsync(String.Empty, message);
        }

        internal static void MsgAdvanced(string header, string body, string footer, string title) {
            MessageWindow mw = new MessageWindow();
            mw.Title = title;
            mw.HeaderMessage = header;
            mw.BodyMessage = body;
            mw.FooterMessage = footer;
            /*
            switch (type)
            {
                case "error":
                    mw.iconBtnErr.Visibility = Visibility.Visible;
                    mw.iconBtnExcl.Visibility = Visibility.Collapsed;
                    break;
                case "info":
                    mw.iconBtnInfo.Visibility = Visibility.Visible;
                    mw.iconBtnExcl.Visibility = Visibility.Collapsed;
                    break;
                default:
                    mw.iconBtnErr.Visibility = Visibility.Collapsed;
                    mw.iconBtnInfo.Visibility = Visibility.Collapsed;
                    mw.iconBtnExcl.Visibility = Visibility.Visible;
                    break;
            } */
            //mw.IconBrush = new VisualBrush() { Visual = (Visual)[icon] };
            mw.DataContext = mw;
            mw.ShowDialog();
            return;
        }

        [Obsolete("Use Common.ChooseFolder() instead")]
        internal static string ChooseFolderTd() {
            if (win.CommonFileDialog.IsPlatformSupported == true) {
                var dialog = new win.CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                win.CommonFileDialogResult result = dialog.ShowDialog();
                if (result == win.CommonFileDialogResult.Ok) {
                    Properties.Settings.Default.FolderPath = dialog.FileName;
                    return dialog.FileName.ToString();
                }
                return null;
            } else {
                var oldDialog = new winForms.FolderBrowserDialog();
                winForms.DialogResult result = oldDialog.ShowDialog();
                if (result == winForms.DialogResult.OK) {
                    Properties.Settings.Default.FolderPath = oldDialog.SelectedPath.ToString();
                    return oldDialog.SelectedPath.ToString();
                }
                return null;
            }
        }

        internal static string ChooseFolder() {
            return null;
        }

        [Obsolete("use Common.ChooseFile() instead")]
        internal static string[] ChooseFileTd() {
            if (win.CommonFileDialog.IsPlatformSupported == true) {
                var dialog = new win.CommonOpenFileDialog();
                dialog.IsFolderPicker = false;
                dialog.DefaultExtension = ".txt";
                win.CommonFileDialogResult result = dialog.ShowDialog();
                if (result == win.CommonFileDialogResult.Ok) {
                    string[] hosts = ReadHostFile(dialog.FileName);
                    return hosts;
                }
            } else {
                var oldDialog = new winForms.OpenFileDialog();
                winForms.DialogResult result = oldDialog.ShowDialog();
                if (result == winForms.DialogResult.OK) {
                    string[] hosts = ReadHostFile(oldDialog.FileName);
                    return hosts;
                }
            }
            return null;
        }

        internal static string[] ChooseFile() {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text documents (.txt)|*.txt";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true) {
                string[] hosts = ReadHostFile(dlg.FileName);
                return hosts;
            }
            return null;
            
        }

        internal static string[] ReadHostFile(string filePath) {
            string[] lines = System.IO.File.ReadAllLines(filePath);
            return lines;
        }

        internal static double toTimestamp(DateTime value) {
            //create span since unix epoch
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0));
            //return seconds since (timestamp)
            return (double)span.TotalSeconds;
        }
    }
}