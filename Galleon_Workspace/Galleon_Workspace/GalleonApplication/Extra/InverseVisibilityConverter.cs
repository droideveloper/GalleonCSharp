using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GalleonApplication.Extra {
    
    public class InverseVisibilityConverter : IValueConverter {

        private Converter<object, bool> Converter {
            get {
                return (o) => {
                    return Boolean.Parse((o ?? false).ToString());
                };
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return Converter.Invoke(value) ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return (value ?? Visibility.Hidden).ToString().ToEnum<Visibility>() != Visibility.Visible;
        }
    }
}
