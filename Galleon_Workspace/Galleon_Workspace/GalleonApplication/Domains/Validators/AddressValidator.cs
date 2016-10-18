using GalleonApplication.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GalleonApplication.Domains.Validators {
    
    public class AddressValidator : IValidationRule {

        private const string REGEX_ADDRESS = @"[\s\S]{2,}$";

        public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
            return invoke<bool>(() => {
                return Regex.IsMatch((value ?? "").ToString(), REGEX_ADDRESS, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            }) ? ValidationResult.ValidResult
               : new ValidationResult(false, Properties.Resources.InvalidAddressText);  
        }
    }
}
