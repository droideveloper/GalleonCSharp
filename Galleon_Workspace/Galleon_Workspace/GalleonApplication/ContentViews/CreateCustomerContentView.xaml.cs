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

namespace GalleonApplication.ContentViews {
    /// <summary>
    /// Interaction logic for CreateCustomerView.xaml
    /// </summary>
    public partial class CreateCustomerContentView : UserControl {
        public CreateCustomerContentView() {
            InitializeComponent();
            Loaded += OnLoad;
            Unloaded += OnUnLoad;
        }

        private RoutedEventHandler OnLoad { 
            get {
                return (sender, args) => {
                    CreateCustomerViewModel viewModel = DataContext as CreateCustomerViewModel;
                    viewModel.OnStart();
                    //no binding for this huh
                    FileUpload.AllowDrop = true;
                    FileUpload.Drop += viewModel.OnFilesDrop;                    
                };
            }
        }

        private RoutedEventHandler OnUnLoad {
            get {
                return (sender, args) => {
                    CreateCustomerViewModel viewModel = DataContext as CreateCustomerViewModel;
                    viewModel.OnStop();
                    //no binding for this huh
                    FileUpload.AllowDrop = false;
                    FileUpload.Drop -= viewModel.OnFilesDrop;                 
                };
            }
        }   
    }
}
