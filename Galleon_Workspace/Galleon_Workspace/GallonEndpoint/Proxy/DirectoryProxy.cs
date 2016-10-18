using GalleonEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GalleonEndpoint.Proxy {
    
    [DataContract]
    public class DirectoryProxy {
       
        [DataMember]
        public int DirectoryID { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int? ParentDirectoryID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string DirectoryName { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public DirectoryProxy Parent { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<DocumentProxy> Documents { get; set; }

        public DirectoryProxy(Directory directory) {
            DirectoryID = directory.DirectoryID;
            ParentDirectoryID = directory.ParentDirectoryID;
            CustomerID = directory.CustomerID;
            DirectoryName = directory.DirectoryName;
            Parent = directory.Parent.IsNullOrEmpty() ?  null : new DirectoryProxy(directory.Parent);
            Documents = directory.Documents.IsNullOrEmpty() ? null : DocumentProxy.FromDocuments(directory.Documents.ToList());
        }

        public static List<DirectoryProxy> FromDirectories(List<Directory> directories) {
            return directories.IsNullOrEmpty() ? null : directories.Select(d => new DirectoryProxy(d)).ToList();
        }
    }
}