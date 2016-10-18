using GalleonApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra {
    
    public class ViewModelLocator {

        public WindowViewModel WindowViewModel {
            get {
                return DependencyInjector.Get<WindowViewModel>();
            }
        }

        public MainViewModel MainViewModel {
            get {
                return DependencyInjector.Get<MainViewModel>();
            }
        }

        public SigninViewModel SigninViewModel {
            get {
                return DependencyInjector.Get<SigninViewModel>();
            }
        }

        public CreateCustomerViewModel CreateCustomerViewModel {
            get {
                return DependencyInjector.Get<CreateCustomerViewModel>();
            }
        }

        public AddNewContactViewModel AddNewContactViewModel {
            get {
                return DependencyInjector.Get<AddNewContactViewModel>();
            }
        }

        public SettingsViewModel SettingsViewModel {
            get {
                return DependencyInjector.Get<SettingsViewModel>();
            }
        }

        public FindCustomerViewModel FindCustomerViewModel {
            get {
                return DependencyInjector.Get<FindCustomerViewModel>();
            }
        }

        public ToolsViewModel ToolsViewModel {
            get {
                return DependencyInjector.Get<ToolsViewModel>();
            }
        }

        public AddFilesViewModel AddFilesViewModel {
            get {
                return DependencyInjector.Get<AddFilesViewModel>();
            }
        }
    }
}
