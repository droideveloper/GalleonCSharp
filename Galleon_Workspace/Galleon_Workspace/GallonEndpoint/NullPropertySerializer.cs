﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Script.Serialization;

namespace GalleonEndpoint {
	public class NullPropertySerializer : JavaScriptConverter {
		
		public override IEnumerable<Type> SupportedTypes {
			get {
				return GetType().Assembly.GetTypes();
			}
		}

		public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer) {
			throw new NotImplementedException();
		}

		public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer) {
			Dictionary<string, object> json = new Dictionary<string, object>();
			foreach(var prop in obj.GetType().GetProperties()) {
				//check if decorated with ScriptIgnore attribute
				bool ignoreProp = prop.IsDefined(typeof(ScriptIgnoreAttribute), true);

				var value = prop.GetValue(obj, BindingFlags.Public, null, null, null);
				if(value != null && !ignoreProp) {
					json.Add(prop.Name, value);
				}
			}
			return json;
		}
	}
}

