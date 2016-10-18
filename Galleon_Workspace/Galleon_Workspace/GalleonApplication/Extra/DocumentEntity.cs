using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra {
    
    public class DocumentEntity : IPropertyChanged {

        [JsonIgnore]
        private int documentID;
        [JsonIgnore]
        private int directoryID;
        [JsonIgnore]
        private int customerID;
        [JsonIgnore]
        private string documentName;
        [JsonIgnore]
        private string contentType;
        [JsonIgnore]
        private long contentLength;
        [JsonIgnore]
        private DateTime createDate;
        [JsonIgnore]
        private DateTime updateDate;

        [JsonIgnore]
        private bool displayProgress;

        public int DocumentID {
            get {
                return documentID;
            }
            set {
                setProperty(ref documentID, value);
            }
        }

        public int DirectoryID {
            get {
                return directoryID;
            }
            set {
                setProperty(ref directoryID, value);
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

        public string DocumentName {
            get {
                return documentName;
            }
            set {
                setProperty(ref documentName, value);
            }
        }

        public string ContentType {
            get {
                return contentType;
            }
            set {
                setProperty(ref contentType, value);
            }
        }

        public long ContentLength {
            get {
                return contentLength;
            }
            set {
                setProperty(ref contentLength, value);
            }
        }

        public DateTime CreateDate {
            get {
                return createDate;
            }
            set {
                setProperty(ref createDate, value);
            }
        }

        public DateTime UpdateDate {
            get {
                return updateDate;
            }
            set {
                setProperty(ref updateDate, value);
            }
        }

        [JsonIgnore]//don't want
        public bool DisplayProgress {
            get {
                return displayProgress;
            }
            set {
                setProperty(ref displayProgress, value);
            }
        }
    }
}
