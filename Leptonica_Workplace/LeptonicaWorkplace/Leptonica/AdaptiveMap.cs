using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leptonica.Interop;
using System.Threading.Tasks;

namespace Leptonica {

    public class AdaptiveMap {

        private const int DefaultReduction = 16;
        private const int DefaultSize = 3;
        private const int DefaultBackgroundColor = 200;

        private const int DefaultTileWidth = 100;
        private const int DefaultTileHeight = 100;
        private const int DefaultDifference = 55;
        private const int DefaultSmoothX = 1;
        private const int DefaultSmoothY = 1;

        private const float DefaultGamma = 1.0f;
        private const int DefaultBlackColor = 70;
        private const int DefaultWhiteColor = 190;

        public static Pix BackgroundNormMorph(Pix pix, int reduction, int size, int backgroundColor) {
            if(pix.Depth != 8) {
                if(pix.Depth == 1) {
                    pix = Convert.Convert1To8(pix);
                } else {
                    pix = Convert.ConvertTo8(pix);
                }
            }
            IntPtr p = LeptonicaNativeApi.Native.pixBackgroundNormMorph(pix.Reference, IntPtr.Zero, reduction, size, backgroundColor);           
            if(p == IntPtr.Zero) {
                throw new NullReferenceException("failed to backgroundNormMorph");
            }
            return Pix.Create(p);
        }

        public static Pix BackgroundNormMorph(Pix pix) {
            return BackgroundNormMorph(pix, DefaultReduction, DefaultSize, DefaultBackgroundColor);
        }
        /*
        public static Pix ContrastNorm(Pix pix) {
            return ContrastNorm(pix, DefaultTileWidth, DefaultTileHeight, DefaultDifference, DefaultSmoothX, DefaultSmoothY);
        }

        public static Pix ContrastNorm(Pix pix, int sx, int sy, int diff, int smoothx, int smoothy) {
            if(pix.Depth != 8) {
                if(pix.Depth == 1) {
                    pix = Convert.Convert1To8(pix);
                } else {
                    pix = Convert.ConvertTo8(pix);
                }
            }
            IntPtr p = LeptonicaNativeApi.Native.pixContrastNorm(IntPtr.Zero, pix.Reference, sx, sy, diff, smoothx, smoothy);
            if(p == IntPtr.Zero) {
                throw new NullReferenceException("failed to contrastNorm");
            }
            return Pix.Create(p);
        }*/

        public static Pix NormalizeIllumination(Pix pix, int reduction, int size, int backgroundColor, int sx, int sy) {
            IntPtr pixr, pixg, pixb;
            LeptonicaNativeApi.Native.pixBackgroundNormRGBArraysMorph(pix.Reference, IntPtr.Zero, reduction, size, backgroundColor, out pixr, out pixg, out pixb);
            if(pixr != IntPtr.Zero && pixg != IntPtr.Zero && pixb != IntPtr.Zero) {
                IntPtr ptr = LeptonicaNativeApi.Native.pixApplyInvBackgroundRGBMap(pix.Reference, pixr, pixg, pixb, sx, sy);
                if(ptr == IntPtr.Zero) {
                    throw new NullReferenceException("failed to normalize illumunation");
                }
                LeptonicaNativeApi.Native.pixDestroy(ref pixr);
                LeptonicaNativeApi.Native.pixDestroy(ref pixg);
                LeptonicaNativeApi.Native.pixDestroy(ref pixb);
                return Pix.Create(ptr);
            }
            throw new NullReferenceException("failed to normalize illumunation");
        }

        public static Pix NormalizeIllumination(Pix pix) {
            return NormalizeIllumination(pix, DefaultReduction, DefaultSize, DefaultBackgroundColor, DefaultSmoothX, DefaultSmoothY);
        }

        public static Pix CleanBackgroundToWhite(Pix pix, float gamma, int blackColor, int whiteColor) {
            IntPtr ptr = LeptonicaNativeApi.Native.pixCleanBackgroundToWhite(pix.Reference, IntPtr.Zero, IntPtr.Zero, gamma, blackColor, whiteColor);
            if(ptr == IntPtr.Zero) {
                throw new NullReferenceException("failed to CleanBackgroundToWhite");
            }
            return Pix.Create(ptr);
        }

        public static Pix CleanBackgroundToWhite(Pix pix) {
            return CleanBackgroundToWhite(pix, DefaultGamma, DefaultBlackColor, DefaultWhiteColor);
        }
    }
}
