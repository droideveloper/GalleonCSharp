using System;
using System.Runtime.Serialization;

namespace GalleonEndpoint {
	
	[DataContract]
	public class Sign {
		
		[DataMember]
		public string UserName { get; set; }
		[DataMember]
		public string Password { get; set; }
	}
}

