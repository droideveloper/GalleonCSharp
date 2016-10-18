using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Managers {
    
    public class PreferenceManager : IPreferenceManager {

        private readonly string STORE_PATH;
        private const string FILE_NAME  = "configs.xml";
        private const string TABLE_NAME = "SharedPreferences";

        public const string KEY_USER_NAME       = "UserName";
        public const string KEY_PASSWORD        = "Password";
        public const string KEY_REMEMBER_ME     = "RememberMe"; 
        public const string KEY_SYNC_ALLOWED    = "Sync";
        public const string KEY_SYNC_FOLDER     = "SyncDirectory";
        public const string KEY_TOOLS_FOLDER    = "ToolsDirectory";

        private Dictionary<string, object> mPreferences;

        public PreferenceManager() {
            mPreferences = new Dictionary<string, object>();
            STORE_PATH = System.AppDomain.CurrentDomain.BaseDirectory;
            //reading defaults or saved ones
            if(!File.Exists(toFilePath(STORE_PATH, FILE_NAME)))  { 
                loadDefaults(); 
            } else { 
                read();
            }
        }

        public bool hasKey(string key) {
            return mPreferences.ContainsKey(key);
        }

        public void putValue<T>(string key, T value) {
            if(!hasKey(key)) {
                mPreferences.Add(key, value);
            } else {
                mPreferences[key] = value;
            }
        }

        public T getValue<T>(string key, T defaultValue) {
            KeyValuePair<string,object> pair = mPreferences.FirstOrDefault(x => x.Key.Equals(key));
            return (pair.Key == null) ? defaultValue : (T) pair.Value; 
        }

        public void read() {
            DataTable table = new DataTable(TABLE_NAME);
            table.ReadXml(toFilePath(STORE_PATH, FILE_NAME));
            foreach(DataRow row in table.Rows) { 
                foreach(DataColumn column in table.Columns) {
                    mPreferences.Add(column.ColumnName, row[column]);
                }
            }
        }

        public void write() {
            DataTable table = new DataTable(TABLE_NAME);
            //create columns
            foreach(string key in mPreferences.Keys) {
                DataColumn newColumn = new DataColumn();
                newColumn.ColumnName = key;
                newColumn.DataType = mPreferences.FirstOrDefault(x => x.Key.Equals(key)).Value.GetType();
                table.Columns.Add(newColumn);
            }
            //insert row
            DataRow row = table.NewRow();
            foreach(string key in mPreferences.Keys) {
                row[key] = mPreferences.FirstOrDefault(x => x.Key.Equals(key)).Value;
            }
            table.Rows.Add(row);
            table.WriteXml(toFilePath(STORE_PATH, FILE_NAME), XmlWriteMode.WriteSchema);
        }

        public void loadDefaults() {
            putValue(KEY_USER_NAME, string.Empty);
            putValue(KEY_PASSWORD, string.Empty);
            putValue(KEY_REMEMBER_ME, false);
            putValue(KEY_SYNC_ALLOWED, true);
            putValue(KEY_SYNC_FOLDER, string.Empty);
            putValue(KEY_TOOLS_FOLDER, string.Empty);
        }

        private string toFilePath(string folder, string file) {
            return Path.Combine(folder, file);
        }                    
    }
}
