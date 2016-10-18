using GalleonEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GalleonEndpoint.Proxy {
    
    [DataContract]
    public class CityProxy {

        [DataMember]
        public int CityID { get; set; }
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]
        public string CityName { get; set; }

        public CityProxy(City city) {
            CityID = city.CityID;
            CountryID = city.CountryID;
            CityName = city.CityName;
        }

        public static List<CityProxy> FromCities(List<City> cities) {
            return cities.IsNullOrEmpty() ? null : cities.Select(c => new CityProxy(c)).ToList();
        }
    }
}