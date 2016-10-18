using GalleonApplication.Events;
using GalleonApplication.Managers;
using GalleonApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GalleonApplication.ContentViews {
    /// <summary>
    /// Interaction logic for MainContentView.xaml
    /// </summary>
    public partial class MainContentView : UserControl {

        private IDisposable subscription;

        public MainContentView() {
            InitializeComponent();
            //register events
            Loaded      += OnLoaded;
            Unloaded    += OnUnLoaded;
        }

        private RoutedEventHandler OnLoaded {
            get {
                return (sender, args) => {
                    subscription = BusManager.Add(EventListener);
                    MainViewModel viewModel = DataContext as MainViewModel;
                    viewModel.OnStart();
                };
            }
        }

        private RoutedEventHandler OnUnLoaded {
            get {
                return (sender, args) => {
                    BusManager.Remove(subscription);
                    MainViewModel viewModel = DataContext as MainViewModel;
                    viewModel.OnStop();
                };
            }
        }

        private Action<object> EventListener {
            get {
                return (evn) => {
                    Application.Current.Dispatcher.Invoke(() => {
                        if(evn is SnackbarEvent) {
                            SnackbarEvent snack = evn as SnackbarEvent;
                            if(snack.isCloseEvent) {
                                Snackbar.Hide();
                            } else {
                                if(snack.withDuration.HasValue) {
                                    Snackbar.Show(snack.textMessage, snack.withDuration.Value);
                                } else {
                                    Snackbar.Show(snack.textMessage);
                                }
                            }
                        } 
                    });
                };
            }
        }       
    }
}
