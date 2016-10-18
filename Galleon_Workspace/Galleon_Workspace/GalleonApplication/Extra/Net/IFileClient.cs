using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra.Net {

    public class FileClientHeader {
        public const string DOCUMENT_ID = "DocumentID";
    }

    public interface IFileClient {

        [Get("/download-document")]
        IObservable<HttpResponseMessage> downloadContent([Header(FileClientHeader.DOCUMENT_ID)] int id);

        [Post("/create-document")]
        IObservable<HttpResponseMessage> createContent([Header(FileClientHeader.DOCUMENT_ID)] int id, Stream file);

        [Post("/update-document")]
        IObservable<HttpResponseMessage> updateContent([Header(FileClientHeader.DOCUMENT_ID)] int id, Stream file);

        [Post("/delete-document")]
        IObservable<HttpResponseMessage> deleteContent([Header(FileClientHeader.DOCUMENT_ID)] int id);
    }
}
