using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leptonica.Interop;
using System.Threading.Tasks;

namespace Leptonica {
    
    public class Convolve {

        public static Pix Blockconvolve(Pix pix, int sx, int sy) {
            IntPtr ptr = LeptonicaNativeApi.Native.pixBlockconv(pix.Reference, sx, sy);
            if(ptr == IntPtr.Zero) {
                throw new NullReferenceException("failed to Blockconvolve");
            }
            return Pix.Create(ptr);
        }

        public static Pix Blockconvolve(Pix pix) {
            return Blockconvolve(pix, 15, 15);
        }
    }
}
