using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.App_Data {    
    
    public class Syncable {

        //TODO put two more values, CreateDate, and LastModified.

        [Key]
        public Int64 SyncableID { get; set; }
        public int RemoteID { get; set; }
        public string FileName { get; set; }
        public string LocalPath { get; set; }
        public DateTime LastModified { get; set; }
    }
}
