using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leptonica.Interop;
using System.Threading.Tasks;

namespace Leptonica {

    public class Rotate {

        public const float TinyAngle = 0.001F;

        public static Pix Pivot(Pix pix, float angle, RotationMethod method = RotationMethod.AreaMap, RotationFill fillColor = RotationFill.White, int? w = null, int? h = null) {
            w = w.IsNullOrEmpty() ? pix.Width : w;
            h = h.IsNullOrEmpty() ? pix.Height : h;
            if(Math.Abs(angle) < TinyAngle) {
                return pix.Clone();
            }
            IntPtr p;

            double rotations = 2 * angle / Math.PI;
            if(Math.Abs(rotations - Math.Floor(rotations)) < TinyAngle) {
                p = LeptonicaNativeApi.Native.pixRotateOrth(pix.Reference, (int) rotations);
            } else {
                p = LeptonicaNativeApi.Native.pixRotate(pix.Reference, angle, method, fillColor, w.Value, h.Value);
            }
            if(p == IntPtr.Zero) {
                throw new NullReferenceException("failed to rotate");
            }
            return Pix.Create(p);
        }

        public static Pix Pivot(Pix pix) {
            return Pivot(pix, 0.01f);
        }
    }
}
