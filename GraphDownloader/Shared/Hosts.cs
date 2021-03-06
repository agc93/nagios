﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace GraphDownloader.Shared
{
    internal class Hosts
    {
        private IsolatedStorageFile storageFile;
        private DataSet _hostsSet;

        private DataSet hostsSet {
            get { return _hostsSet; }
            set {
                _hostsSet = value;
                //SaveHosts(_hostsSet);
            }
        }

        public Hosts() {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming, null, null);
            this.storageFile = isoStore;
            if (CheckDataFile()) {
                LoadHosts();
            }
        }

        #region static methods
        #endregion static methods

        #region non-static methods

        internal bool CheckDataFile() {
            try {
                if (storageFile.FileExists(Properties.Resources.connString)) {
                    return true;
                } else {
                    SaveHosts(new DataSet());
                    return true;
                }
            }
            catch (Exception ex) {
                Common.MsgAdvanced(Properties.Resources.dataFileError1, Properties.Resources.appError, ex.InnerException.ToString(), Properties.Resources.settingsErrorTitle);
                return false;
            }
        }

        private void LoadHosts() {
            FileStream stream = loadFileStream();
            DataSet ds = new DataSet();
            ds = LoadFromStream(ds, stream);
            //ds.Tables.Add("test-table");
            this.hostsSet = ds;
        }

        private DataSet LoadFromStream(DataSet ds, FileStream stream) {
            XmlSerializer serz = new XmlSerializer(typeof(DataSet));
            ds = (DataSet)serz.Deserialize(stream);
            return ds;
        }

        internal FileStream loadFileStream() {
            try {
                IsolatedStorageFileStream loadStream = new IsolatedStorageFileStream(Properties.Resources.connString, FileMode.Open, storageFile);
                return loadStream;
            }
            catch (Exception ex) {
                Common.MsgSimple("The hosts data file is currently in use. Please wait a moment and try again");
                return null;
            }
        }

        private bool SaveHosts(DataSet ds) {
            XmlSerializer serz = new XmlSerializer(typeof(DataSet));
            using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(Properties.Resources.connString, FileMode.Create, storageFile)) {
                using (TextWriter writer = new StreamWriter(fileStream)) {
                    try {
                        serz.Serialize(writer, ds);
                        writer.Close();
                        return true;
                    }
                    catch (Exception ex) {
                        Common.MsgAdvanced(Properties.Resources.dataFileError1, Properties.Resources.appError, ex.InnerException.ToString(), Properties.Resources.settingsErrorTitle);
                        return false;
                    }
                }
            }
        }

        internal void AddTable(DataTable dataTable) {
            if (dataTable != null) {
                hostsSet.Tables.Add(dataTable);
                SaveHosts(hostsSet);
            }
        }

        internal List<string> ListTableNames() {
            List<string> names = new List<string>();
            foreach (DataTable table in hostsSet.Tables) {
                names.Add(table.TableName);
            }
            return names;
        }

        internal DataTable ListTableDetails() {
            DataTable names = new DataTable();
            names.Columns.Add("Group Name");
            names.Columns.Add("Hosts");
            foreach (DataTable table in hostsSet.Tables) {
                DataRow row = names.NewRow();
                names.Rows.Add(table.TableName, table.Rows.Count);
            }
            return names;
        }

        internal DataTable GetTableFromName(string p) {
            try {
                DataTable dTable = hostsSet.Tables[p];
                return dTable;
            }
            catch (Exception) {
                throw new InvalidDataException("Table does not exist");
            }
            //need error handling here.
        }

        internal void DeleteTableName(string p) {
            DataTable dTable = hostsSet.Tables[p];
            //need error handling
            hostsSet.Tables.Remove(dTable);
            SaveHosts(hostsSet);
        }

        #endregion non-static methods
    }
}