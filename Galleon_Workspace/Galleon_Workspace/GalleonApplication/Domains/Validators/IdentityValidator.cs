using GalleonApplication.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GalleonApplication.Domains.Validators {
    
    public class IdentityValidator : IValidationRule {

        private const string REGEX_IDENTITY = @"^\d{11}$";

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
            return invoke<bool>(() => {
                return Regex.IsMatch((value ?? "").ToString(), REGEX_IDENTITY);
            }) ? ValidationResult.ValidResult
               : new ValidationResult(false, Properties.Resources.InvalidIdentityText);
        }
    }
}
