using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra {

    public abstract class IPropertyChanged : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// method
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// field setter as property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool setProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null) {
            if(EqualityComparer<T>.Default.Equals(field, value)) { return false; }//checks if it's same or not
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// invoke helper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected T invoke<T>(Func<T> func) {
            return func();
        }
    }
}
