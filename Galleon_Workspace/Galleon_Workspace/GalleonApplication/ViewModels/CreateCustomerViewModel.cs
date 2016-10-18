using GalaSoft.MvvmLight.CommandWpf;
using GalleonApplication.ContentViews;
using GalleonApplication.Events;
using GalleonApplication.Extra;
using GalleonApplication.Extra.Net;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Reactive.Concurrency;
using System.Windows.Threading;
using GalleonApplication.Managers;
using System.IO;
using GalleonApplication.App_Data;
using System.Diagnostics;

namespace GalleonApplication.ViewModels {
    
    public class CreateCustomerViewModel : IPropertyChanged {

        private string firstName;
        private string middleName;
        private string lastName;
        private string identity;
        private CategoryEntity  category;

        private IEndpointClient endpointClient;

        private CategoryEntities categories;
        private ContactEntities contacts;
        private CustomerFileEntities customerFiles;

        private int customerID;

        public string FirstName {
            get {
                return firstName;
            }
            set {
                setProperty(ref firstName, value);
            }
        }

        public string MiddleName {
            get {
                return middleName;
            }
            set {
                setProperty(ref middleName, value);
            }
        }

        public string LastName {
            get {
                return lastName;
            }
            set {
                setProperty(ref lastName, value);
            }
        }

        public string Identity {
            get {
                return identity;
            }
            set {
                setProperty(ref identity, value);
            }           
        }

        public CategoryEntity Category {
            get {
                return category;
            }
            set {
                setProperty(ref category, value);
            }
        }

        public CategoryEntities Categories {
            get {
                return categories;
            }
            set {
                setProperty(ref categories, value);
            }            
        }

        public ContactEntities Contacts {
            get {
                return contacts;
            }
            set {
                setProperty(ref contacts, value);
            }
        }

        public CustomerFileEntities CustomerFiles {
            get {
                return customerFiles;
            }
            set {
                setProperty(ref customerFiles, value);
            }
        }

        private bool showCategoriesProgress;
        public bool ShowCategoriesProgress {
            get {
                return showCategoriesProgress;
            }
            set {
                setProperty(ref showCategoriesProgress, value);
            }
        }

        public ICommand RemoveFileCommand {
            get {
                return new RelayCommand<int>(index => {
                    if(!CustomerFiles.IsNullOrEmpty()) {
                        if(index < CustomerFiles.Count() && index >= 0) {
                            CustomerFiles.RemoveAt(index);
                        }
                    }
                });
            }
        }

        public ICommand AddContactCommand {
            get {
                return new RelayCommand(() => {
                    BusManager.Send(new DialogEvent() {
                        view = new AddNewContactContentView(),
                        identifier = null,
                        closeHandler = OnCloseDialog
                    });
                });
            }
        }

        public ICommand RemoveContactCommand {
            get {
                return new RelayCommand<int>(index => {
                    if(!Contacts.IsNullOrEmpty()) {
                        if(index < Contacts.Count() && index >= 0) {
                            Contacts.RemoveAt(index);
                        }
                    }
                });
            }
        }

        public ICommand EditContactCommand {
            get {
                return new RelayCommand<int>(index => {
                    if(!Contacts.IsNullOrEmpty()) {
                        if(index < Contacts.Count() && index >= 0) {
                            ContactEntity edit = Contacts.ElementAt(index);
                            BusManager.Send(new DialogEvent() {
                                view = new AddNewContactContentView(edit),
                                identifier = null,
                                closeHandler = (sender, args) => {
                                    if(args.Parameter is bool) { return;//Cancel Clicked.
                                    } else if(args.Parameter is ContactEntity) {
                                        ContactEntity entity = args.Parameter as ContactEntity;
                                        if(entity != null) {
                                            //check has name
                                            if(IsPropertyInvalid(entity.ContactName)) {
                                                ShowSnackbar(Properties.Resources.InvalidContactNameText);
                                                CancelDialogExit(args);
                                                return;
                                            }
                                            //check has address
                                            if(IsPropertyInvalid(entity.Address)) {
                                                ShowSnackbar(Properties.Resources.InvalidAddressText);
                                                CancelDialogExit(args);
                                                return;
                                            }
                                            //check has phone
                                            if(IsPropertyInvalid(entity.Phone)) {
                                                ShowSnackbar(Properties.Resources.InvalidPhoneText);
                                                CancelDialogExit(args);
                                                return;
                                            }
                                            //check has country
                                            if(IsPropertyInvalid(entity.Country)) {
                                                ShowSnackbar(Properties.Resources.CountryHint + "\t" + Properties.Resources.InvalidOptionText);
                                                CancelDialogExit(args);
                                                return;
                                            }
                                            //check has city
                                            if(IsPropertyInvalid(entity.City)) {
                                                ShowSnackbar(Properties.Resources.CityHint + "\t" + Properties.Resources.InvalidOptionText);
                                                CancelDialogExit(args);
                                                return;
                                            }                                           
                                        }
                                    }
                                }
                            });                                       
                        }
                    }
                });
            }
        }

