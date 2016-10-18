using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leptonica.Interop;
using System.Threading.Tasks;

namespace Leptonica {
    
    
    public class GrayMorph {

        public static Pix CloseGray(Pix pix, int sx, int sy) {
            IntPtr ptr = LeptonicaNativeApi.Native.pixCloseGray(pix.Reference, sx, sy);
            if(ptr == IntPtr.Zero) {
                throw new NullReferenceException("failed to CloseGray");
            }
            return Pix.Create(ptr);
        }

        public static Pix CloseGray(Pix pix) {
            return CloseGray(pix, 25, 25);
        }
    }
}
