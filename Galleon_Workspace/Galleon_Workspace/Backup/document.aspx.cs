using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using GallonEntity;
using System.Dynamic;
using System.IO;

namespace GallonEndpoint {

	public partial class document : Page {

		private const string KEY_CONTENT_NAME = "X-File-Name";
		private const string KEY_CONTENT_TYPE = "X-File-Type";
		private const string KEY_LAST_MODIFIED = "X-Update-Date";
		private const string KEY_CREATE_DATE = "X-Create-Date";

		private const string QUERY_KEY 	= "key";
		private const string QUERY_TYPE = "type";

		private const string MIME_JSON 	= "application/json; charset=utf-8";
		private const string MIME_OCTET = "application/octet-stream";

		private const string PATH_DOWNLOAD = "download-document";
		private const string PATH_CREATE = "create-document";
		private const string PATH_UPDATE = "update-document";
		private const string PATH_DELETE = "delete-document";

		private JavaScriptSerializer 	serializer 		= new JavaScriptSerializer();
		private GallonEntityContext 	entityContext   = new GallonEntityContext();

		/// <summary>
		/// Entry pointA
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnLoad(EventArgs e) {
			serializer.RegisterConverters(new JavaScriptConverter[] { new NullPropertySerializer() });
			base.OnLoad(e);
		}

		/// <summary>
		/// Entry pointB
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		protected void Page_Load(object sender, EventArgs args) { 
			HttpRequest request = Request;
			HttpResponse response = Response;


			string token = GetToken(request);
			if(string.IsNullOrEmpty(token)) {
				response.StatusCode = 200;
				response.ContentType = MIME_JSON;
				response.Write(serializer.Serialize(new Response<bool>() {
					Code = 401,
					Message = "unauthorized",
					Data = false
				}));
				return;
			}

			Session session = entityContext.Sessions
										   .Where(se => se.Token.Equals(token))
										   .FirstOrDefault();
			if(session == null || session.DueDate < DateTime.Now) {
				if(session != null) {
					entityContext.Sessions.Remove(session);
					entityContext.SaveChanges();
				}
				response.StatusCode = 200;
				response.ContentType = MIME_JSON;
				response.Write(serializer.Serialize(new Response<bool>() {
					Code = 401,
					Message = "session invalid.",
					Data = false
				}));
				return;
			}

			switch(request.HttpMethod) {
				case HttpMethod.GET: {
						provideDownloadContent(request, response);
						break;
					}
				case HttpMethod.POST: {
						providePost(request, response);
						break;
					}
				default: {
						response.StatusCode = 200;
						response.ContentType = MIME_JSON;
						response.Write(new Response<bool>() {
							Code = 404,
							Message = string.Format("{0} is not supported", request.HttpMethod),
							Data = false
						});
							
					}
					break;
			}
		}

		/// <summary>
		/// Provides the content of the download.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <param name="response">Response.</param>
		protected void provideDownloadContent(HttpRequest request, HttpResponse response) { 
			string key = Invoke<string>(() => {
				return HttpUtility.ParseQueryString(new Uri(request.Url.Scheme
															+ "://" + request.Url.Authority
															+ request.RawUrl).Query).Get(QUERY_KEY);
			});

			string path = Invoke<string>(() => {
				return request.RawUrl.Split('/')
							  .ToList().LastOrDefault();
			});

			if(!PATH_DOWNLOAD.Equals(path)) {
				response.StatusCode = 200;
				response.ContentType = MIME_JSON;
				response.Write(new Response<bool>() { 
					Code = 404,
					Message = "no such content",
					Data = false
				});
				return;
			}

			int documentID;
			int.TryParse(key, out documentID);

			Content content = entityContext.Contents
										   .Where(c => c.DocumentID == documentID)
										   .FirstOrDefault();
			if(content == null) {
				response.StatusCode = 200;
				response.ContentType = MIME_JSON;
				response.Write(new Response<bool>() {
					Code = 404,
					Message = "no such content.",
					Data = false
				});
				return;
			}

			Document doc = entityContext.Documents
											 .Where(d => d.DocumentID == documentID)
											 .FirstOrDefault();

			response.ContentType = MIME_OCTET;
			if(doc != null) {
				response.AddHeader(KEY_CONTENT_NAME, doc.DocumentName);
				response.AddHeader(KEY_CONTENT_TYPE, doc.ContenType);
				response.AddHeader(KEY_LAST_MODIFIED, doc.UpdateDate.ToString());
				response.AddHeader(KEY_CREATE_DATE, doc.CreateDate.ToString());
			}
			response.StatusCode = 200;
			response.OutputStream.Write(content.ContentSink, 0, content.ContentSink.Length);
		}

