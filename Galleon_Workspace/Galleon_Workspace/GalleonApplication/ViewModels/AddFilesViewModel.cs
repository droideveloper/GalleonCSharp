using GalaSoft.MvvmLight.CommandWpf;
using GalleonApplication.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GalleonApplication.ViewModels {

    public class AddFilesViewModel : IPropertyChanged {

        private CustomerFileEntities addFiles;

        public ICommand RemoveAtCommand {
            get {
                return new RelayCommand<int>((index) => {
                    if(index >= 0) {
                        if(AddFiles != null) {
                            if(AddFiles.Count() > index) {
                                AddFiles.RemoveAt(index);
                            }
                        }
                    }
                });
            }
        }

        public DragEventHandler AddFileEvent {
            get {
                return (sender, args) => {
                    if(AddFiles.IsNullOrEmpty()) {
                        AddFiles = new CustomerFileEntities();
                    } else {
                        AddFiles.Clear();
                    }
                    args.ToFileList()
                        .Distinct()
                        .OrderBy(x => x)//order by name only since it's string only
                        .ToList()
                        .ToFileList()
                        .Subscribe(x => {
                            x.ForEach(y => AddFiles.Add(y));
                        });
                };
            }
        }

        public CustomerFileEntities AddFiles {
            get {
                return addFiles;
            }
            set {
                setProperty(ref addFiles, value);
            }
        }

        /// <summary>
        /// Default Constructor as you see
        /// </summary>
        public AddFilesViewModel() { }
    }
}
