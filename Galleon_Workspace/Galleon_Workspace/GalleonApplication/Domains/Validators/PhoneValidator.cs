using GalleonApplication.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GalleonApplication.Domains.Validators {
    
    public class PhoneValidator : IValidationRule {

        private const string REGEX_PHONE = @"\(?([0-9]{3})\)?([ .-]?)([0-9]{3})\2([0-9]{2})?([ .-]?)([0-9]{2})$";

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
            return invoke<bool>(() => {
                return (value ?? "").ToString().Length >= 10 ? Regex.IsMatch((value ?? "").ToString(), REGEX_PHONE) : (value ?? "").ToString().Length == 10;
            }) ? ValidationResult.ValidResult
               : new ValidationResult(false, Properties.Resources.InvalidPhoneText);
        }
    }
}
