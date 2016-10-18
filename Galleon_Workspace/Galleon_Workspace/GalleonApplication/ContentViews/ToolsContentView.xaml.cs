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
    /// Interaction logic for ToolsContentView.xaml
    /// </summary>
    public partial class ToolsContentView : UserControl {

        private IDisposable subscription;

        public ToolsContentView() {
            InitializeComponent();
            Loaded += OnLoad;
            Unloaded += OnUnload;
        }

        protected RoutedEventHandler OnLoad {
            get {
                return (sender, args) => {
                    ToolsViewModel viewModel = DataContext as ToolsViewModel;
                    viewModel.OnStart();
                    //register drag and drop event
                    FileUpload.AllowDrop = true;
                    FileUpload.Drop += viewModel.OnFileDragAndDrop;
                    //register
                    subscription = BusManager.Add((evnt) => {
                        if(evnt is PreviewEvent) {
                            App.Current.Dispatcher.Invoke(() => {
                                PreviewEvent evt = evnt as PreviewEvent;
                                if(evt != null) {
                                    if(evt.isDisplayEvent) {
                                        PdfViewer.OpenFile(evt.DisplayFile);
                                    } else {
                                        PdfViewer.UnloadPdf();
                                    }
                                }
                            });
                        }
                    });
                };
            }
        }

        protected RoutedEventHandler OnUnload {
            get {
                return (sender, args) => {
                    ToolsViewModel viewModel = DataContext as ToolsViewModel;
                    viewModel.OnStop();
                    //unregister drag and drop event
                    FileUpload.AllowDrop = false;
                    FileUpload.Drop -= viewModel.OnFileDragAndDrop;
                    //unregister
                    if(subscription != null) {
                        BusManager.Remove(subscription);
                    }
                };
            }
        }
    }
}
