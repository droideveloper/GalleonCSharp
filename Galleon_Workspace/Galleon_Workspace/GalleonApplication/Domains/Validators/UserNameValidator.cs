using GalleonApplication.Extra;
using GalleonApplication.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GalleonApplication.Domains.Validators {
    
    public class UserNameValidator : IValidationRule {

        private const string EMAIL_REGEX = @"^(([^<>()\[\]\\.,;:\s@']+(\.[^<>()\[\]\\.,;:\s@']+)*)|('.+'))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
            return invoke<bool>(() => {
                IPreferenceManager prefs = DependencyInjector.Get<PreferenceManager>();
                if(prefs.getValue(PreferenceManager.KEY_REMEMBER_ME, false)) {
                    return true;
                }
                return Regex.IsMatch((value ?? "").ToString(), EMAIL_REGEX, RegexOptions.IgnoreCase);
            }) ? ValidationResult.ValidResult
               : new ValidationResult(false, Properties.Resources.InvalidUserNameText);  
        }
    }
}
