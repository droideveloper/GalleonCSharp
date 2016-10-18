using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GalleonApplication.Extra {
    
    public class VisibilityColapsedConverter : IValueConverter {

        private Converter<object, bool> Converter {
            get {
                return (o) => {
                    return Boolean.Parse((o ?? false).ToString());
                };
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return Converter.Invoke(value) ? Visibility.Visible : Visibility.Collapsed;            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return (value ?? Visibility.Hidden).ToString().ToEnum<Visibility>() == Visibility.Visible;
        }
    }
}
