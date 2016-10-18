using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leptonica.Interop;
using System.Threading.Tasks;

namespace Leptonica {

    public class Dewrap {

        public static Pix DewrapSinglePage(Pix pix, DewrapThresh thresh, DewrapThresholding adaptive, DewrapOrientation orinetation, DewrapOptions options) {
            IntPtr ptr;
            int result = LeptonicaNativeApi.Native.dewarpSinglePage(pix.Reference, thresh, adaptive, orinetation, out ptr, IntPtr.Zero, options);
            if(result == 1) {
                throw new NullReferenceException("failed to dewrap");
            }
            return Pix.Create(ptr);
        }

        public static Pix DewrapSinglePage(Pix pix) {
            return DewrapSinglePage(pix, DewrapThresh.Enabled, DewrapThresholding.Adaptive, DewrapOrientation.VerticalAndHorizontal, DewrapOptions.Release);
        }
    }

    public enum DewrapThresh : int { 
        Enabled = 0,
        Disabled = 1
    }

    public enum DewrapOrientation : int {
        Vertical = 0,
        VerticalAndHorizontal = 1
    }

    public enum DewrapThresholding : int { 
        Adaptive = 1,
        Global = 0
    }

    public enum DewrapOptions : int { 
        Debug = 1,
        Release = 0
    }
}
