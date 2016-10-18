using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra {

    public class CategoryEntity : IPropertyChanged {

        [JsonIgnore]
        private int categoryID;
        [JsonIgnore]
        private string categoryName;

        public int CategoryID {
            get {
                return categoryID;
            }
            set {
                setProperty(ref categoryID, value);
            }
        }

        public string CategoryName {
            get {
                return categoryName;
            }
            set {
                setProperty(ref categoryName, value);
            }
        }
    }
}
