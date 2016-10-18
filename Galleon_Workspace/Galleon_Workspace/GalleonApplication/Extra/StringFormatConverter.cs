using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GalleonApplication.Extra {
    public class StringFormatConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return parse((value ?? "").ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return parseBack((value ?? "").ToString());
        }

        public string parse(string str) {
            string format = string.Join(String.Empty, str.ToArray().Select((c, i) => i == 0 ? "({" + i + "}" 
                                                                                            : i == 2 ? "{" + i + "})" 
                                                                                                     : i == 3 ? " {" + i + "}" 
                                                                                                              : i == 5 ? "{" + i + "} " 
                                                                                                                       : i == 7 ? "{" + i + "} " 
                                                                                                                                : "{" + i + "}").ToArray());
            return string.Format(format, str.ToArray()
                                            .Select(c => new string(new char[] { c } ))
                                                        .ToArray<object>());
        }

        public string parseBack(string str) {
            return str.Contains("(") || str.Contains(")") ? new string(str.ToArray()
                                                                          .Where(c => Regex.IsMatch(c.ToString(), @"^\d$", RegexOptions.IgnoreCase))
                                                                                    .ToArray()) 
                                                          : str;
        }
    }
}
