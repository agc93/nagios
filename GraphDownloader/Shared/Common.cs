using System;
using System.Windows;
using GraphDownloader.UI;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using win = Microsoft.WindowsAPICodePack.Dialogs;
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

        [Obsolete("use Common.MsgSimple or Common.MsgDialog instead.", true)]
        public static void taskDialogSimple(string mainText, string caption, string footerText) {
            if (win.TaskDialog.IsPlatformSupported == true) {
                win.TaskDialog td = new win.TaskDialog();
                td.InstructionText = mainText;
                td.Text = caption;
                td.FooterText = footerText;
                td.Show();
            } else {
                winForms.MessageBox.Show(mainText + Environment.NewLine + caption + Environment.NewLine + footerText);
            }
        }

        [Obsolete("use Common.MsgAdvanced instead", true)]
        public static void taskDialogAdv(string mainText, string caption, string footerText, string windowTitle) {
            if (win.TaskDialog.IsPlatformSupported == true) {
                win.TaskDialog td = new win.TaskDialog();
                td.Cancelable = false;
                td.StandardButtons = win.TaskDialogStandardButtons.Ok;
                td.DetailsExpanded = false;
                td.ExpansionMode = win.TaskDialogExpandedDetailsLocation.ExpandFooter;
                td.Icon = win.TaskDialogStandardIcon.Warning;
                td.InstructionText = mainText;
                td.Text = caption;
                td.DetailsCollapsedLabel = "More details";
                td.DetailsExpandedLabel = "Less details";
                td.DetailsExpandedText = footerText;
                td.Show();
            } else {
                winForms.MessageBox.Show((mainText + Environment.NewLine + caption + Environment.NewLine + footerText), windowTitle);
            }
        }

        internal static string ChooseFolder() {
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

        internal static string[] ChooseFile() {
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