using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.App_Data {
    
    public class LogException {

        [Key]
        public int LogExceptionID { get; set; }
        public int? ParentLogExceptionID { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime CreateDate { get; set; }

        [ForeignKey("ParentLogExceptionID")]
        public virtual LogException Parent { get; set; }
    }
}
