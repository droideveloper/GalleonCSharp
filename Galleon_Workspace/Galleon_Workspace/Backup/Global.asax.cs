using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace GallonEndpoint {
	public class Global : HttpApplication {
		protected void Application_Start() {
			//GlobalConfiguration.Configure(WebApiConfig.Register);
			RegisterRouteTables(RouteTable.Routes);
		}

		private static void RegisterRouteTables(RouteCollection routes) { 
			routes.Ignore("{resource}.axd/{*pathInfo}");
			//routes.Add(new ServiceRoute("/endpoint/v1", new WebServiceHostFactory(), typeof(IEndpoint)));
			//download
			routes.MapPageRoute("cg", 
			                    "v1/files/download-document", 
			                    "~/document.aspx");
			//create
			routes.MapPageRoute("cc",
			                   "v1/files/create-document",
			                   "~/document.aspx");
			//update
			routes.MapPageRoute("cu",
							   "v1/files/update-document",
								"~/document.aspx");
			//delete
			routes.MapPageRoute("cd",
							   "v1/files/delete-document",
								"~/document.aspx");
		}
	}
}
