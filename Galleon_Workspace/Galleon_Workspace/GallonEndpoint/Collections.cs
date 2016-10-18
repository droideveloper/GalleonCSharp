using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace GalleonEndpoint {
    
    public static class Collections {

        //this part will help me to use such method on the fly
        private static JavaScriptSerializer serializer;
        private static JavaScriptSerializer JSONSerializer {
            get {
                if(serializer.IsNullOrEmpty()) {
                    serializer = new JavaScriptSerializer();
                    serializer.RegisterConverters(new JavaScriptConverter[] { new NullPropertySerializer() });
                }
                return serializer;
            }
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> collection) {
            return collection == null || collection.Count <= 0;
        }

        public static bool IsNullOrEmpty<T>(this T t) {
            return t == null || (t is string || t is String) ? string.IsNullOrEmpty(t as string) : false;
        }

        public static string Seriaize<T>(this Response<T> content) {
            if(content.IsNullOrEmpty()) { return string.Empty; }
            return JSONSerializer.Serialize(content);
        }
    }
}