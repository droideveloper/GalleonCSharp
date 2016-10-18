using Leptonica.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leptonica {
    public static class Extensions {

        public static string ToFormatedString(this object value, string formatStr) {
            return string.Format(formatStr, value);
        }

        public static void ThrowIfNullOrEmpty(this object value) {
            if(value.IsNullOrEmpty()) {
                throw new ArgumentNullException(value.ToFormatedString("Argument {0} must not be null or empty."));
            }
        }

        public static bool IsNullOrEmpty(this object value) {
            return value == null || (value is string || value is String) ? string.IsNullOrEmpty((value as string)) : false; 
        }

        public static void VerifyCondition(this bool isValid, string message, params object[] args) {
            if(!isValid) {
                throw new InvalidOperationException(string.Format(message, args));
            }
        }

        public static float DegreeToRadians(this float degree) {
            return (float)Math.PI / 180.0f * degree;
        }

        public static bool IsRotationConfident(double confident) {
            return confident >= 2.0 && confident <= 3.0;
        }

        /*pix extensions*/
        public static Pix PixBackgroundNormSimple(this Pix pix) {
            if(pix == null) {
                throw new ArgumentNullException("pix is null");
            }
            IntPtr pixPtr = LeptonicaNativeApi.Native.pixBackgroundNormSimple(pix.Reference, IntPtr.Zero, IntPtr.Zero);
            return Pix.Create(pixPtr);
        }

        public static Pix PixConvertRGBToGray(this Pix pix, float r = 0.5f, float g = 0.3f, float b = 0.2f) {
            if(pix == null) {
                throw new ArgumentNullException("pix is null");
            }
            IntPtr pixPtr = LeptonicaNativeApi.Native.pixConvertRGBToGray(pix.Reference, r, g, b);
            return Pix.Create(pixPtr);
        }

        public static Pix PixRotate(this Pix pix, float angle, float confidence, RotationMethod method = RotationMethod.Shear, RotationFill fill = RotationFill.White) {
            if(pix == null) {
                throw new ArgumentNullException("pix is null");
            }
            if(IsRotationConfident(confidence)) {
                IntPtr pixPtr = LeptonicaNativeApi.Native.pixRotate(pix.Reference, angle.DegreeToRadians(), method, fill, pix.Width, pix.Height);
                return Pix.Create(pixPtr);
            }
            return pix;
        }

        public static Pix PixFindSkewAndDeskew(this Pix pix, int windowSize = 2) {
            if(pix == null) {
                throw new ArgumentNullException("pix is null");
            }
            float angle, confidence;
            IntPtr pixPtr = LeptonicaNativeApi.Native.pixFindSkewAndDeskew(pix.Reference, windowSize, out angle, out confidence);
            return Pix.Create(pixPtr).PixRotate(angle, confidence);
        }

        public static Pix PixTophat(this Pix pix, int w = 17, int h = 17, L_TOPHAT options = L_TOPHAT.BLACK) {
            if(pix == null) { 
                throw new ArgumentNullException("pix is null");
            }
            IntPtr pixPtr = LeptonicaNativeApi.Native.pixTophat(pix.Reference, h, w, options);
            return Pix.Create(pixPtr);
        }

        public static Pix PixInvert(this Pix pix) {
            if(pix == null) {
                throw new ArgumentNullException("pix is null");
            }
            LeptonicaNativeApi.Native.pixInvert(pix.Reference, pix.Reference);
            return pix;
        }

        public static Pix PixGammaRTC(this Pix pix, float gamma = 1.0f, int blackColor = 170, int whiteColor = 245) {
            if(pix == null) {
                throw new ArgumentNullException("pix is null");
            }
            LeptonicaNativeApi.Native.pixGammaTRC(pix.Reference, pix.Reference, gamma, blackColor, whiteColor);
            return pix;
        }

        public static Pix PixThresholdToBinary(this Pix pix, int whiteColor = 35) {
            if(pix == null) {
                throw new ArgumentNullException("pix is null");
            }
            IntPtr pixPtr = LeptonicaNativeApi.Native.pixThresholdToBinary(pix.Reference, whiteColor);
            return Pix.Create(pixPtr);
        }

        //file info
        public static FileInfo ToNewExtension(this FileInfo f, string newExtension) {
            if(f == null || newExtension.IsNullOrEmpty() || !newExtension.StartsWith(".")) {
                throw new ArgumentNullException("file or new extension is null");
            }
            return new FileInfo(f.FullName.Substring(0, f.FullName.IndexOf(f.Extension)) + newExtension);
        }
    }
}
