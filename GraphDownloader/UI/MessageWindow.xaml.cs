using System;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace GraphDownloader.UI
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow
    {
        /// <summary>
        /// Standard MessageWindow constructor. All work done in calling method.
        /// </summary>
        public MessageWindow() {
            InitializeComponent();
        }

        public string BodyMessage { get; set; }

        public string HeaderMessage { get; set; }

        public string FooterMessage { get; set; }

        public string MessageCaption { get; set; }

        public VisualBrush IconBrush { get; set; }

        /// <summary>
        /// Helper method to emulate MessageBox behaviour. Builds a very simple box, using only the bodyText
        /// </summary>
        /// <param name="caption">UNUSED Window title</param>
        /// <param name="headerText">UNUSED Message header</param>
        /// <param name="bodyText">UNUSED main messages text</param>
        /// <param name="Footertext">UNUSED extra footer text</param>
        [Obsolete("Consider using ShowBox() and setting class objects in calling method", false)]
        public void ShowWin(string caption, string headerText, string bodyText, string Footertext) {
            MessageWindow mw = new MessageWindow();
            mw.mainMessage.Text = bodyText;
        }

        /// <summary>
        /// Helper method to show the actual Messagebox, after setting instance variables to correct values.
        /// </summary>
        public void ShowBox() {
            this.mainMessage.Text = BodyMessage;
            this.headerMessage.Text = HeaderMessage;
            this.footerMessage.Text = FooterMessage;
            this.Show();
        }

        private void btnDetailsArrow_Click(object sender, RoutedEventArgs e) {
            var flyout = this.Flyouts.Items[0] as Flyout;
            flyout.IsOpen = !flyout.IsOpen;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        /// <summary>
        /// Copys error or information detail to the clipboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>disables the Copy button after use.</remarks>
        private void btnCopy_Click(object sender, RoutedEventArgs e) {
            string devMessage = this.Title.ToString() + Environment.NewLine + this.mainMessage + Environment.NewLine + this.footerMessage.Text.ToString();
            Clipboard.SetText(devMessage);
            btnCopy.IsEnabled = false;
        }
    }
}