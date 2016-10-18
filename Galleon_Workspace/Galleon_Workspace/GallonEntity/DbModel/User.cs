using System;
using System.ComponentModel.DataAnnotations;
namespace GalleonEntity {
	/// <summary>
	/// User
	/// </summary>
	public class User {

        [Key]
		public int 		UserID 		{ get; set; }
		public string 	Username 	{ get; set; }
		public string 	Password 	{ get; set; }
		public DateTime CreateDate 	{ get; set; }
		public DateTime UpdateDate 	{ get; set; }
	}
}

