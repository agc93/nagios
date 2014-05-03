using System;
using System.Data;
using System.IO;
using System.Xml.Serialization;

namespace GraphDownloader.Shared
{
    internal class Hosts
    {
        internal string filePath = Properties.Resources.connString;
        private DataSet _hostsSet;

        public DataSet hostsSet {
            get { return _hostsSet; }
            set {
                _hostsSet = value;
                SaveHosts(_hostsSet);
            }
        }

        public Hosts() {
            LoadHosts();
        }

        private void LoadHosts() {
            FileStream stream = loadFileStream();
            DataSet ds = new DataSet();
            ds = LoadFromStream(ds, stream);
            this.hostsSet = ds;
        }

        private DataSet LoadFromStream(DataSet ds, FileStream stream) {
            XmlSerializer serz = new XmlSerializer(typeof(DataSet));
            ds = (DataSet)serz.Deserialize(stream);
            return ds;
        }

        internal FileStream loadFileStream() {
            FileStream loadStream = new FileStream(filePath, FileMode.Open);
            return loadStream;
        }

        private bool SaveHosts(DataSet ds) {
            XmlSerializer serz = new XmlSerializer(typeof(DataSet));
            TextWriter writer = new StreamWriter(filePath);
            try {
                serz.Serialize(writer, ds);
                writer.Close();
                return true;
            }
            catch (Exception ex) {
                Common.taskDialogAdv(Properties.Resources.dataFileError1, Properties.Resources.appError, ex.InnerException.ToString(), Properties.Resources.settingsErrorTitle);
                return false;
            }
        }
    }
}