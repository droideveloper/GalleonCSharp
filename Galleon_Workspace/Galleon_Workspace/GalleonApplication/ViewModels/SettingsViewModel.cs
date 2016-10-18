using GalleonApplication.Extra;
using GalleonApplication.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.ViewModels {
    
    public class SettingsViewModel : IPropertyChanged {

        private IPreferenceManager preferenceManager;

        public bool Sync {
            get {
                return preferenceManager.getValue(PreferenceManager.KEY_SYNC_ALLOWED, false);
            }
            set {
                bool currentValue = preferenceManager.getValue(PreferenceManager.KEY_SYNC_ALLOWED, false);
                if(!value.IsEquals(currentValue)) {
                    preferenceManager.putValue(PreferenceManager.KEY_SYNC_ALLOWED, value);
                    firePropertyChanged();
                }
            }
        }

        public bool RememberMe {
            get {
                return preferenceManager.getValue(PreferenceManager.KEY_REMEMBER_ME, false);
            }
            set {
                bool currentValue = preferenceManager.getValue(PreferenceManager.KEY_REMEMBER_ME, false);
                if(!value.IsEquals(currentValue)) {
                    preferenceManager.putValue(PreferenceManager.KEY_REMEMBER_ME, value);
                    firePropertyChanged();
                }
            }
        }

        public string SyncDirectory {
            get {
                return preferenceManager.getValue(PreferenceManager.KEY_SYNC_FOLDER, string.Empty);
            }
            set {
                string currentValue = preferenceManager.getValue(PreferenceManager.KEY_SYNC_FOLDER, string.Empty);
                if(!value.IsEquals(currentValue)) {
                    preferenceManager.putValue(PreferenceManager.KEY_SYNC_FOLDER, value);
                    firePropertyChanged();
                }
            }
        }

        public string ToolsDirectory {
            get {
                return preferenceManager.getValue(PreferenceManager.KEY_TOOLS_FOLDER, string.Empty);
            }
            set { 
                string currentValue = preferenceManager.getValue(PreferenceManager.KEY_TOOLS_FOLDER, string.Empty);
                if(!value.IsEquals(currentValue)) {
                    preferenceManager.putValue(PreferenceManager.KEY_TOOLS_FOLDER, value);
                    firePropertyChanged();
                }
            }
        }

        public SettingsViewModel(IPreferenceManager preferenceManager) {
            this.preferenceManager = preferenceManager;
        }

        private void firePropertyChanged([CallerMemberName] string propertyName = null) {
            OnPropertyChanged(propertyName);
        }
    }
}
