using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leptonica.Interop;
using System.Threading.Tasks;

namespace Leptonica {
        
    public class Transform {

        private const int DefaultHorizontalShift = 1;
        private const int DefaultVerticalShift = 1;

        public static Pix Translate(Pix pix, int horizontalShift, int verticalShift, ColorOptions options) {
            IntPtr ptr = LeptonicaNativeApi.Native.pixTranslate(IntPtr.Zero, pix.Reference, horizontalShift, verticalShift, options);
            if(ptr == IntPtr.Zero) {
                throw new NullReferenceException("failed to translate");
            }
            return Pix.Create(ptr);
        }

        public static Pix Translate(Pix pix) {
            return Translate(pix, ColorOptions.BringInWhite);
        }

        public static Pix Translate(Pix pix, ColorOptions options) {
            return Translate(pix, DefaultHorizontalShift, DefaultVerticalShift, options);
        }
    }

    public enum ColorOptions : int { 
        BringInWhite = 1,
        BringInBlack = 2
    }
}
