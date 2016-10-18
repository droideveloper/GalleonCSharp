using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra {
    
    
    public class LoginEntity {

        [JsonIgnore]
        private string userName;
        [JsonIgnore]
        private string password;

        public string UserName {
            get {
                return userName;
            }
            set {
                userName = value;
            }
        }

        public string Password {
            get {
                return password;
            }
            set {
                password = value;
            }
        }
    }
}
