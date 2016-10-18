using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra {
    
    public class CountryEntity : IPropertyChanged {

        [JsonIgnore]
        private int countryID;
        [JsonIgnore]
        private string countryName;

        public int CountryID {
            get {
                return countryID;
            }
            set {
                setProperty(ref countryID, value);
            }
        }

        public string CountryName {
            get {
                return countryName;
            }
            set {
                setProperty(ref countryName, value);
            }
        }
    }
}