		/// <summary>
		/// Provides the content of the update.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <param name="response">Response.</param>
		/// <param name="docx">Docx.</param>
		/// <param name="content">Content.</param>
		protected void provideUpdateContent(HttpRequest request, HttpResponse response, Document docx, Content content) {
			if(content == null) {
				response.StatusCode = 200;
				response.ContentType = MIME_JSON;
				response.Write(new Response<bool>() { 
					Code = 404,
					Message = "content can be found.",
					Data = false
				});
				return;
			}

			MemoryStream stream = new MemoryStream();
			request.InputStream.CopyTo(stream);
			//update blob
			content.ContentSink = stream.ToArray();
			content.UpdateDate = DateTime.Now;
			//update info 
			docx.UpdateDate = DateTime.Now;
			//save change
			entityContext.SaveChanges();
			//send user json response
			response.StatusCode = 200;
			response.ContentType = MIME_JSON;
			response.Write(new Response<bool>() {
				Code = 200,
				Message = "success.",
				Data = true
			});
		}

		/// <summary>
		/// Provides the content of the delete.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <param name="response">Response.</param>
		/// <param name="docx">Docx.</param>
		/// <param name="content">Content.</param>
		protected void provideDeleteContent(HttpRequest request, HttpResponse response, Document docx, Content content) {
			if(content == null) {
				response.StatusCode = 200;
				response.ContentType = MIME_JSON;
				response.Write(new Response<bool>() {
					Code = 404,
					Message = "no such content exists",
					Data = false
				});
				return;
			}

			//this directly delete it what if I want to recover
			entityContext.Contents.Remove(content);
			entityContext.Documents.Remove(docx);
			entityContext.SaveChanges();

			response.StatusCode = 200;
			response.ContentType = MIME_JSON;
			response.Write(new Response<bool>() {
				Code = 200,
				Message = "success.",
				Data = false
			});
		}

		/// <summary>
		/// Provides the content of the create.
		/// </summary>
		/// <param name="requst">Requst.</param>
		/// <param name="response">Response.</param>
		/// <param name="docx">Docx.</param>
		protected void provideCreateContent(HttpRequest requst, HttpResponse response, Document docx) {
			if(docx == null) {
				response.StatusCode = 200;
				response.ContentType = MIME_JSON;
				response.Write(new Response<bool>() { 
					Code = 200,
					Message = "no such document, sorry.",
					Data = false
				});
				return;
			}

			MemoryStream stream = new MemoryStream();
			requst.InputStream.CopyTo(stream);

			entityContext.Contents.Add(new Content() {
				ContentSink = stream.ToArray(),
				DocumentID = docx.DocumentID,
				CreateDate = DateTime.Now,
				UpdateDate = DateTime.Now
			});

			docx.UpdateDate = DateTime.Now;
			entityContext.SaveChanges();

			response.StatusCode = 200;
			response.ContentType = MIME_JSON;
			response.Write(new Response<bool>() { 
				Code = 200,
				Message = "succes.",
				Data = false
			});
		}

		/// <summary>
		/// Provides the post.
		/// </summary>
		/// <param name="request">Request.</param>
		/// <param name="response">Response.</param>
		protected void providePost(HttpRequest request, HttpResponse response) {
			string key = Invoke<string>(() => {
				return HttpUtility.ParseQueryString(new Uri(request.Url.Scheme
															+ "://" + request.Url.Authority
															+ request.RawUrl).Query).Get(QUERY_KEY);
			});
			string type = Invoke<string>(() => {
				return request.Path
							  .Split('/').ToList()
							  .LastOrDefault();
			});

			int documentID;
			int.TryParse(key, out documentID);

			Document docx = entityContext.Documents
											 .Where(d => d.DocumentID == documentID)
											 .FirstOrDefault();

			Content content = entityContext.Contents
										   .Where(c => c.DocumentID == documentID)
										   .FirstOrDefault();

			if(PATH_UPDATE.Equals(type)) {
				provideUpdateContent(request, response, docx, content);
			} else if(PATH_DELETE.Equals(type)) {
				provideDeleteContent(request, response, docx, content);
			} else if(PATH_CREATE.Equals(type)) {
				provideCreateContent(request, response, docx);
			} else {
				response.StatusCode = 200;
				response.ContentType = MIME_JSON;
				response.Write(new Response<bool>() { 
					Code = 200,
					Message = "no content",
					Data = false
				});
			}
		}

		/// <summary>
		/// Invoke the specified lamda expression and returs T type result
		/// </summary>
		/// <param name="func">Func.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		protected T Invoke<T>(Func<T> func) {
			return func();
		}

		/// <summary>
		/// Gets the token, from header and check it if it contains token
		/// </summary>
		/// <returns>The token.</returns>
		/// <param name="request">Request.</param>
		protected string GetToken(HttpRequest request) {
			string[] keys = request.Headers.AllKeys;
			string[] values;
			for(int i = 0, z = keys.Length; i < z; i++) {
				string key = keys[i];
				values = request.Headers.GetValues(key);
				if(key.Equals(Endpoint.SESSION_TOKEN)) {
					return values.ToList()
								 .FirstOrDefault();
				}
			}
			return null;
		}
	}
}

