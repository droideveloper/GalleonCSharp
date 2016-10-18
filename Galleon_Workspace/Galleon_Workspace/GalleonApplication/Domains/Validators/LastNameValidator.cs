using GalleonApplication.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GalleonApplication.Domains.Validators {

    public class LastNameValidator : IValidationRule {

        private const string SURNAME_REGEX = @"[\w]{2,}$";

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
            return invoke<bool>(() => {
                return Regex.IsMatch((value ?? "").ToString(), SURNAME_REGEX, RegexOptions.IgnoreCase);
            }) ? ValidationResult.ValidResult
               : new ValidationResult(false, Properties.Resources.InvalidCustomerSurnameText);
        }
    }
}
