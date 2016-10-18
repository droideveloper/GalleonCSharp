using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra {
    
    public class ParameterArray {

        public ParameterArray(object[] parameters) {
            Parameters = parameters;
        } 

        public object[] Parameters { get; private set; }        
    }
}
