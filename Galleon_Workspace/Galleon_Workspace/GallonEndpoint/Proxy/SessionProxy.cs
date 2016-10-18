using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GalleonEndpoint.Proxy {
    
    [DataContract]
    public class SessionProxy {

        [DataMember]
        public string Token { get; set; }
    }
}