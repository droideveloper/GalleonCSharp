using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leptonica.Interop;
using System.Threading.Tasks;

namespace Leptonica {

    public class Binarize {

        private const int DefaultSX = 32;
        private const int DefaultSY = 32;
        private const int DefaultSmoothX = 2;
        private const int DefaultSmoothY = 2;
        private const float DefaultFraction = 0.1F;

        private const int DefaultWindowHalfWidth = 8;
        private const float DefaultReductionFactor = 0.34F;
        private const int DefaultTileX = 1;
        private const int DefaultTileY = 1;

        public static Pix OtsuAdaptiveThreshold(Pix pix) {
            return OtsuAdaptiveThreshold(pix, DefaultSX, DefaultSY, DefaultSmoothX, DefaultSmoothY, DefaultFraction);
        }

        public static Pix OtsuAdaptiveThreshold(Pix pix, int sx, int sy, int smoothx, int smoothy, float fraction) {
            if(pix.Depth != 8) {
                if(pix.Depth == 1) {
                    pix = Convert.Convert1To8(pix);
                } else {
                    pix = Convert.ConvertTo8(pix);
                }
            }
            (sx >= 16).VerifyCondition("sx must be at least 16 or greater.");
            (sy >= 16).VerifyCondition("sy must be at least 16 or greater.");
            IntPtr ppxith, ppixd;
            int result = LeptonicaNativeApi.Native.pixOtsuAdaptiveThreshold(pix.Reference, sx, sy, smoothx, smoothy, fraction, out ppxith, out ppixd);
            if(ppxith != IntPtr.Zero) {
                LeptonicaNativeApi.Native.pixDestroy(ref ppxith);
            }
            if(result == 1) {
                throw new NullReferenceException("failed to binarize.");
            }
            return Pix.Create(ppixd);
        }

        public static Pix SauvolaTiled(Pix pix) {
            return SauvolaTiled(pix, DefaultWindowHalfWidth, DefaultReductionFactor, DefaultTileX, DefaultTileY);
        }

        public static Pix Sauvola(Pix pix) {
            return Sauvola(pix, DefaultWindowHalfWidth, DefaultReductionFactor, false);
        }

        public static Pix Sauvola(Pix pix, int wsize, float factor, bool addBorder) {
            if(pix.Depth != 8) {
                if(pix.Depth == 1) {
                    pix = Convert.Convert1To8(pix);
                } else {
                    pix = Convert.ConvertTo8(pix);
                }
            }
            (wsize >= 2).VerifyCondition("wsize must be greater then 2");
            int max = Math.Min((pix.Width - 3) / 2, (pix.Height - 3) / 2);
            (wsize < max).VerifyCondition("wsize must be lower than value {0}", max);
            (factor >= 0).VerifyCondition("factor must be greater than 0");
            IntPtr a, b, c, d;
            int result = LeptonicaNativeApi.Native.pixSauvolaBinarize(pix.Reference, wsize, factor, addBorder ? 1 : 0, out a, out b, out c, out d);
            if(a != IntPtr.Zero) {
                LeptonicaNativeApi.Native.pixDestroy(ref a);
            }
            if(b != IntPtr.Zero) {
                LeptonicaNativeApi.Native.pixDestroy(ref b);
            }
            if(c != IntPtr.Zero) {
                LeptonicaNativeApi.Native.pixDestroy(ref c);
            }
            if(result == 1) {
                throw new NullReferenceException("failed to binarize.");
            }
            return Pix.Create(d);
        }

        public static Pix SauvolaTiled(Pix pix, int wsize, float factor, int nx, int ny) {
            if(pix.Depth != 8) {
                if(pix.Depth == 1) {
                    pix = Convert.Convert1To8(pix);
                } else {
                    pix = Convert.ConvertTo8(pix);
                }
            }
            (wsize >= 2).VerifyCondition("wsize must be greater then 2");
            int max = Math.Min((pix.Width - 3) / 2, (pix.Height - 3) / 2);
            (wsize < max).VerifyCondition("wsize must be lower than value {0}", max);
            (factor >= 0).VerifyCondition("factor must be greater than 0");
            IntPtr a, b;
            int result = LeptonicaNativeApi.Native.pixSauvolaBinarizeTiled(pix.Reference, wsize, factor, nx, ny, out a, out b);
            if(a != IntPtr.Zero) {
                LeptonicaNativeApi.Native.pixDestroy(ref a);
            }
            if(result == 1) {
                throw new NullReferenceException("failed to binarize.");
            }
            return Pix.Create(b);
        }
    }
}
