using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leptonica.Interop;
using System.Threading.Tasks;

namespace Leptonica {
    
    public class Clip {

        public static Pix ClipToRectangle(Pix pix, Box box) {
            IntPtr ptr = LeptonicaNativeApi.Native.pixClipRectangle(pix.Reference, box.Reference, IntPtr.Zero);
            if(ptr == IntPtr.Zero) {
                throw new NullReferenceException("failed to crop");
            }
            return Pix.Create(ptr);
        }
    }
}
