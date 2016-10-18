using System;
using System.Runtime.Serialization;

namespace GallonEndpoint {
	
	[DataContract]
	public class Sign {
		
		[DataMember(Name = "username")]
		public string UserName { get; set; }
		[DataMember(Name = "password")]
		public string Password { get; set; }
	}
}

