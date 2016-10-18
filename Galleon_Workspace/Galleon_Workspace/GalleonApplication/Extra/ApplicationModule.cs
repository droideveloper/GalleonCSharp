using GalleonApplication.Extra.Net;
using GalleonApplication.Managers;
using GalleonApplication.ViewModels;
using Ninject.Modules;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra {
    
    public class ApplicationModule : NinjectModule {
        //TODO Change this
        private const string HOST_URI = "http://localhost:49051/v1/endpoint";
        private const string FILE_URI = "http://localhost:49051/v1/files";

        public override void Load() {
            Bind<IEndpointClient>().ToConstant(ProvideEndpointClient).InSingletonScope();
            Bind<IFileClient>().ToConstant(ProvideFileClient).InSingletonScope();
            Bind<IPreferenceManager>().To<PreferenceManager>().InSingletonScope();
            Bind<WindowViewModel>().ToSelf().InTransientScope();
            Bind<MainViewModel>().ToSelf().InTransientScope();
            Bind<SigninViewModel>().ToSelf().InTransientScope();
            Bind<CreateCustomerViewModel>().ToSelf().InTransientScope();
            Bind<AddNewContactViewModel>().ToSelf().InTransientScope();
            Bind<SettingsViewModel>().ToSelf().InTransientScope();
            Bind<FindCustomerViewModel>().ToSelf().InTransientScope();
            Bind<ToolsViewModel>().ToSelf().InTransientScope();
            Bind<AddFilesViewModel>().ToSelf().InTransientScope();
            Bind<DbManager>().ToSelf().InSingletonScope();
            Bind<IFileManager>().To<FileManager>().InSingletonScope();
        }

        private IEndpointClient ProvideEndpointClient { 
            get {
                return RestService.For<IEndpointClient>(new HttpClient(new AccessTokenHttpClient()) {
                    BaseAddress = new Uri(HOST_URI)
                }, new RefitSettings() {
                    JsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings() { 
                        NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                        DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat
                    }
                });
            }
        }

        private IFileClient ProvideFileClient {
            get {
                return RestService.For<IFileClient>(new HttpClient(new AccessTokenHttpClient()) {
                    BaseAddress = new Uri(FILE_URI)
                }, new RefitSettings() {
                    JsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings() {
                        NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                        DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat
                    }
                });   
            }            
        } 
    }
}
