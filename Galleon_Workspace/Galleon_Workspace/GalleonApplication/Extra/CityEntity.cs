using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra {
    
    public class CityEntity : IPropertyChanged {

        [JsonIgnore]
        private int cityID;
        [JsonIgnore]
        private string cityName;

        public int CityID {
            get {
                return cityID;
            }
            set {
                setProperty(ref cityID, value);
            }
        }

        public string CityName {
            get {
                return cityName;
            }
            set {
                setProperty(ref cityName, value);
            }
        }
    }
}