        public ICommand ClearCustomerCommand {
            get {
                return new RelayCommand(() => {
                    FirstName = null;
                    MiddleName = null;
                    LastName = null;
                    Identity = null;
                    Category = null;
                    if(!Contacts.IsNullOrEmpty()) {
                        Contacts.Clear();
                    }
                    if(!CustomerFiles.IsNullOrEmpty()) {
                        CustomerFiles.Clear();
                    }
                });
            }
        }

        public ICommand SaveCustomerCommand {
            get {
                return new RelayCommand(() => {
                    if(IsPropertyInvalid(FirstName)) {
                        ShowSnackbar(Properties.Resources.InvalidCustomerNameText);
                        return;
                    }
                    if(IsPropertyInvalid(LastName)) {
                        ShowSnackbar(Properties.Resources.InvalidCustomerSurnameText);
                        return;
                    }
                    if(IsPropertyInvalid(Identity)) {
                        ShowSnackbar(Properties.Resources.InvalidIdentityText);
                        return;
                    }
                    if(IsPropertyInvalid(Category)) {
                        ShowSnackbar(Properties.Resources.CategoryHint + "\t" + Properties.Resources.InvalidOptionText);
                        return;
                    }

                    BusManager.Send(new DialogEvent() {
                        view = new ProgressContentView(),
                        isCloseEvent = false,
                        identifier = null,
                        closeHandler = null
                    });
                    //should do this way but when I get ok then I should continue to other request which will continue with same instance of code not the all
                    endpointClient.createCustomer(ToCustomer)
                                .ToResponse<CustomerEntity>()
                                .SelectMany(x => {
                                    if(x.Code == ResponseCode.OK) {
                                        CustomerEntity customer = x.Data;
                                        if(customer.IsNullOrEmpty()) {
                                            return Observable.Empty<List<ContactEntity>>();//stop
                                        } else {
                                            return Observable.Return(ToContacts(customer));
                                        }
                                    } else {
                                        x.PersistResponseError();//store response
                                        return Observable.Empty<List<ContactEntity>>();//stop
                                    }
                                })
                                .SelectMany(x => {
                                    return endpointClient.createContacts(x)
                                                            .ToResponse<List<ContactEntity>>();
                                })
                                .SelectMany(x => {
                                    if(x.Code == ResponseCode.OK) {
                                        ContactEntity contact = x.Data.FirstOrDefault();
                                        if(contact.IsNullOrEmpty()) {
                                            return Observable.Empty<DirectoryEntity>();
                                        } else {
                                            this.customerID = contact.CustomerID;//store customer id
                                            return Observable.Return(new DirectoryEntity() {
                                                DirectoryName = contact.CustomerID.ToString(),
                                                CustomerID = contact.CustomerID//this is important
                                            });
                                        }
                                    } else {
                                        x.PersistResponseError();
                                        return Observable.Empty<DirectoryEntity>();
                                    }
                                })
                                .SelectMany(x => {
                                    return endpointClient.createDirectory(x)
                                                            .ToResponse<DirectoryEntity>();
                                })
                                .SelectMany(x => {
                                    if(x.Code == ResponseCode.OK) {
                                        DirectoryEntity directory = x.Data;
                                        if(directory.IsNullOrEmpty()) {
                                            return Observable.Empty<List<DocumentEntity>>();
                                        } else {
                                            if(CustomerFiles.IsNullOrEmpty()) {
                                                return Observable.Empty<List<DocumentEntity>>();
                                            } else {
                                                return Observable.Return(CustomerFiles.Select(f => {
                                                    return new DocumentEntity() {
                                                        DirectoryID = directory.DirectoryID,
                                                        DocumentName = f.Name,
                                                        CustomerID = directory.CustomerID,//added
                                                        ContentType = f.Name.Split('.').LastOrDefault(),
                                                        ContentLength = f.Length,
                                                        CreateDate = f.CreationTime,
                                                        UpdateDate = f.LastWriteTime
                                                    };
                                                }).ToList());
                                            }
                                        }
                                    } else {
                                        x.PersistResponseError();
                                        return Observable.Empty<List<DocumentEntity>>();
                                    }
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
                                        Content = CustomerFiles.FirstOrDefault(z => z.Length == y.ContentLength
                                                                                    && z.Name.Equals(y.DocumentName))
                                    }).ToList());
                                })
                                .SelectMany(x => x)
                                .SelectMany(x => {
                                    IFileClient fileClient = DependencyInjector.Get<IFileClient>();
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
                                .Subscribe(OnSuccessCustomerSave, OnErrorCustomerSave, () => {
                                    //store syncable and move files
                                    StartFileProcessing();
                                });
                });
            }
        }

