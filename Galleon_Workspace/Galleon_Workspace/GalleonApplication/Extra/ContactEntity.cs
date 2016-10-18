using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra {

    public class ContactEntity : IPropertyChanged {

        [JsonIgnore]
        private string contactName;
        [JsonIgnore]
        private string address;
        [JsonIgnore]
        private string phone;
        [JsonIgnore]
        private CountryEntity country;
        [JsonIgnore]
        private CityEntity city;
        [JsonIgnore]
        private int customerID;
        [JsonIgnore]
        private int cityID;
        [JsonIgnore]
        private int countryID;

        public int CityID {
            get {
                return cityID;
            }
            set {
                setProperty(ref cityID, value);
            }
        }

        public int CountryID {
            get {
                return countryID;
            }
            set {
                setProperty(ref countryID, value);
            }
        }

        public string ContactName {
            get {
                return contactName;
            }
            set {
                setProperty(ref contactName, value);
            }
        }

        public string Address {
            get {
                return address;
            }
            set {
                setProperty(ref address, value);
            }
        }

        public int CustomerID {
            get {
                return customerID;
            }
            set {
                setProperty(ref customerID, value);
            }
        }

        public string Phone {
            get {
                return phone;
            }
            set {
                setProperty(ref phone, value);
            }
        }

        public CountryEntity Country {
            get {
                return country;
            }
            set {
                setProperty(ref country, value);
            }
        }

        public CityEntity City {
            get {
                return city;
            }
            set {
                setProperty(ref city, value);
            }
        }
    }
}
