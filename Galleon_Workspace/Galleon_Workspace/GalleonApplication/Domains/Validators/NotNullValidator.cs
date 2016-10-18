using GalleonApplication.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GalleonApplication.Domains.Validators {

    public class NotNullValidator : IValidationRule {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
            return invoke<bool>(() => {
                return !value.IsNullOrEmpty();
            }) ? ValidationResult.ValidResult
               : new ValidationResult(false, Properties.Resources.InvalidOptionText);
        }
    }
}
