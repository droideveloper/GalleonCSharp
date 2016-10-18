using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra {
    
    public class CustomerEntity : IPropertyChanged {

        [JsonIgnore]
        private string firstName;
        [JsonIgnore]
        private string middleName;
        [JsonIgnore]
        private string lastName;
        [JsonIgnore]
        private string identity;
        [JsonIgnore]
        private int categoryID;
        [JsonIgnore]
        private int customerID;

        [JsonIgnore]
        private CategoryEntity category;
        [JsonIgnore]
        private ContactEntities contacts;

        public int CustomerID {
            get {
                return customerID;
            }
            set {
                setProperty(ref customerID, value);
            }
        }

        public string FirstName {
            get {
                return firstName;
            }
            set {
                setProperty(ref firstName, value);
            }
        }

        public string MiddleName {
            get {
                return middleName;
            }
            set {
                setProperty(ref middleName, value);
            }
        }

        public string LastName {
            get {
                return lastName;
            }
            set {
                setProperty(ref lastName, value);
            }
        }

        public string Identity {
            get {
                return identity;
            }
            set {
                setProperty(ref identity, value);
                
            }
        }

        public int CategoryID {
            get {
                return categoryID;
            }
            set {
                setProperty(ref categoryID, value);
            }
        }

        public CategoryEntity Category {
            get {
                return category;
            }
            set {
                setProperty(ref category, value);
            }
        } 

        public ContactEntities Contacts {
            get {
                return contacts;
            }
            set {
                setProperty(ref contacts, value);
            }
        }
    }
}
