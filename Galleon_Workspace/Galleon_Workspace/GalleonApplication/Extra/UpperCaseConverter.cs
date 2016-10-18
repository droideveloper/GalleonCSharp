using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GalleonApplication.Extra {

    public class UpperCaseConverter : IValueConverter {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string str = (value ?? "").ToString();
            return culture.TextInfo.ToTitleCase(str);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            string str = (value ?? "").ToString();
            return culture.TextInfo.ToTitleCase(str);
        }
    }
}
