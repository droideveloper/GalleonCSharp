using GalleonEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GalleonEndpoint.Proxy {

    [DataContract]
    public class ContactProxy {

        [DataMember]
        public int CountactID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string ContactName { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public int CityID { get; set; }
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]        
        public CityProxy City { get; set; }
        [DataMember]
        public CountryProxy Country { get; set; }

        public ContactProxy(Contact contact) {
            this.CountactID = contact.ContactID;
            this.CustomerID = contact.CustomerID;
            this.ContactName = contact.ContactName;
            this.CityID = contact.CityID;
            this.CountryID = contact.CountryID;
            this.Address = contact.Address;
            this.Phone = contact.Phone;
            this.City = new CityProxy(contact.City);
            this.Country = new CountryProxy(contact.Country);
        }

        public static List<ContactProxy> FromContacts(List<Contact> contacts) {
            return contacts.IsNullOrEmpty() ? null : contacts.Select(c => new ContactProxy(c)).ToList();
        }
    }
}