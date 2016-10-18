using System;
using System.ComponentModel.DataAnnotations;
namespace GalleonEntity {
	/// <summary>
	/// Document
	/// </summary>
	public class Document {

        [Key]
		public int 		DocumentID 		{ get; set; }
		public int 		DirectoryID 	{ get; set; }
		public int 		UserID 			{ get; set; }
		public int 		CustomerID 		{ get; set; }
		public string 	DocumentName 	{ get; set; }
		public string 	ContentType 	{ get; set; }
		public long 	ContentLength 	{ get; set; }
		public DateTime CreateDate 		{ get; set; }
		public DateTime UpdateDate 		{ get; set; }
	}
}

