using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using GalleonEntity;

namespace GalleonEndpoint.Proxy {
    
    [DataContract]
    public class CountryProxy {

        [DataMember]
        public int CountryID { get; set; }
        [DataMember]
        public string CountryName { get; set; }

        public CountryProxy(Country country) {
            CountryID = country.CountryID;
            CountryName = country.CountryName;
        }

        public static List<CountryProxy> FromCountries(List<Country> countries) {
            return countries.IsNullOrEmpty() ? null : countries.Select(c => new CountryProxy(c)).ToList();
        }
    }
}