using GalleonApplication.Managers;
using GalleonApplication.ViewModels;
using GalleonApplication.Events;
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
    /// Interaction logic for AddFilesContentView.xaml
    /// </summary>
    public partial class AddFilesContentView : UserControl {

        public AddFilesContentView() {
            InitializeComponent();
            Loaded += OnLoaded;//event of register
            Unloaded += OnUnloaded;//event of unregister
        }

        private RoutedEventHandler OnLoaded {
            get {
                return (sender, args) => {
                    AddFilesViewModel viewModel = DataContext as AddFilesViewModel;
                    if(viewModel != null) {
                        FilesDrag.Drop += viewModel.AddFileEvent;
                        FilesDrag.AllowDrop = true;
                    }
                };
            }
        }

        private RoutedEventHandler OnUnloaded { 
            get {
                return (sender, args) => {
                    AddFilesViewModel viewModel = DataContext as AddFilesViewModel;
                    if(viewModel != null) {
                        FilesDrag.Drop -= viewModel.AddFileEvent;
                        FilesDrag.AllowDrop = false;
                    }
                   
                };
            }
        }       
    }
}
