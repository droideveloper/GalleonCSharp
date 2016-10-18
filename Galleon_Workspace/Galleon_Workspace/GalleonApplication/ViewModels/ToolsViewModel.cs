using GalleonApplication.Extra;
using GalleonApplication.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Leptonica;
using Leptonica.Interop;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;
using GalleonApplication.Events;
using GalleonApplication.ContentViews;

namespace GalleonApplication.ViewModels {
    
    public class ToolsViewModel : IPropertyChanged {

        private const string EXPLORER_EXE = "explorer.exe";
        private const string ANY_FILE     = "*.*";
        private const string PDF_FILE     = "*.pdf";
        private const string PDF_EXTENSION = ".pdf";

        private ToolsFileEntities toolsFiles;
        private int selectedPosition;

        /// <summary>
        /// Clear Command that handled with {Binding ClearCommand}
        /// </summary>
        public ICommand ClearCommand {
            get {
                return new RelayCommand(() => {
                    if(!ToolsFiles.IsNullOrEmpty()) {
                        ToolsFiles.Clear();//clear all in there, may be also the file in there and delete created files ?
                    }
                    if(!toolsPath.IsNullOrEmpty()) {
                        Observable.Return(new DirectoryInfo(toolsPath))
                                  .SelectMany(x => x.GetFiles(ANY_FILE, SearchOption.AllDirectories)
                                                    .ToList())
                                  .SelectMany(x => {
                                      if(x.Exists) {
                                          x.Delete();
                                          return Observable.Return(true);
                                      }
                                      return Observable.Return(false);
                                  }).Subscribe(x => {
                                      //we did delete if true else false
                                  }, error => {
                                      //error
                                      error.PersistException();
                                  }, () => {
                                      //completed
                                  });                              
                    }
                    //exit pdf
                    BusManager.Send(new PreviewEvent() {
                        isDisplayEvent = false
                    });
                });
            }
        }

        /// <summary>
        /// Remove at index of position in ListBox Selected item
        /// </summary>
        public ICommand RemoveAtCommand {
            get {
                return new RelayCommand<int>((index) => {
                    if(ToolsFiles.IsNullOrEmpty()) { return; } //exit already
                    if(index >= 0 && index < ToolsFiles.Count()) {
                        //pdf file if exits
                        FileInfo f = new FileInfo(Path.Combine(toolsPath, ToolsFiles.ElementAt(index)
                                                                                     .ToNewExtension(PDF_EXTENSION)
                                                                                     .Name));
                        if(f.Exists) {
                            f.Delete();
                        }
                        //exit display
                        BusManager.Send(new PreviewEvent() {
                            isDisplayEvent = false
                        });

                        ToolsFiles.RemoveAt(index); 
                    }
                });
            }
        }

        /// <summary>
        /// Process images into files that are pdf and shrink their size in proper spot
        /// </summary>
        public ICommand ProcessImagesCommand {
            get {
                return new RelayCommand(() => {

                    BusManager.Send(new DialogEvent() {
                        view = new ProgressContentView(),
                        isCloseEvent = false,
                        identifier = null,
                        closeHandler = null
                    });

                    cancel = Observable.Return(ToolsFiles)
                                       .SelectMany(x => {
                                           return Observable.Return(x.ToList());
                                       })
                                       .SelectMany(x => x)
                                       .SelectMany(f => {
                                           FileInfo newFile = new FileInfo(Path.Combine(toolsPath, f.ToNewExtension(PDF_EXTENSION).Name));
                                           Pix pixs = Pix.LoadFromFile(f.FullName);
                                           pixs = pixs.PixBackgroundNormSimple();
                                           pixs = pixs.PixConvertRGBToGray();
                                           pixs = pixs.PixFindSkewAndDeskew();
                                           pixs = pixs.PixTophat();
                                           pixs = pixs.PixInvert();
                                           pixs = pixs.PixGammaRTC();
                                           pixs = pixs.PixThresholdToBinary();
                                           pixs.Save(newFile.FullName, ImageSaveFormat.Lpdf);
                                           pixs.Dispose();
                                           return Observable.Return(newFile);
                                       }).SubscribeOn(ThreadPoolScheduler.Instance)
                                        .ObserveOnDispatcher(DispatcherPriority.Background)
                                        .Subscribe(x => {
                                            if(x.Exists) {
                                                BusManager.Send(new PreviewEvent() {
                                                    isDisplayEvent = true,
                                                    DisplayFile = x.FullName
                                                });
                                            }
                                        }, error => {
                                            //error
                                            error.PersistException();
                                        }, () => {
                                            //completed
                                            BusManager.Send(new DialogEvent() {
                                                isCloseEvent = true
                                            });
                                        });
                });
            }
        }

        /// <summary>
        /// Shows Tools directory opened in directory browser window
        /// </summary>
        public ICommand ShowInDirectoryCommand {
            get {
                return new RelayCommand(() => {
                    if(!toolsPath.IsNullOrEmpty()) {
                        Process.Start(EXPLORER_EXE, toolsPath);
                    }
                });
            }
        }

        /// <summary>
        /// Hadle of DragAndDrop Files into panel so we can process those
        /// </summary>
        public DragEventHandler OnFileDragAndDrop {
            get {
                return (sender, args) => {
                    if(ToolsFiles.IsNullOrEmpty()) {
                        ToolsFiles = new ToolsFileEntities();
                    } else {
                        ToolsFiles.Clear();
                    }
                    args.ToFileList()
                        .Distinct()
                        .OrderBy(x => x)//for name I guess
                        .ToList()    
                        .ToFileList()                        
                        .Subscribe(x => {
                            x.ForEach(f => ToolsFiles.Add(f));
                        });
                };
            }
        }

        public int SelectedPosition {
            get {
                return selectedPosition;
            }
            set {
                bool hasNewValue = setProperty(ref selectedPosition, value);
                if(hasNewValue && value >= 0) {//should be positive integer
                    //send preview event
                    //if we keep this part too busy it kinda bitches about it
                    Task.Factory.StartNew(() => {
                        FileInfo f = new FileInfo(Path.Combine(toolsPath, ToolsFiles.ElementAt(value)
                                                                                         .ToNewExtension(PDF_EXTENSION)
                                                                                         .Name));
                        if(f.Exists) {
                            BusManager.Send(new PreviewEvent() {
                                isDisplayEvent = true,
                                DisplayFile = f.FullName
                            });
                        }
                    });
                }
            }
        }

        public ToolsFileEntities ToolsFiles {
            get {
                return toolsFiles;
            }
            set {
                setProperty(ref toolsFiles, value);
            }
        }

        private IPreferenceManager preferenceManager;
        private IDisposable cancel;

        private string toolsPath;

        public ToolsViewModel(IPreferenceManager preferenceManager) {
            this.preferenceManager = preferenceManager;
        } 

        public void OnStart() {
            toolsPath = preferenceManager.getValue(PreferenceManager.KEY_TOOLS_FOLDER, string.Empty);            
        }

        public void OnStop() { }
    }
}
