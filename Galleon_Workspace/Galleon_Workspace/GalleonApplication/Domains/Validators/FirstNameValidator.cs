using System;
using System.Collections.Generic;
using GalleonApplication.Extra;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;

namespace GalleonApplication.Domains.Validators {

    public class FirstNameValidator : IValidationRule {

        private const string NAME_REGEX = @"[\w]{3,}$";

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
            return invoke<bool>(() => {
                return Regex.IsMatch((value ?? "").ToString(), NAME_REGEX, RegexOptions.IgnoreCase);
            }) ? ValidationResult.ValidResult
               : new ValidationResult(false, Properties.Resources.InvalidCustomerNameText); 
        }
    }
}
