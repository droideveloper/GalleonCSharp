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
    /// Interaction logic for FindCustomerContentView.xaml
    /// </summary>
    public partial class FindCustomerContentView : UserControl {

        private IDisposable subscription;

        public FindCustomerContentView() {
            InitializeComponent();
            Loaded += OnLoad;
            Unloaded += OnUnLoad;
        }

        private RoutedEventHandler OnUnLoad {
            get {
                return (sender, args) => {
                    FindCustomerViewModel viewModel = DataContext as FindCustomerViewModel;
                    viewModel.OnStop();
                    if(subscription != null) {
                        BusManager.Remove(subscription);
                    }
                };
            }
        }

        private RoutedEventHandler OnLoad {
            get {
                return (sender, args) => {
                    FindCustomerViewModel viewModel = DataContext as FindCustomerViewModel;
                    viewModel.OnStart();
                    subscription = BusManager.Add(EventListener);
                };
            }
        }

        private Action<object> EventListener {
            get {
                return (evnt) => {
                    if(evnt is SelectedFileChangedEvent) {
                        SelectedFileChangedEvent evt = evnt as SelectedFileChangedEvent;
                        if(evt != null) {
                            CustomerFilesListBox.SelectedItem = null;
                            CustomerFilesListBox.SelectedItem = evt.Selection;
                        }
                    }
                };
            }
        }
    }
}
