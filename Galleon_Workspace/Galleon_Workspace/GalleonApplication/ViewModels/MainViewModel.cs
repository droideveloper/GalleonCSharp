using GalleonApplication.ContentViews;
using GalleonApplication.Extra;
using GalleonApplication.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalleonApplication.Events;
using GalleonApplication.Managers;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Windows.Threading;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace GalleonApplication.ViewModels {
    
    public class MainViewModel : IPropertyChanged {

        private string message;
        private int position;
        private bool isDisplaySearchProgress;
        private bool isDisplaySearchBar;
        private string queryText;
        private Navigation[] navigationContents;

        public int Position {
            get {
                return position;
            }
            set {
                setProperty(ref position, value);
            }
        }

        public Navigation[] NavigationContents {
            get {
                return navigationContents;
            }
            set { 
                setProperty(ref navigationContents, value);
            }
        }

        public bool IsDisplaySearchBar {
            get {
                return isDisplaySearchBar;
            }
            set {
                setProperty(ref isDisplaySearchBar, value);
            }
        }

        public bool ShowSearchProgress {
            get {
                return isDisplaySearchProgress;
            }
            set {
                setProperty(ref isDisplaySearchProgress, value);
            }
        }

        public string QueryText {
            get {
                return queryText;
            }

            set {
                setProperty(ref queryText, value);
                ShowSearchProgress = true;
                if(!querySendThrotte.IsNullOrEmpty()) {
                    querySendThrotte.Dispose();
                    querySendThrotte = null;
                }
                querySendThrotte = Observable.Return(value.IsNullOrEmpty())
                                             .SelectMany(x => {
                                                 if(x) {
                                                     ShowSearchProgress = false;
                                                     return Observable.Empty<string>();
                                                 } else {
                                                     return Observable.Return(value);
                                                 }
                                             }).Throttle(TimeSpan.FromMilliseconds(500))
                                               .SelectMany(x => {
                                                   BusManager.Send(new SearchBarEvent() {
                                                       isQueryChange = true,
                                                       queryText = value
                                                   });
                                                   return Observable.Return(true);
                                             }).SubscribeOn(ThreadPoolScheduler.Instance)
                                               .ObserveOnDispatcher(DispatcherPriority.Background)
                                               .Subscribe(x => { }, error => { error.PersistException(); });                
            }
        }

        public string Message {
            get {
                return message;
            }
            set {
                setProperty(ref message, value);
            }
        }

        public ICommand DismissSnackbarCommand {
            get {
                return new RelayCommand(() => {
                    BusManager.Send(new SnackbarEvent() {
                        isCloseEvent = true
                    });
                });
            }
        }

        public MainViewModel() {
            NavigationContents = new Navigation[] {
                new Navigation() {
                    Title = "Find Customers",
                    ContentView = new FindCustomerContentView()
                },
                new Navigation() {
                    Title = "Create New Customer",
                    ContentView = new CreateCustomerContentView()
                },
                new Navigation() {
                    Title = "Tools",
                    ContentView = new ToolsContentView()
                },
                new Navigation() {
                    Title = "Settings",
                    ContentView = new SettingContentView()
                }
            };
            //initial selection
            Position = 0;
        }

        private IDisposable subscription;
        private IDisposable querySendThrotte;

        //Lifecycle - start
        public void OnStart() {
            subscription = BusManager.Add((args) => {
                if(args is SearchBarEvent) {
                    SearchBarEvent evn = args as SearchBarEvent;
                    if(evn.isShow) {
                        IsDisplaySearchBar = !IsDisplaySearchBar;
                    } else if(evn.isProgress) {
                        ShowSearchProgress = !ShowSearchProgress;
                    }
                }
            });
        }

        //Lifecycle - stop
        public void OnStop() {
            BusManager.Remove(subscription);
        }
    }
}
