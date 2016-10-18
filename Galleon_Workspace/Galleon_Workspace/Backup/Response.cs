using System;
using System.Runtime.Serialization;
namespace GallonEndpoint {
	/// <summary>
	/// Response
	/// </summary>
	[DataContract]
	public class Response<T> {
		
		[DataMember(Name = Constants.NAME_CODE)]
		public int Code { get; set; }

		[DataMember(Name = Constants.NAME_MESSAGE)]
		public string Message { get; set; }

		[DataMember(Name = Constants.NAME_DATA)]
		public T Data { get; set; }
	}
}

