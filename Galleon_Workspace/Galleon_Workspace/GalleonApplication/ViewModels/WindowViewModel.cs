using GalaSoft.MvvmLight.CommandWpf;
using GalleonApplication.Events;
using GalleonApplication.Extra;
using GalleonApplication.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GalleonApplication.ViewModels {
    
    public class WindowViewModel : IPropertyChanged {

        private int position;
        private object[] vewModels;

        public int Position {
            get {
                return position;
            }
            set {
                setProperty(ref position, value);
            }
        }

        public object[] ViewModels {
            get {
                return vewModels;
            }
            set {
                setProperty(ref vewModels, value);
            }
        }

        private bool isDisplayDialog;
        public bool IsDisplayDialog {
            get {
                return isDisplayDialog;
            }
            set {
                setProperty(ref isDisplayDialog, value);
            }
        }

        private IDisposable subscription;

        public WindowViewModel(SigninViewModel mSigninViewModel, MainViewModel mMainViewModel) {
            //contents
            ViewModels = new object[] {
                mSigninViewModel, 
                mMainViewModel 
            };
            ShowLogin();
        }

        private void firePropertyChanged([CallerMemberName] string propertyName = null) {
            OnPropertyChanged(propertyName);
        }

        public void OnStart() {
            subscription = BusManager.Add((evnt) => {
                if(evnt is DialogEvent) {
                    DialogEvent ev = evnt as DialogEvent;
                    if(ev.isCloseEvent) {
                        Application.Current.Dispatcher.Invoke(() => {
                            IsDisplayDialog = false;
                        });
                    }
                }
            });
        }

        public void onStop() {
            BusManager.Remove(subscription);
        }

        public void ShowLogin() {
            Position = 0;
        }

        public void ShowMain() {
            Position = 1;
        }
    }
}
