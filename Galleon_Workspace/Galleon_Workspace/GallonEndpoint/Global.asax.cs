using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Linq;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using GalleonEndpoint.Proxy;
using System.Runtime.Serialization;

namespace GalleonEndpoint {
	public class Global : HttpApplication {
		protected void Application_Start() {
			RegisterRouteTables(RouteTable.Routes);            
		}

		private static void RegisterRouteTables(RouteCollection routes) { 
			//routes.Ignore("{resource}.axd/{*pathInfo}");
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

            routes.Add(new ServiceRoute("v1/endpoint", new WebServiceHostFactory(), typeof(Endpoint)));
		}
	}
}
