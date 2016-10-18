using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Toast.Converters {

    public class CharacterCasingConverter : IValueConverter	{

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			CharacterCasing casing;
			if (value == null || parameter == null || !Enum.TryParse<CharacterCasing>(parameter.ToString(), out casing)) {
				return value;
			}

			string str = value.ToString();
			switch (casing)	{
				case CharacterCasing.Lower:
					return str.ToLower(culture);
				case CharacterCasing.Normal:
					return str;
				case CharacterCasing.Upper:
					return str.ToUpper(culture);
				default:
					break;
			}

			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
