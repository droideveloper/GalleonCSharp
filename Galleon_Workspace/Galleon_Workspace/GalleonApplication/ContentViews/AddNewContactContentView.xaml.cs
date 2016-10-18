using GalleonApplication.Extra;
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
    /// Interaction logic for AddNewContactContentView.xaml
    /// </summary>
    public partial class AddNewContactContentView : UserControl {
           
        public AddNewContactContentView() {
            InitializeComponent();
            Loaded += OnLoad;
            Unloaded += OnUnLoad;
        }

        public AddNewContactContentView(ContactEntity edit) : this() {
            AddNewContactViewModel viewModel = DataContext as AddNewContactViewModel;
            viewModel.ContactEntity = edit;
        }

        private RoutedEventHandler OnLoad { 
            get {
                 return (sender, args) => {
                     AddNewContactViewModel viewModel = DataContext as AddNewContactViewModel;
                     if(viewModel != null) {
                         viewModel.OnStart();
                     }
                 }; 
            }
        }

        private RoutedEventHandler OnUnLoad {
            get {
                return (sender, args) => {
                    AddNewContactViewModel viewModel = DataContext as AddNewContactViewModel;
                    if(viewModel != null) {
                        viewModel.OnStop();
                    }
                };
            }
        }
    }
}
