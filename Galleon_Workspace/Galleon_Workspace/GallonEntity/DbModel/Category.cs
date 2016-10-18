using System;
using System.ComponentModel.DataAnnotations;
namespace GalleonEntity {
	/// <summary>
	/// Category
	/// </summary>
	public class Category {
		
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
	}
}

