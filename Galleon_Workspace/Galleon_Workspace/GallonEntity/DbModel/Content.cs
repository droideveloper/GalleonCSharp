using System;
using System.ComponentModel.DataAnnotations;
namespace GalleonEntity {
	/// <summary>
	/// Content
	/// </summary>
	public class Content {

        [Key]
		public int 		ContentID 		{ get; set; }
		public int 		DocumentID 		{ get; set; }
		public int 		UserID 			{ get; set; }
		public byte[] 	ContentSink 	{ get; set; }
		public DateTime CreateDate 		{ get; set; }
		public DateTime UpdateDate 		{ get; set; }
	}
}

