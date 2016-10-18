using System;
using System.Runtime.Serialization;
namespace GalleonEndpoint {
	/// <summary>
	/// Response
	/// </summary>
	[DataContract]
	public class Response<T> {
		
		[DataMember]
		public int Code { get; set; }
		[DataMember]
		public string Message { get; set; }
		[DataMember]
		public T Data { get; set; }
	}
}

