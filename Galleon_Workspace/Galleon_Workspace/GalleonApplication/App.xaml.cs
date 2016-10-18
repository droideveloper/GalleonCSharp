using GalleonApplication.App_Data;
using GalleonApplication.Events;
using GalleonApplication.Extra;
using GalleonApplication.Extra.Net;
using GalleonApplication.Managers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GalleonApplication {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private const string AUTH_TOKEN_KEY = "X-Auth-Token";

        private IDisposable subscription;
        private IDisposable bounce;

        protected override void OnStartup(StartupEventArgs e) {
            //first thing to do
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            //start dependency injection
            DependencyInjector.Init(new ApplicationModule());
            //register for login event
            subscription = BusManager.Add((evnt) => {
                if(evnt is LoginEvent) {
                    LoginEvent loginEvent = evnt as LoginEvent;
                    if(loginEvent.isSuccess) {
                        bounce = OnUserLogin;
                    }
                }
            });
            Exit += OnDestroy;
            //start file manager
            IFileManager fileManager = DependencyInjector.Get<IFileManager>();
            if(!fileManager.IsNullOrEmpty()) {
                fileManager.start();
            }
            base.OnStartup(e);
        }

        protected ExitEventHandler OnDestroy {
            get {
                return (sender, args) => {
                    IPreferenceManager preferenceManager = DependencyInjector.Get<IPreferenceManager>();
                    if(!preferenceManager.IsNullOrEmpty()) {
                        preferenceManager.write();
                    }
                    IFileManager fileManager = DependencyInjector.Get<IFileManager>();
                    if(!fileManager.IsNullOrEmpty()) {
                        fileManager.stop();
                    }

                    BusManager.Remove(subscription);
                    if(!bounce.IsNullOrEmpty()) {
                        bounce.Dispose();
                        bounce = null;
                    }
                };
            }
        }

        //Make this method log into xml or json so we can read it later
        protected UnhandledExceptionEventHandler OnUnhandledException {
            get { 
                return (sender, args) => {
                    Exception error =  args.ExceptionObject as Exception;
                    if(error != null) {
                        error.PersistException();
                    }
                };
            }
        }

        protected IDisposable OnUserLogin {
            get {
                return Observable.Interval(TimeSpan.FromMinutes(25))//25min try to keep user session active
                    .SelectMany(s => {
                        IEndpointClient endpointClient = DependencyInjector.Get<IEndpointClient>();
                        string token = App.Current.Properties.Contains(AUTH_TOKEN_KEY)
                            ? App.Current.Properties[AUTH_TOKEN_KEY] as string
                            : string.Empty;

                        return token.IsNullOrEmpty()
                            ? Observable.Return(new Response<SessionEntity>() {
                                Code = 404,
                                Message = "Access token is null.",
                                Data = null
                            }) : endpointClient.tryKeepAlive("\""+ token + "\"")//hand made json, str
                                                .ToResponse<SessionEntity>();
                    })
                    .SubscribeOn(ThreadPoolScheduler.Instance)
                    .ObserveOnDispatcher(DispatcherPriority.Background)
                    .Subscribe(response => {
                        if(response.Code == ResponseCode.OK) {
                            SessionEntity entity = response.Data;
                            if(!entity.IsNullOrEmpty()) {
                                if(App.Current.Properties.Contains(AUTH_TOKEN_KEY)) {
                                    App.Current.Properties.Remove(AUTH_TOKEN_KEY);//if we already stored one we have to delete it.
                                }
                                App.Current.Properties.Add(AUTH_TOKEN_KEY, entity.Token);
                            } else {
                                //store as error
                                DbManager dbManager = DependencyInjector.Get<DbManager>();
                                LogException parentException = new LogException() {
                                    Message = response.Message,
                                    StackTrace = string.Format("Code: {0}, Message: {1}", response.Code, response.Message),
                                    CreateDate = DateTime.Now
                                };
                                dbManager.LogExceptions.Add(parentException);
                                dbManager.SaveChanges();
                            }
                        }
                    }, error => {
                        error.PersistException();
                    }); 
            }
        }
    }
}
