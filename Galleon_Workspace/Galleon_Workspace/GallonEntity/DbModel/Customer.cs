using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GalleonEntity {
	/// <summary>
	/// Customer
	/// </summary>
	public class Customer {

        [Key]
		public int 		CustomerID 	{ get; set; }
        public int      CategoryID  { get; set; }
		public string 	Firstname 	{ get; set; }
        //public string   MiddleName { get; set; }TODO
		public string 	Lastname 	{ get; set; }
		public string 	Identity 	{ get; set; }
		public DateTime CreateDate 	{ get; set; }
		public DateTime UpdateDate 	{ get; set; }

		//FK
		public virtual ICollection<Contact> Contacts { get; set; }
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
	}
}

