using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leptonica.Interop;
using System.Threading.Tasks;

namespace Leptonica {
    
    public class Convert {

        private const int DefaultThreshold = 130;

        public static Pix ConvertRGBToGray(Pix pix, float rwt, float gwt, float bwt) {
            (pix.Depth == 32).VerifyCondition("image must be 32pp.");
            (rwt >= 0).VerifyCondition("red weight must be greater than 0");
            (gwt >= 0).VerifyCondition("green weight must be greater than 0");
            (bwt >= 0).VerifyCondition("blue weight must be greater than 0");

            IntPtr p = LeptonicaNativeApi.Native.pixConvertRGBToGray(pix.Reference, rwt, gwt, bwt);
            if(p == IntPtr.Zero) {
                throw new NullReferenceException("failed to convert gray scale.");
            }
            return Pix.Create(p);
        }

        public static Pix ConvertRGBToGray(Pix pix) {
            return ConvertRGBToGray(pix, 0f, 0f, 0f);
        } 

        public static Pix ConvertTo8(Pix pix, int flag) {            
            IntPtr p = LeptonicaNativeApi.Native.pixConvertTo8(pix.Reference, flag);
            if(p == IntPtr.Zero) {
                throw new NullReferenceException("failed to convert 8pp");
            }
            return Pix.Create(p);
        }

        public static Pix ConvertTo8(Pix pix) {
            return ConvertTo8(pix, 0);//false
        }

        public static Pix ConvertRGBToLuminance(Pix pix) {
            IntPtr ptr = LeptonicaNativeApi.Native.pixConvertRGBToLuminance(pix.Reference);
            if(ptr == IntPtr.Zero) {
                throw new NullReferenceException("failed to ConvertRGBToLuminance");
            }
            return Pix.Create(ptr);
        }

        public static Pix ConvertTo1(Pix pix, int threshold) {
            IntPtr ptr = LeptonicaNativeApi.Native.pixConvertTo1(pix.Reference, threshold);
            if(ptr == IntPtr.Zero) {
                throw new NullReferenceException("failed to ConvertTo1");
            }
            return Pix.Create(ptr);
        }

        public static Pix ConvertTo1(Pix pix) {
            return ConvertTo1(pix, DefaultThreshold);
        }

        public static Pix Convert1To8(Pix pix, int whiteColor, int blackColor) {
            IntPtr ptr = LeptonicaNativeApi.Native.pixConvert1To8(IntPtr.Zero, pix.Reference, whiteColor, blackColor);
            if(ptr == IntPtr.Zero) {
                throw new NullReferenceException("failed to Convert1To8");
            }
            return Pix.Create(ptr);
        }

        public static Pix Convert1To8(Pix pix) {
            return Convert1To8(pix, 255, 0);
        }
    }
}
