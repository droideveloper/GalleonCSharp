using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra {

    public class SessionEntity {

        [JsonIgnore]
        private string token;

        public string Token {
            get {
                return token;
            }

            set {
                token = value;
            }
        }
    }
}
