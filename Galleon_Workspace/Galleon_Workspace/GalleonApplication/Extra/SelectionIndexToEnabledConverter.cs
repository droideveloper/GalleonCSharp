using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GalleonApplication.Extra {

    public class SelectionIndexToEnabledConverter : IValueConverter {
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return Int32.Parse((value ?? "-1").ToString()) >= 0;    
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
