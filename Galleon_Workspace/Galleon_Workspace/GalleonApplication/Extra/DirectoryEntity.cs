using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra {

    public class DirectoryEntity : IPropertyChanged {

        [JsonIgnore]
        private int directoryID;
        [JsonIgnore]
        private int? parentDirectoryID;
        [JsonIgnore]
        private int customerID;
        [JsonIgnore]
        private string directoryName;
        [JsonIgnore]
        private DirectoryEntity parent;
        [JsonIgnore]
        private DocumentEntities documents;

        public int DirectoryID {
            get {
                return directoryID;
            }
            set {
                setProperty(ref directoryID, value);
            }
        }

        public int? ParentDirectoryID {
            get {
                return parentDirectoryID;
            }
            set {
                setProperty(ref parentDirectoryID, value);
            }
        }

        public int CustomerID {
            get {
                return customerID;
            }
            set {
                setProperty(ref customerID, value);
            }
        }

        public string DirectoryName {
            get {
                return directoryName;
            }
            set {
                setProperty(ref directoryName, value);
            }
        }

        public DirectoryEntity Parent {
            get {
                return parent;
            }
            set {
                setProperty(ref parent, value);
            }
        }

        public DocumentEntities Documents {
            get {
                return documents;
            }
            set {
                setProperty(ref documents, value);
            }
        }
    }
}
