using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GalleonApplication.Extra {
    
    public class EnabledConverter : IValueConverter {

        private Converter<object, bool> Converter {
            get {
                return (o) => {
                    return Boolean.Parse((o ?? false).ToString());
                };
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return !Converter.Invoke(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return !Converter.Invoke(value);
        }
    }
}
