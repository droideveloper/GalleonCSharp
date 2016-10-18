using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GalleonApplication.Extra {

    public class CommandParamsConverter : IMultiValueConverter {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return new ParameterArray((object[]) values.Clone());
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
