using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using GalleonEntity;
using System.Collections.Generic;
using GalleonEndpoint.Proxy;

namespace GalleonEndpoint {

	[ServiceContract]
	public interface IEndpoint {
//SIGN IN -- START
		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/sign-in")]
        Response<SessionProxy> signin(Sign sign);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/keep-alive")]
		Response<SessionProxy> keepAlive(string token);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/sign-out")]
		Response<bool> signout(string token);
//SIGN IN -- END
//CUSTOMER -- BEGIN
		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/customer/{id}")]
		Response<CustomerProxy> queryCustomer(string id);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/customers")]
        Response<List<CustomerProxy>> queryAllCustomers();

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/customers-filter?start={start}&limit={limit}")]
        Response<List<CustomerProxy>> queryCustomersWithStartAndLimit(string start, string limit);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/customers-query?q={query}&by={property}")]
        Response<List<CustomerProxy>> queryCustomerWithQueryAndProperty(string query, string property);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.POST,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare,
                   UriTemplate = "/customers-query/{query}")]
        Response<List<CustomerProxy>> queryCustomers(string query);

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
        Response<CustomerProxy> createCustomer(CustomerProxy customer);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/update/customer")]
        Response<CustomerProxy> updateCustomer(CustomerProxy customer);
//CUSTOMER -- END

//CONTACT -- START
		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/contact/{id}")]
		Response<ContactProxy> queryContact(string id);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/contacts/{id}")]
        Response<List<ContactProxy>> queryContactsByCustomerID(string id);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/contacts/{id}?s={start}&l={limit}")]
        Response<List<ContactProxy>> queryContactsByCustomerIDAndStartAndLimit(string id, string start, string limit);

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
        Response<ContactProxy> createContact(ContactProxy contact);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.POST,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare,
                   UriTemplate = "/create/contacts")]
        Response<List<ContactProxy>> createContacts(List<ContactProxy> contacts);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/update/contact")]
        Response<ContactProxy> updateContact(ContactProxy contact);
//CONTACT -- END

//DIRECTORY -- BEGIN
		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/directory/{id}")]
		Response<DirectoryProxy> queryDirectory(string id);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/directories/{id}")]
        Response<List<DirectoryProxy>> queryDirectoriesByCustomerID(string id);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/directories-filter/{id}?start={start}&limit={limit}")]
        Response<List<DirectoryProxy>> queryDirectoriesByCustomerIDAndStartAndLimit(string id, string start, string limit);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/create/directory")]
        Response<DirectoryProxy> createDirectory(DirectoryProxy directory);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/update/directory")]
        Response<DirectoryProxy> updateDirectory(DirectoryProxy directory);

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
		Response<DocumentProxy> queryDocument(string id);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/documents/{id}")]
        Response<List<DocumentProxy>> queryDocumentsByDirectoryID(string id);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/documents-filter/{id}?start={start}&limit={limit}")]
        Response<List<DocumentProxy>> queryDocumentsByDirectoryAndStartAndLimit(string id, string start, string limit);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/create/document")]
        Response<DocumentProxy> createDocument(DocumentProxy newDocument);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.POST,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare,
                   UriTemplate = "/create/documents")]
        Response<List<DocumentProxy>> createDocuments(List<DocumentProxy> documents);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.POST,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   BodyStyle = WebMessageBodyStyle.Bare,
				   UriTemplate = "/update/document")]
        Response<DocumentProxy> updateDocument(DocumentProxy updateDocument);

		[OperationContract]
		[WebInvoke(Method = HttpMethod.GET,
				   RequestFormat = WebMessageFormat.Json,
				   ResponseFormat = WebMessageFormat.Json,
				   UriTemplate = "/delete/document/{id}")]
		Response<bool> deleteDocument(string id);
//DOCUMENT -- END

        [OperationContract]
        [WebInvoke(Method = HttpMethod.GET,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare,
                   UriTemplate = "/countries")]
        Response<List<CountryProxy>> queryCountries();

        [OperationContract]
        [WebInvoke(Method = HttpMethod.GET,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare,
                   UriTemplate = "/cities/{id}")]
        Response<List<CityProxy>> queryCities(string id);

        [OperationContract]
        [WebInvoke(Method = HttpMethod.GET,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare,
                   UriTemplate = "/categories")]
        Response<List<CategoryProxy>> queryCategories();

        [OperationContract]
        [WebInvoke(Method = HttpMethod.GET,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare,
                   UriTemplate = "/version")]
        Response<int> version();
	}
}

