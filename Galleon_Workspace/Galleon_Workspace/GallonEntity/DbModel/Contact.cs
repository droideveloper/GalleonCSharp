using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GalleonEntity {
	/// <summary>
	/// Contact
	/// </summary>
	public class Contact {

        [Key]
		public int 			ContactID 	{ get; set; }
		public int 			CustomerID 	{ get; set; }
		public string 		ContactName { get; set; }
		public string 		Address 	{ get; set; }
		public int 		    CityID 		{ get; set; }
		public int   		CountryID 	{ get; set; }
		public string 		Phone 		{ get; set; }
		public DateTime 	CreateDate 	{ get; set; }
		public DateTime 	UpdateDate 	{ get; set; }

        [ForeignKey("CityID")]
        public virtual City City { get; set; }
        [ForeignKey("CountryID")]
        public virtual Country Country { get; set; }

	}
}

