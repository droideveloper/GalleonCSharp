using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leptonica.Interop;
using System.Threading.Tasks;

namespace Leptonica {

    public class Scale {

        public static Pix Flake(Pix pix, float x, float y) {
            IntPtr p = LeptonicaNativeApi.Native.pixScale(pix.Reference, x, y);
            if(p == IntPtr.Zero) {
                throw new NullReferenceException("failed to scale.");
            }
            return Pix.Create(p);
        }
    }
}
