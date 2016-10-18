using GalleonApplication.Extra;
using GalleonApplication.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GalleonApplication.Domains.Validators {

    public class PasswordValidator : IValidationRule {
        
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
            return invoke<bool>(() => {
                IPreferenceManager prefs = DependencyInjector.Get<PreferenceManager>();
                if(prefs.getValue(PreferenceManager.KEY_REMEMBER_ME, false)) {
                    return true;
                }
                return (value ?? "").ToString().Length >= 8;//min 8 chars password
            }) ? ValidationResult.ValidResult
               : new ValidationResult(false, Properties.Resources.InvalidPasswordText); 
        }
    }
}
