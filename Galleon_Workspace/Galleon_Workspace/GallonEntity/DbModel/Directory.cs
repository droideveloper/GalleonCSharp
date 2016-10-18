using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GalleonEntity {
	/// <summary>
	/// Directory
	/// </summary>
	public class Directory {

        [Key]
		public int 		DirectoryID 		{ get; set; }
		public int? 	ParentDirectoryID 	{ get; set; }
		public int 		CustomerID 			{ get; set; }
		public int 		UserID 				{ get; set; }
		public string 	DirectoryName 		{ get; set; }
		public DateTime CreateDate 			{ get; set; }
		public DateTime UpdateDate 			{ get; set; }

		//FK
        [ForeignKey("ParentDirectoryID")]
		public virtual Directory 				Parent 		{ get; set; }
		public virtual ICollection<Document> 	Documents 	{ get; set; }
	}
}

