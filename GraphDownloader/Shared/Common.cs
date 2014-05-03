using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using winForms = System.Windows.Forms;
using win = Microsoft.WindowsAPICodePack.Dialogs;


namespace GraphDownloader.Shared
{
    public static class Common
    {
        public static String[] yellowCabs = new String[] { "Yellow_BPC_ADSL", "Yellow_Woolloongabba_LAN", "Yellow_Woolloongabba_Frame", "Yellow_Woolloongabba_NextG", "Yellow_Warwick_LAN", "Yellow_Warwick_Frame", "Yellow_Warwick_NextG", "Yellow_MtCotton_LAN", "Yellow_MtCotton_Frame", "Yellow_MtCotton_NextG", "Yellow_Toowoomba_LAN", "Yellow_Toowoomba_Frame", "Yellow_Toowoomba_NextG", "Yellow_OceanView_LAN", "Yellow_OceanView_Frame", "Yellow_OceanView_NextG", "Yellow_MtCoottha_LAN", "Yellow_MtCoottha_Frame", "Yellow_MtCoottha_NextG", "Yellow_Hobart_LAN", "Yellow_Hobart_Frame", "Yellow_Hobart_NextG", "Yellow_WaterfrontPlace_LAN", "Yellow_WaterfrontPlace_Frame", "Yellow_WaterfrontPlace_NextG", "Yellow_Ipswich_LAN", "Yellow_Ipswich_Frame", "Yellow_Ipswich_NextG" };
        public static String[] bneAirport = new String[] { "BACS1_HQ_L2_AVGW_01", "BACS3-ITB-PA-VGW-01", "BACS1-SPN-L1-AVGW-02", "BACS2-DTB-PA-VGW-01", "BACS1-SPN-L1-AVGW-01", "BACS1-SPN-L2-AVGW-01", "BACS1-SPN-L2-AVGW-02", "BACS2-DTB-PA-AVGW-01", "BACS2-DTB-PA-AVGW-02", "BACS2-MLCP-P2-EXBTH-AVGW-01", "BACS2-AMC5-AVGW-01", "BACS2-AMC5-AVGW-02", "BACS3-ITB-PA-AVGW-01", "BACS3-ITB-PA-AVGW-02", "BACS3-ITB-PA-AVGW-03", "MGTUCMESX01", "MGTUCMESX02", "BACUCMESX01", "BACUCMESX02", "BACVSCUCM01", "BACVSCUCM02", "BACVSCUC01", "BACVSCUC02", "BACVSCUP01", "BACVSCUP02", "BACVSUCCX01", "BACVSUCCX02", "BACVSCUBAC01" };
        
        internal static void ChooseFolder()
        {
            if (win.CommonFileDialog.IsPlatformSupported == true)
            {
                var dialog = new win.CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                win.CommonFileDialogResult result = dialog.ShowDialog();
                if (result == win.CommonFileDialogResult.Ok)
                {
                    Properties.Settings.Default.FolderPath = dialog.FileName;
                }
            }
            else
            {
                var oldDialog = new winForms.FolderBrowserDialog();
                winForms.DialogResult result = oldDialog.ShowDialog();
                if (result == winForms.DialogResult.OK)
                {
                    Properties.Settings.Default.FolderPath = oldDialog.SelectedPath.ToString();
                }
            }
        }

        internal static string[] ChooseFile()
        {
            if (win.CommonFileDialog.IsPlatformSupported == true)
            {
                var dialog = new win.CommonOpenFileDialog();
                dialog.IsFolderPicker = false;
                dialog.DefaultExtension = ".txt";
                win.CommonFileDialogResult result = dialog.ShowDialog();
                if (result == win.CommonFileDialogResult.Ok)
                {
                    string[] hosts = ReadHostFile(dialog.FileName);
                    return hosts;
                }
            }
            else
            {
                var oldDialog = new winForms.OpenFileDialog();
                winForms.DialogResult result = oldDialog.ShowDialog();
                if (result == winForms.DialogResult.OK)
                {
                    string[] hosts = ReadHostFile(oldDialog.FileName);
                    return hosts;
                }
            }
            return null;
        }

        internal static string[] ReadHostFile(string filePath)
        {
            string[] lines = System.IO.File.ReadAllLines(filePath);
            return lines;
        }

        internal static double toTimestamp(DateTime value)
        {
            //create span since unix epoch
            //value.TimeOfDay = 
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0));
            //return seconds since (timestamp)
            return (double)span.TotalSeconds;
        }

        public static void taskDialogSimple(string mainText, string caption, string footerText)
        {
            if (win.TaskDialog.IsPlatformSupported == true)
            {
                win.TaskDialog td = new win.TaskDialog();
                td.InstructionText = mainText;
                td.Text = caption;
                td.FooterText = footerText;
                td.Show();
            }
            else
            {
                winForms.MessageBox.Show(mainText + Environment.NewLine + caption + Environment.NewLine + footerText);
            }
            
        }

        public static void taskDialogAdv(string mainText, string caption, string footerText, string windowTitle)
        {
            if (win.TaskDialog.IsPlatformSupported == true)
            {
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
            }
            else
            {
                winForms.MessageBox.Show((mainText + Environment.NewLine + caption + Environment.NewLine + footerText), windowTitle);
            }
            
        }
    }
}
