using GalleonApplication.Extra;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GalleonApplication.Domains.Validators {

    public class QueryValidator : IValidationRule  {
    
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            return invoke<bool>(() => {
                return Regex.IsMatch((value ?? "").ToString(), @"([a-zA-Z0-9]{3,}[,\s]*)$", RegexOptions.IgnoreCase);
            }) ? ValidationResult.ValidResult : new ValidationResult(false, "");
        }
    }
}
