using GalleonEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GalleonEndpoint.Proxy {
   
    [DataContract]
    public class CustomerProxy {

        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public int CategoryID { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Identity { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<ContactProxy> Contacts { get; set; }
        [DataMember]
        public CategoryProxy Category { get; set; }

        public CustomerProxy(Customer customer) {
            CustomerID = customer.CustomerID;
            FirstName = customer.Firstname;
            LastName = customer.Lastname;
            Identity = customer.Identity;
            Contacts = customer.Contacts.IsNullOrEmpty() ? null : ContactProxy.FromContacts(customer.Contacts.ToList());
            CategoryID = customer.CategoryID;
            Category = new CategoryProxy(customer.Category);
        }

        public static List<CustomerProxy> FromCustomers(List<Customer> customers) {
            return customers.IsNullOrEmpty() ? null : customers.Select(c => new CustomerProxy(c)).ToList();
        }
    }
}