using GalleonApplication.Events;
using GalleonApplication.Extra;
using GalleonApplication.Managers;
using GalleonApplication.ViewModels;
using MaterialDesignThemes.Wpf;
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

namespace GalleonApplication {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            Loaded += OnLoad;
            Unloaded += OnUnLoad;
        }

        private IDisposable subscription;

        private RoutedEventHandler OnLoad {
            get {
                return (sender, args) => {
                    subscription = BusManager.Add(EventListener);
                    WindowViewModel viewModel = DataContext as WindowViewModel;
                    viewModel.OnStart();
                };
            }
        }

        private RoutedEventHandler OnUnLoad {
            get {
                return (sender, args) => {
                    BusManager.Remove(subscription);
                    WindowViewModel viewModel = DataContext as WindowViewModel;
                    viewModel.onStop();
                };
            }
        }

        private Action<object> EventListener {
            get {
                return (evn) => {
                    //to ensure those ui related tasks handled by ui thread
                    Application.Current.Dispatcher.Invoke(() => {
                        if(evn is DialogEvent) {
                            DialogEvent dialog = evn as DialogEvent;
                            DialogHost.Show(dialog.view, dialog.identifier, dialog.closeHandler);
                        } else if(evn is LoginEvent) {
                            LoginEvent login = evn as LoginEvent;
                            if(login.isSuccess) {
                                WindowViewModel viewModel = DataContext as WindowViewModel;
                                viewModel.ShowMain();
                            }
                        }
                    });
                };
            }
        }       
    }
}
