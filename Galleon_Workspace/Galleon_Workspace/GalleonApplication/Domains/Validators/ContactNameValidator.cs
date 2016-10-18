using GalleonApplication.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GalleonApplication.Domains.Validators {
    public class ContactNameValidator : IValidationRule {

        private const string REGEX_CONTACT_NAME = @"[\w]{2,}$";

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
            return invoke<bool>(() => {
                return Regex.IsMatch((value ?? "").ToString(), REGEX_CONTACT_NAME, RegexOptions.IgnoreCase);
            }) ? ValidationResult.ValidResult
               : new ValidationResult(false, Properties.Resources.InvalidContactNameText);
        }
    }
}
