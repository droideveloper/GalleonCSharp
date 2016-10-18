using GalaSoft.MvvmLight.Command;
using GalleonApplication.Extra.Net;
using GalleonApplication.Extra;
using Refit;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Net.Http;
using GalleonApplication.Managers;
using GalleonApplication.Events;
using GalleonApplication.ContentViews;
using MaterialDesignThemes.Wpf;
using System.Threading;
using System.Reactive.Concurrency;
using System.Windows;
using System.Windows.Threading;

namespace GalleonApplication.ViewModels {
    
    public class SigninViewModel : IPropertyChanged {

        private string userName;
        private string password;
        private string passwordMask;
        private bool rememberMe;
        private IEndpointClient endpointClient;
        private IPreferenceManager preferenceManager;

        public bool RememberMe {
            get {
                return rememberMe = preferenceManager.getValue(PreferenceManager.KEY_REMEMBER_ME, false);
            }
            set {
                setProperty(ref rememberMe, value);
                preferenceManager.putValue(PreferenceManager.KEY_REMEMBER_ME, rememberMe);
                if(!rememberMe) {
                    preferenceManager.putValue(PreferenceManager.KEY_USER_NAME, string.Empty);
                    preferenceManager.putValue(PreferenceManager.KEY_PASSWORD, string.Empty);
                }
            }
        }        

        public string UserName {
            get {
                return userName;
            }
            set {
                setProperty(ref userName, value);
                if(rememberMe && !systemLogin) {
                    preferenceManager.putValue(PreferenceManager.KEY_USER_NAME, value);
                }
            }
        }

        public string PasswordMask {
            get {
                return passwordMask;
            }
            set {
                if(password == null) { password = ""; }
                password += invoke<string>(() => {
                    return string.IsNullOrEmpty(value) ? "" : value.Contains("*") ? value.Replace("*", "") : value; 
                });
                //in case content started to delete
                password = password.Substring(0, value.Length);
                setProperty(ref passwordMask, invoke<string>(() => {
                    return new string(value.ToCharArray().Select(c => '*').ToArray());
                }), "Password");
                if(rememberMe && !systemLogin) {
                    preferenceManager.putValue(PreferenceManager.KEY_PASSWORD, Password);
                }
            }
        }

        public string Password {
            get {
                MD5 md5 = MD5.Create();
                byte[] source = Encoding.UTF8.GetBytes(password);
                byte[] sink = md5.ComputeHash(source);
                StringBuilder str = new StringBuilder();
                sink.ToList().ForEach(s => str.Append(s.ToString("X2")));
                return str.ToString();
            }
        }

        public ICommand LoginCommand {
            get {
                return new RelayCommand(() => {
                    if(UserName.IsNullOrEmpty()) {
                        BusManager.Send(new SnackbarEvent() {
                            textMessage = Properties.Resources.InvalidUserNameText,
                            isCloseEvent = false,
                            withDuration = null
                        });
                        return;//quit execution
                    } else if(PasswordMask.IsNullOrEmpty()) {
                        BusManager.Send(new SnackbarEvent() {
                            textMessage = Properties.Resources.InvalidPasswordText,
                            isCloseEvent = false,
                            withDuration = null
                        });
                        return;//quit execution
                    }

                    BusManager.Send(new DialogEvent() {
                        view = new ProgressContentView(),
                        identifier = null,
                        closeHandler = new DialogClosingEventHandler((sender, args) => {
                            if(!cancelSignin.IsNullOrEmpty()) {
                                cancelSignin.Cancel();
                                cancelSignin.Dispose();
                            }
                        })
                    });

                    if(cancelSignin == null) {
                        cancelSignin = new CancellationTokenSource();
                    }

                    endpointClient.tryLogin(new LoginEntity() { UserName = this.UserName, Password = this.Password })
                                   .ToResponse<SessionEntity>()
                                   .SubscribeOn(ThreadPoolScheduler.Instance)
                                   .ObserveOnDispatcher(DispatcherPriority.Background)
                                   .Subscribe(OnSuccess, OnError, () => {
                                       BusManager.Send(new DialogEvent() {
                                           isCloseEvent = true
                                       }); 
                                   }, cancelSignin.Token);
                    
                });
            }
        }

        private Action<Exception> OnError {
            get {
                return error => {
                    BusManager.Send(new SnackbarEvent() {
                        isCloseEvent = false,
                        textMessage = Properties.Resources.UnexpectedErrorText,
                        withDuration = null
                    });
                    //logging mechanism
                    error.PersistException();
                };
            }
        }

        private Action<Response<SessionEntity>> OnSuccess {
            get {
                return x => {
                    if(x.Code == ResponseCode.OK) {
                        if(rememberMe && !systemLogin) {
                            preferenceManager.putValue(PreferenceManager.KEY_USER_NAME, this.UserName);
                            preferenceManager.putValue(PreferenceManager.KEY_PASSWORD,  this.Password);
                        }
                        App.Current.Properties.Add("X-Auth-Token", x.Data.Token);
                        BusManager.Send(new LoginEvent() {
                            isSuccess = true
                        });
                    } else {
                        //store as error
                        x.PersistResponseError();
                    }                    
                    //if it's true change it
                    systemLogin = systemLogin ? false : systemLogin;
                };
            }
        }

        private bool systemLogin;
        private CancellationTokenSource cancelSignin;

        public SigninViewModel(IEndpointClient endpointClient, IPreferenceManager preferenceManager) {
            this.endpointClient = endpointClient;
            this.preferenceManager = preferenceManager;
        }

        public void OnStart() {
            if(RememberMe) { 
                string username = preferenceManager.getValue(PreferenceManager.KEY_USER_NAME, string.Empty);
                string password = preferenceManager.getValue(PreferenceManager.KEY_PASSWORD, string.Empty);
                if(!username.IsNullOrEmpty() && !password.IsNullOrEmpty()) {
                    systemLogin = true;
                    UserName = username;//else ui shows error
                    PasswordMask = password;//else ui shows error
                    BusManager.Send(new DialogEvent() {
                        view = new ProgressContentView(),
                        identifier = null,
                        closeHandler = new DialogClosingEventHandler((sender, args) => {
                            if(!cancelSignin.IsNullOrEmpty()) {
                                cancelSignin.Cancel();
                                cancelSignin.Dispose();
                                cancelSignin = null;
                            }
                        })
                    });
                    if(cancelSignin == null) {
                        cancelSignin = new CancellationTokenSource();
                    }
                    endpointClient.tryLogin(new LoginEntity() { UserName = username, Password = password })
                        .ToResponse<SessionEntity>()
                        .SubscribeOn(ThreadPoolScheduler.Instance)
                        .ObserveOnDispatcher(DispatcherPriority.Background)
                        .Subscribe(OnSuccess, OnError, () => {
                            BusManager.Send(new DialogEvent() {
                                isCloseEvent = true
                            });     
                        }, cancelSignin.Token);
                } else {
                    BusManager.Send(new SnackbarEvent() {
                        textMessage = Properties.Resources.UnexpectedErrorText,
                        isCloseEvent = false,
                        withDuration = null
                    });
                }
            }
        }

        public void OnStop() {
            if(!cancelSignin.IsNullOrEmpty()) {
                cancelSignin.Cancel();
                cancelSignin.Dispose();
                cancelSignin = null;
            }
        }
    }
}
