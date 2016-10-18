using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leptonica.Interop;
using System.Threading.Tasks;

namespace Leptonica {
    
    public class Enhance {

        public const int DefaultHalfWidth   = 1;
        public const float DefaultFraction  = 0.3f;

        public static Pix UnsharpMasking(Pix pix) {
            IntPtr p = LeptonicaNativeApi.Native.pixUnsharpMasking(pix.Reference, DefaultHalfWidth, DefaultFraction);
            if(p == IntPtr.Zero) {
                throw new NullReferenceException("failed to unsharpMasking");
            }
            return Pix.Create(p);
        }

        public static Pix UnsharpMasking(Pix pix, int halfwidth, float fract) {
            (fract > 0.2f).VerifyCondition("fraction must be greater than 0.2");
            (fract < 0.7f).VerifyCondition("fraction must be lower than 0.7");
            IntPtr p = LeptonicaNativeApi.Native.pixUnsharpMasking(pix.Reference, halfwidth, fract);
            if(p == IntPtr.Zero) {
                throw new NullReferenceException("failed to unsharpMasking");
            }
            return Pix.Create(p);
        }
    }
}
