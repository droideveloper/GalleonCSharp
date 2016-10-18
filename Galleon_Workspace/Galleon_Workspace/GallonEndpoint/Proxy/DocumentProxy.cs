using GalleonEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GalleonEndpoint.Proxy {
    
    [DataContract]
    public class DocumentProxy {

        [DataMember]
        public int DocumentID { get; set; }
        [DataMember]
        public int DirectoryID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string DocumentName { get; set; }
        [DataMember]
        public string ContentType { get; set; }
        [DataMember]
        public long ContentLength { get; set; }
        [DataMember]
        public DateTime CreateDate { get; set; }
        [DataMember]
        public DateTime UpdateDate { get; set; }

        public DocumentProxy(Document document) {
            DocumentID = document.DocumentID;
            DirectoryID = document.DirectoryID;
            CustomerID = document.CustomerID;
            DocumentName = document.DocumentName;
            ContentType = document.ContentType;
            ContentLength = document.ContentLength;
            CreateDate = document.CreateDate;
            UpdateDate = document.UpdateDate;
        }

        public static List<DocumentProxy> FromDocuments(List<Document> documents) { 
            return documents.IsNullOrEmpty() ? null : documents.Select(d => new DocumentProxy(d)).ToList();
        }
    }
}