        private Action<Response<bool>> OnSuccessCustomerSave {
            get {
                return x => {
                    if(x.Code == ResponseCode.OK) {
                        Debug.WriteLine(x.Message);
                    } else {
                        x.PersistResponseError();                      
                    }
                };
            }
        }

        private Action<Exception> OnErrorCustomerSave {
            get {
                return error => {
                    //also close dialog
                    BusManager.Send(new DialogEvent() {
                        isCloseEvent = true
                    });
                    ShowSnackbar(Properties.Resources.UnexpectedErrorText);
                    //store in database
                    error.PersistException();
                };
            }
        }

        private Action StartFileProcessing {
            get {
                return () => {
                    endpointClient.queryDocumentsByDirectoryID(customerID)
                            .ToResponse<List<DocumentEntity>>()
                            .SelectMany(x => {
                                if(x.Code == ResponseCode.OK) {
                                    List<DocumentEntity> documents = x.Data;
                                    if(!documents.IsNullOrEmpty()) {
                                        DbManager dbManager = DependencyInjector.Get<DbManager>();
                                        IPreferenceManager preferenceManager = DependencyInjector.Get<IPreferenceManager>();
                                        string syncableDirectory = preferenceManager.getValue(PreferenceManager.KEY_SYNC_FOLDER, string.Empty);
                                        if(syncableDirectory.IsNullOrEmpty()) {
                                            return Observable.Empty<bool>();
                                        } else {
                                            DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(syncableDirectory, customerID.ToString()));
                                            if(!directoryInfo.Exists) {
                                                directoryInfo.Create();
                                            }
                                            CustomerFiles.ToList().ForEach(y => {
                                                FileInfo compressed = new FileInfo(y.FullName + ".gz");
                                                if(compressed.Exists) {
                                                    compressed.Delete();
                                                }
                                                FileInfo newLocation = new FileInfo(Path.Combine(directoryInfo.FullName, y.Name));
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
                                        }
                                    }
                                    return Observable.Return(true);
                                }
                                return Observable.Empty<bool>();
                            })
                            .SubscribeOn(ThreadPoolScheduler.Instance)
                            .ObserveOnDispatcher(DispatcherPriority.Background)
                            .Subscribe(x => {
                                ShowSnackbar(Properties.Resources.SaveCustomerSuccessText);
                            }, err => {
                                BusManager.Send(new DialogEvent() {
                                    isCloseEvent = true
                                });
                                err.PersistException();
                            }, () => {
                                BusManager.Send(new DialogEvent() {
                                    isCloseEvent = true
                                });
                            });
                    
                };
            }
        }

        private CustomerEntity ToCustomer {
            get {
                return new CustomerEntity() {
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    Identity = this.Identity,
                    CategoryID = this.Category.CategoryID
                };
            }
        }

        private Func<CustomerEntity, List<ContactEntity>> ToContacts {
            get {
                return c => {
                    return Contacts.Select(x => new ContactEntity() {
                        CustomerID = c.CustomerID,
                        ContactName = x.ContactName,
                        Address = x.Address,
                        Phone = x.Phone,
                        CountryID = x.Country.CountryID,
                        CityID = x.City.CityID
                    }).ToList();
                };
            }
        }

        private DialogClosingEventHandler OnCloseDialog {
            get {
                return (sender, args) => {
                    if(args.Parameter is bool) { return;//Cancel Clicked.
                    } else if(args.Parameter is ContactEntity) {
                        ContactEntity entity = args.Parameter as ContactEntity;
                        if(entity != null) {
                            //check has name
                            if(IsPropertyInvalid(entity.ContactName)) {
                                ShowSnackbar(Properties.Resources.InvalidContactNameText);
                                CancelDialogExit(args);
                                return;
                            }
                            //check has address
                            if(IsPropertyInvalid(entity.Address)) {
                                ShowSnackbar(Properties.Resources.InvalidAddressText);
                                CancelDialogExit(args);
                                return;
                            }
                            //check has phone
                            if(IsPropertyInvalid(entity.Phone)) {
                                ShowSnackbar(Properties.Resources.InvalidPhoneText);
                                CancelDialogExit(args);
                                return;
                            }
                            //check has country
                            if(IsPropertyInvalid(entity.Country)) {
                                ShowSnackbar(Properties.Resources.CountryHint + "\t" + Properties.Resources.InvalidOptionText);
                                CancelDialogExit(args);
                                return;
                            }
                            //check has city
                            if(IsPropertyInvalid(entity.City)) {
                                ShowSnackbar(Properties.Resources.CityHint + "\t" + Properties.Resources.InvalidOptionText);
                                CancelDialogExit(args);
                                return;
                            }
                            //it's ok for us to bind it know
                            if(Contacts == null) {
                                Contacts = new ContactEntities();
                            }
                            Contacts.Add(entity);
                        }
                    }
                };
            }
        }

        private Action<string> ShowSnackbar {
            get {
                return str => {
                    BusManager.Send(new SnackbarEvent() {
                        isCloseEvent = false,
                        textMessage = str,
                        withDuration = null
                    });
                };
            }
        }

        private Action<DialogClosingEventArgs> CancelDialogExit {
            get {
                return args => {
                    args.Cancel();
                };
            }
        }

        public DragEventHandler OnFilesDrop {
            get {
                return (sender, e) => {
                    if(CustomerFiles.IsNullOrEmpty()) {
                        CustomerFiles = new CustomerFileEntities();
                    } else {
                        CustomerFiles.Clear();
                    }
                    //linq is nice!
                    e.ToFileList()
                     .Distinct()
                     .OrderBy(x => x)//name I guess
                     .ToList()
                     .ToFileList()
                     .Subscribe(x => {
                         x.ForEach(y => CustomerFiles.Add(y));
                     });
                };
            }
        }

        public CreateCustomerViewModel(IEndpointClient endpointClient) {
            this.endpointClient = endpointClient;
        }

        private bool IsPropertyInvalid<T>(T property) {
            return property is int ? Convert.ToInt32(property) == 0 : property.IsNullOrEmpty();
        }

        public void OnStart() {            
            if(Categories == null) {
                ShowCategoriesProgress = true;
                //get categories
                endpointClient.queryCategories()
                    .ToResponse<List<CategoryEntity>>()
                    .SubscribeOn(ThreadPoolScheduler.Instance)
                    .ObserveOnDispatcher(DispatcherPriority.Background)
                    .Subscribe(x => {
                        if(x.Code == ResponseCode.OK) {
                            List<CategoryEntity> dataSet = x.Data;
                            if(!dataSet.IsNullOrEmpty()) {
                                if(Categories.IsNullOrEmpty()) {
                                    Categories = new CategoryEntities();
                                }
                                dataSet.ForEach(c => Categories.Add(c));
                            }
                        } else {
                            x.PersistResponseError();
                        }
                    }, error => {
                        ShowCategoriesProgress = false;
                        ShowSnackbar(Properties.Resources.UnexpectedErrorText);
                        error.PersistException();
                    }, () => {
                        ShowCategoriesProgress = false;
                    });
            }
        }

        public void OnStop() {
            ShowCategoriesProgress = false;            
        }
    }
}
