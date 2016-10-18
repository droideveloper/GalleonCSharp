using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using GallonEntity;
using System.Collections.Generic;

namespace GallonEndpoint {

	[ServiceContract]
	public interface IEndpoint {
//SIGN IN -- START
		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/signin")]
		Response<Session> signin(Sign sign);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/keepalive")]
		Response<Session> keepAlive(string token);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/signout")]
		Response<bool> signout(string token);
//SIGN IN -- END
//CUSTOMER -- BEGIN
		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/customer/{id}")]
		Response<Customer> queryCustomer(string id);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/customers")]
		Response<List<Customer>> queryAllCustomers();

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/customers?start={start}&limit={limit}")]
		Response<List<Customer>> queryCustomersWithStartAndLimit(string start, string limit);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/customers?q={query}&by={property}")]
		Response<List<Customer>> queryCustomerWithQueryAndProperty(string query, string property);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/delete/customer/{id}")]
		Response<bool> deleteCustomer(string id);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/create/customer")]
		Response<Customer> createCustomer(Customer customer);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/update/customer")]
		Response<Customer> updateCustomer(Customer customer);
//CUSTOMER -- END

//CONTACT -- START
		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/contact/{id}")]
		Response<Contact> queryContact(string id);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/contacts/{id}")]
		Response<List<Contact>> queryContactsByCustomerID(string id);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/contacts/{id}?start={start}&limit={limit}")]
		Response<List<Contact>> queryContactsByCustomerIDAndStartAndLimit(string id, string start, string limit);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/delete/contact/{id}")]
		Response<bool> deleteContact(string id);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/create/contact")]
		Response<Contact> createContact(Contact contact);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/update/contact")]
		Response<Contact> updateContact(Contact contact);
//CONTACT -- END

//DIRECTORY -- BEGIN
		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/directory/{id}")]
		Response<Directory> queryDirectory(string id);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/directories/{id}")]
		Response<List<Directory>> queryDirectoriesByCustomerID(string id);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/directories/{id}?start={start}&limit={limit}")]
		Response<List<Directory>> queryDirectoriesByCustomerIDAndStartAndLimit(string id, string start, string limit);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/create/directory")]
		Response<Directory> createDirectory(Directory directory);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/update/directory")]
		Response<Directory> updateDirectory(Directory directory);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/delete/directory/{id}")]
		Response<bool> deleteDirectory(string id);
//DIRECTORY -- END

//DOCUMENT -- START
		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/document/{id}")]
		Response<Document> queryDocument(string id);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/documents/{id}")]
		Response<List<Document>> queryDocumentsByDirectoryID(string id);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/documents/{id}?start={start}&limit={limit}")]
		Response<List<Document>> queryDocumentsByDirectoryAndStartAndLimit(string id, string start, string limit);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/create/doucment")]
		Response<Document> createDocument(Document document);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/update/document")]
		Response<Document> updateDocument(Document document);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   UriTemplate = "/delete/document/{id}")]
		Response<bool> deleteDocument(string id);
//DOCUMENT -- END
	}
}

