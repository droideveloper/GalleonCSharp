using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonEntity {
    
    public class City {

        [Key]
        public int CityID { get; set; }
        public int CountryID { get; set; }
        public string CityName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
