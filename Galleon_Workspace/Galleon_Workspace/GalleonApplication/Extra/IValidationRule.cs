using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace GalleonApplication.Extra {
    public abstract class IValidationRule : ValidationRule {

        protected T invoke<T>(Func<T> func) { return func(); }
    }
}
