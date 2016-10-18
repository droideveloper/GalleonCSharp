using Refit;
using System.Net.Http;
using System;
using System.Text;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GalleonApplication.Extra;
using System.Collections.Generic;

namespace GalleonApplication.Extra.Net {
    
    public interface IEndpointClient {

        [Get("/version")]
        IObservable<HttpResponseMessage> version();

        [Post("/sign-in")]
        IObservable<HttpResponseMessage> tryLogin([Body] LoginEntity creditentials);

        [Get("/directory/{id}")]
        IObservable<HttpResponseMessage> queryDirectory(int id);

        [Get("/document/{id}")]
        IObservable<HttpResponseMessage> queryDocument(int id);

        [Get("/countries")]
        IObservable<HttpResponseMessage> queryCountries();

        [Get("/cities/{id}")]
        IObservable<HttpResponseMessage> queryCities(int id);

        [Get("/categories")]
        IObservable<HttpResponseMessage> queryCategories();

        [Post("/customers-query/{query}")]
        IObservable<HttpResponseMessage> queryCustomers(string query);

        [Get("/documents/{id}")]
        IObservable<HttpResponseMessage> queryDocumentsByDirectoryID(int id);

        [Post("/create/customer")]
        IObservable<HttpResponseMessage> createCustomer([Body] CustomerEntity customer);

        [Post("/create/contact")]
        IObservable<HttpResponseMessage> createContact([Body] ContactEntity contact);

        [Post("/create/contacts")]
        IObservable<HttpResponseMessage> createContacts([Body] List<ContactEntity> contacts);

        [Post("/create/directory")]
        IObservable<HttpResponseMessage> createDirectory([Body] DirectoryEntity directory);

        [Post("/create/documents")]
        IObservable<HttpResponseMessage> createDocuments([Body] List<DocumentEntity> documents);

        [Post("/update/document")]
        IObservable<HttpResponseMessage> updateDocument([Body] DocumentEntity document);

        [Post("/keep-alive")]
        [Headers("Content-Type: application/json; charset=utf-8")]
        IObservable<HttpResponseMessage> tryKeepAlive([Body(BodySerializationMethod.Json)] string token);
    }
}
