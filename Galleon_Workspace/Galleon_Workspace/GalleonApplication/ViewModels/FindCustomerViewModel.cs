using GalaSoft.MvvmLight.CommandWpf;
using GalleonApplication.App_Data;
using GalleonApplication.ContentViews;
using GalleonApplication.Events;
using GalleonApplication.Extra;
using GalleonApplication.Extra.Net;
using GalleonApplication.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace GalleonApplication.ViewModels {
    
    public class FindCustomerViewModel : IPropertyChanged {

        private CustomerEntities customers;
        private int position;
        private DocumentEntities customerFiles;

        public int Position {
            get {
                return position;
            }
            set {
                setProperty(ref position, value);
                if(position >= 0) {
                    CustomerEntity customer = customers.ElementAt(position);
                    if(customer != null) {
                        endpointClient.queryDocumentsByDirectoryID(customer.CustomerID)
                                      .ToResponse<List<DocumentEntity>>()
                                      .SelectMany(x => {
                                          if(x.Code == 200) {
                                              List<DocumentEntity> files = x.Data;
                                              if(!files.IsNullOrEmpty()) {
                                                  //Dispatcher of MainThread
                                                  App.Current.Dispatcher.Invoke(() => {
                                                      if(CustomerFiles.IsNullOrEmpty()) {
                                                          CustomerFiles = new DocumentEntities();
                                                      } else {
                                                          CustomerFiles.Clear();
                                                      }
                                                  });
                                                  return Observable.Return(files);
                                              }
                                              return Observable.Empty<List<DocumentEntity>>();
                                          } else {
                                              x.PersistResponseError();
                                              return Observable.Empty<List<DocumentEntity>>();
                                          }
                                      })
                                      .SelectMany(x => x)
                                      .SubscribeOn(ThreadPoolScheduler.Instance)
                                      .ObserveOnDispatcher(DispatcherPriority.Background)
                                      .Subscribe(x => {
                                          CustomerFiles.Add(x);
                                      }, error => {
                                          error.PersistException();
                                      }, () => {
                                          Debug.Write("Completed");
                                      });
                    }
                }
            }
        }

        public CustomerEntities Customers {
            get {
                return customers;
            }
            set {
                setProperty(ref customers, value);
            }
        }

        public DocumentEntities CustomerFiles {
            get {
                return customerFiles;
            }
            set {
                setProperty(ref customerFiles, value);
            }
        }
   
        public ICommand AddFileCommand {
            get {
                return new RelayCommand<CustomerEntity>((customer) => {
                    BusManager.Send(new DialogEvent() {
                        isCloseEvent = false,
                        view = new AddFilesContentView(),
                        closeHandler = (sender, args) => { 
                            if(!args.Parameter.IsNullOrEmpty()) {
                                CustomerFileEntities files = args.Parameter as CustomerFileEntities;
                                if(!files.IsNullOrEmpty()) { 
                                    //cancel content here
                                    if(uploadFileCancel != null) {
                                        uploadFileCancel.Dispose();
                                        uploadFileCancel = null;
                                    }
                                    uploadFileCancel = new DialogEvent() {
                                        isCloseEvent = false,
                                        view = new ProgressContentView(),
                                        closeHandler = (s, a) => {
                                            //cancel request
                                            if(uploadFileCancel != null) {
                                                uploadFileCancel.Dispose();
                                                uploadFileCancel = null;
                                            }
                                        }
                                    }
                                    .ThoretteEvent(TimeSpan.FromMilliseconds(300))                                          
                                    .SelectMany(x => {
                                              BusManager.Send(x);
                                              return endpointClient.queryDirectory(customer.CustomerID);
                                    })
                                    .ToResponse<DirectoryEntity>()
                                    .SelectMany(x => {
                                        if(x.Code == ResponseCode.OK) {
                                            if(x.Data.IsNullOrEmpty()) {
                                                return endpointClient.createDirectory(new DirectoryEntity() {
                                                    DirectoryID = customer.CustomerID,
                                                    DirectoryName = customer.CustomerID.ToString()
                                                }).ToResponse<DirectoryEntity>()
                                                .Select(r => r.Data);
                                            } else {
                                                return Observable.Return<DirectoryEntity>(x.Data);
                                            }
                                        } else {
                                            return endpointClient.createDirectory(new DirectoryEntity() {
                                                DirectoryID = customer.CustomerID,
                                                DirectoryName = customer.CustomerID.ToString()
                                            }).ToResponse<DirectoryEntity>()
                                            .Select(r => r.Data);
                                        }
                                    })
                                    .SelectMany(x => {
                                        return Observable.Return(files.Select(f => {
                                            return new DocumentEntity() {
                                                DirectoryID = x.DirectoryID,
                                                DocumentName = f.Name,
                                                CustomerID = customer.CustomerID,//added
                                                ContentType = f.Name.Split('.').LastOrDefault(),
                                                ContentLength = f.Length,
                                                CreateDate = f.CreationTime,
                                                UpdateDate = f.LastWriteTime
                                            };
                                        }).ToList());
                                    })
                                    .SelectMany(x => {
                                        return endpointClient.createDocuments(x)
                                                            .ToResponse<List<DocumentEntity>>();
                                    })
                                    .SelectMany(x => {
                                        if(x.Code == ResponseCode.OK) {
                                            return Observable.Return(x.Data);
                                        } else {
                                            x.PersistResponseError();
                                            return Observable.Empty<List<DocumentEntity>>();
                                        }
                                    })                                
                                    .SelectMany(x => {
                                        return Observable.Return(x.Select(y => new {
                                            Entity = y,
                                            Content = files.FirstOrDefault(z => z.Length == y.ContentLength
                                                                             && z.Name.Equals(y.DocumentName))
                                        }).ToList());                                
                                    })                                
                                    .SelectMany(x => x)                                
                                    .SelectMany(x => {
                                        FileInfo compressed = x.Content.ToCompress();
                                        if(compressed.IsNullOrEmpty()) {
                                            return Observable.Empty<Response<bool>>();
                                        } else {
                                            return fileClient.createContent(x.Entity.DocumentID, compressed.OpenRead())
                                                             .ToResponse<bool>();
                                        }
                                    })                                
                                    .SubscribeOn(ThreadPoolScheduler.Instance)                                
                                    .ObserveOnDispatcher(DispatcherPriority.Background)                                
                                    .Subscribe(x => {
                                        if(x.Code == ResponseCode.OK) {
                                            //we should show it better way
                                            Debug.Write(x.Message);
                                        } else {
                                            x.PersistResponseError();
                                        }
                                    }, error => {
                                        //there is error here
                                        error.PersistException();
                                    }, () => {
                                        //close dialog on completion and say we re ok
                                        uploadFileCancel = null;
                                        StartFileProcessing(customer, files);
                                    });
                                }
                            }
                        }
                    });
                });
            }
        }

        public ICommand RemoveFileCommand {
            get {
                return new RelayCommand<int>((index) => { 
                    if(index >= 0 && index < CustomerFiles.Count) {
                        DocumentEntity document = CustomerFiles.ElementAtOrDefault(index);
                        if(!document.IsNullOrEmpty()) { //if document is not null or empty
                            //start showing progress 
                            BusManager.Send(new DialogEvent() {
                                isCloseEvent = false,
                                view = new ProgressContentView()
                            });
                            Syncable local = dbManager.Syncables.FirstOrDefault(x => x.RemoteID == document.DocumentID);
                            if(!local.IsNullOrEmpty()) {
                                //check if such a file exists then delete it from local existance
                                if(File.Exists(local.LocalPath)) {
                                    File.Delete(local.LocalPath);
                                }
                                //clean up local track database
                                dbManager.Syncables.Remove(local);
                                dbManager.SaveChanges();
                            }
                            //rx style handle
                            fileClient.deleteContent(document.DocumentID)
                                      .ToResponse<bool>()
                                      .SubscribeOn(ThreadPoolScheduler.Instance)
                                      .ObserveOnDispatcher(DispatcherPriority.Background)
                                      .Subscribe(x => {
                                          if(x.Code == ResponseCode.OK) {
                                              CustomerFiles.RemoveAt(index);
                                              BusManager.Send(new SnackbarEvent() {
                                                  isCloseEvent = false,
                                                  textMessage = Properties.Resources.RemoveFileSuccess
                                              });
                                          } else {
                                              //persist error on local db
                                              x.PersistResponseError();
                                              BusManager.Send(new SnackbarEvent() {
                                                  isCloseEvent = false,
                                                  textMessage = Properties.Resources.RemoveFileErrorText
                                              });
                                          }
                                      }, error => {
                                          //persists error on local db
                                          error.PersistException();
                                          BusManager.Send(new SnackbarEvent() {
                                              isCloseEvent = false,
                                              textMessage = Properties.Resources.RemoveFileErrorText
                                          });
                                      }, () => {
                                          //we can close indicator
                                          BusManager.Send(new DialogEvent() {
                                              isCloseEvent = true
                                          });
                                      });
                        }
                    }
                });
            }
        }

        public ICommand DownloadFileCommand {
            get {
                return new RelayCommand<ParameterArray>((p) => {
                    string syncPath = preferenceManager.getValue(PreferenceManager.KEY_SYNC_FOLDER, string.Empty);
                    //Get first and second of the array
                    DocumentEntity remote = p.Parameters.ElementAt(0) as DocumentEntity;
                    CustomerEntity customer = p.Parameters.ElementAt(1) as CustomerEntity;
                    if(!syncPath.IsNullOrEmpty()) {
                        if(!remote.IsNullOrEmpty()) {
                            remote.DisplayProgress = true;
                            //if previous there get rid of it
                            if(downloadFileCancel != null) {
                                downloadFileCancel.Dispose();
                                downloadFileCancel = null;
                            }
                            downloadFileCancel = fileClient.downloadContent(remote.DocumentID)
                                                           .ToStream()
                                                           .ToFile(new Syncable() { LocalPath = Path.Combine(syncPath, 
                                                                                                             customer.CustomerID.ToString(), 
                                                                                                             remote.DocumentName) })
                                                           .SelectMany(x => {
                                                               if(x.Exists) {
                                                                   //store it in local db since we have this
                                                                   dbManager.Syncables.Add(new Syncable() {
                                                                       RemoteID = remote.DocumentID,
                                                                       FileName = remote.DocumentName,
                                                                       LastModified = remote.UpdateDate,
                                                                       LocalPath = Path.Combine(syncPath, 
                                                                                                customer.CustomerID.ToString(), 
                                                                                                remote.DocumentName)
                                                                   });
                                                                   dbManager.SaveChanges();
                                                                   return Observable.Return(x);
                                                               }
                                                               return Observable.Empty<FileInfo>();
                                                           }).SubscribeOn(ThreadPoolScheduler.Instance)
                                                             .ObserveOnDispatcher(DispatcherPriority.Background)
                                                             .Subscribe(x => {
                                                                 if(!x.IsNullOrEmpty()) {
                                                                     x.LastWriteTime = remote.UpdateDate;
                                                                 }
                                                                 remote.DisplayProgress = false;
                                                             }, error => {
                                                                 error.PersistException();
                                                             },
                                                             () => {
                                                                 BusManager.Send(new SelectedFileChangedEvent(remote));//changes selection of item null and back itself in order to trigger button visibility problem
                                                                 downloadFileCancel = null;//set it null in completion
                                                             });

                                                           

                        }
                    } else {
                        BusManager.Send(new SnackbarEvent() {
                            isCloseEvent = false,
                            textMessage = Properties.Resources.DownloadFileErrorText1
                        });
                    }
                });
            }
        }

        public ICommand ViewFileCommand {
            get {
                return new RelayCommand<DocumentEntity>((file) => {
                    if(!file.IsNullOrEmpty()) {
                        Syncable sync = dbManager.Syncables.FirstOrDefault(x => x.RemoteID == file.DocumentID);
                        if(sync.IsNullOrEmpty()) {
                            BusManager.Send(new SnackbarEvent() {
                                textMessage = Properties.Resources.ViewFileErrorText1,
                                isCloseEvent = false
                            });
                            return;
                        }
                        string fileLocation = sync.LocalPath;
                        if(!fileLocation.IsNullOrEmpty() && File.Exists(fileLocation)) {
                            Process.Start(fileLocation);
                        } else {
                            BusManager.Send(new SnackbarEvent() {
                                textMessage = Properties.Resources.ViewFileErrorText2,
                                isCloseEvent = false
                            });
                        }
                    }
                });
            }
        }

        private IEndpointClient endpointClient;
        private IPreferenceManager preferenceManager;
        private IFileClient fileClient;
        private DbManager dbManager;

        private IDisposable downloadFileCancel;
        private IDisposable uploadFileCancel;
        private IDisposable queryCancel;
        private IDisposable querySubscription;

        public FindCustomerViewModel(IEndpointClient endpointClient, DbManager dbManager, IPreferenceManager preferenceManager, IFileClient fileClient) {
            this.endpointClient = endpointClient;
            this.dbManager = dbManager;
            this.preferenceManager = preferenceManager;
            this.fileClient = fileClient;
        }

        public void OnStart() {
            BusManager.Send(new SearchBarEvent() {
                isShow = true
            });
            querySubscription = BusManager.Add((args) => {
                if(args is SearchBarEvent) {
                    SearchBarEvent evt = args as SearchBarEvent;
                    if(!evt.IsNullOrEmpty()) {
                        if(evt.isQueryChange) {
                            if(!queryCancel.IsNullOrEmpty()) {
                                queryCancel.Dispose();
                                queryCancel = null;
                            }
                            queryCancel = Observable.Return(
                                Regex.IsMatch(evt.queryText, @"([a-zA-Z0-9]{3,}[,\s]*)$", RegexOptions.IgnoreCase)
                            ).Throttle(TimeSpan.FromMilliseconds(500))
                             .SelectMany(x => {
                                 if(x) {
                                     return endpointClient.queryCustomers(evt.queryText);
                                 } else {
                                     App.Current.Dispatcher.Invoke(() => {
                                         if(!Customers.IsNullOrEmpty()) {
                                             Customers.Clear();
                                         }
                                         if(!CustomerFiles.IsNullOrEmpty()) {
                                             CustomerFiles.Clear();
                                         }
                                     });
                                 }
                                 return Observable.Empty<HttpResponseMessage>();
                             }).ToResponse<List<CustomerEntity>>()
                               .SubscribeOn(ThreadPoolScheduler.Instance)
                               .ObserveOn(App.Current.Dispatcher)
                               .Subscribe(x => {
                                    if(x.Code == ResponseCode.OK) {
                                        List<CustomerEntity> customers = x.Data;
                                        if(!customers.IsNullOrEmpty()) {
                                            if(Customers.IsNullOrEmpty()) {
                                                Customers = new CustomerEntities();
                                            } else {
                                                Customers.Clear();
                                                if(!CustomerFiles.IsNullOrEmpty()) {
                                                    CustomerFiles.Clear();
                                                }
                                            }
                                            customers.ForEach(y => Customers.Add(y));
                                        }
                                    } else {
                                        x.PersistResponseError();
                                    }
                                }, error => {
                                    error.PersistException();
                                    Debug.WriteLine("Error: " + error.StackTrace);
                                }, () => {
                                    BusManager.Send(new SearchBarEvent() {
                                        isProgress = true
                                    });
                                });                           
                        }
                    }
                
                }
            });        
        }

        public void OnStop() {
            BusManager.Remove(querySubscription);
            BusManager.Send(new SearchBarEvent() {
                isShow = true
            });
        }

        protected T Invoke<T>(Func<T> func) { return func(); }

        private Action<CustomerEntity, CustomerFileEntities> StartFileProcessing {
            get {
                return (customer, files) => { 
                    if(customer.IsNullOrEmpty()) { return; }
                    //else
                    endpointClient.queryDocumentsByDirectoryID(customer.CustomerID)
                                  .ToResponse<List<DocumentEntity>>()
                                  .SelectMany(x => {
                                      if(x.Code == ResponseCode.OK) {
                                          List<DocumentEntity> documents = x.Data;
                                          if(!documents.IsNullOrEmpty()) {
                                              string syncDirectory = preferenceManager.getValue(PreferenceManager.KEY_SYNC_FOLDER, string.Empty);
                                              if(!syncDirectory.IsNullOrEmpty()) {
                                                  DirectoryInfo dir = new DirectoryInfo(Path.Combine(syncDirectory, customer.CustomerID.ToString()));
                                                  if(!dir.Exists) {
                                                      dir.Create();
                                                  }
                                                  files.ToList()
                                                       .ForEach(y => {
                                                           FileInfo compressed = new FileInfo(y.FullName + ".gz");
                                                           if(compressed.Exists) {
                                                               compressed.Delete();
                                                           }
                                                           FileInfo newLocation = new FileInfo(Path.Combine(dir.FullName, y.Name));
                                                           y.MoveTo(newLocation.FullName);
                                                           DocumentEntity docx = documents.FirstOrDefault(z => z.ContentLength == y.Length && z.DocumentName.Equals(y.Name));
                                                           if(!docx.IsNullOrEmpty()) {
                                                               Syncable syncable = new Syncable() {
                                                                   RemoteID = docx.DocumentID,
                                                                   LastModified = y.LastWriteTime,
                                                                   FileName = y.Name,
                                                                   LocalPath = newLocation.FullName
                                                               };
                                                               dbManager.Syncables.Add(syncable);
                                                               dbManager.SaveChanges();
                                                           }
                                                       });
                                                  return Observable.Return(documents);
                                              }
                                          }
                                      } else {
                                          //an error occured
                                          x.PersistResponseError();
                                      }
                                      return Observable.Never<List<DocumentEntity>>();
                                  })
                                  .SubscribeOn(ThreadPoolScheduler.Instance)
                                  .ObserveOnDispatcher(DispatcherPriority.Background)
                                  .Subscribe(x => {
                                      if(!CustomerFiles.IsNullOrEmpty()) {
                                          CustomerFiles.Clear();
                                      } else {
                                          CustomerFiles = new DocumentEntities();
                                      }
                                      x.ForEach(y => CustomerFiles.Add(y));
                                  }, error => {
                                      error.PersistException();
                                  }, () => {
                                      BusManager.Send(new DialogEvent() {
                                          isCloseEvent = true
                                      });
                                  });

                };
            }
        }
    }
}
