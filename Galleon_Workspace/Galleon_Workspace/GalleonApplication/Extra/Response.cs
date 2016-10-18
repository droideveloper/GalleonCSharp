using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra {

    public class Response<T> {    

        public int      Code    { get; set; }
        public string   Message { get; set; }
        public T        Data    { get; set; }

        public static Response<T> Deserialize(string json) {
            return JsonConvert.DeserializeObject<Response<T>>(json);
        }
    }
}
