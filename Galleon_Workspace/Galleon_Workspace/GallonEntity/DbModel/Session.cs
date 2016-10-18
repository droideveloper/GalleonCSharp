using System;
using System.ComponentModel.DataAnnotations;
namespace GalleonEntity {
	/// <summary>
	/// Session
	/// </summary>
	public class Session {

        [Key]
		public int 		SessionID 	{ get; set; }
		public int 		UserID 		{ get; set; }
		public string 	Token 		{ get; set; }
		public DateTime CreateDate 	{ get; set; }
		public DateTime DueDate 	{ get; set; }
	}
}

