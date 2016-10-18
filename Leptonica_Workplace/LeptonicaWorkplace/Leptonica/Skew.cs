using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leptonica.Interop;
using System.Threading.Tasks;

namespace Leptonica {

    public class Skew {

        public const int DefaultBinarySearchReduction = 2;
        public const int DefaultBinaryThreshold = 130;

        public static Pix Deskew(Pix pix) {
            Scew s;
            return Deskew(pix, DefaultBinarySearchReduction, out s);
        }

        public static Pix Deskew(Pix pix, out Scew scew) {
            return Deskew(pix, DefaultBinarySearchReduction, out scew);
        }

        public static Pix Deskew(Pix pix, int redSearch, out Scew scew) {
            return Deskew(pix, Sweep.Default, redSearch, DefaultBinaryThreshold, out scew);
        }

        public static Pix Deskew(Pix pix, Sweep sweep, int redSearch, int threshold, out Scew scew) {
            float angle, confidence;
            IntPtr p = LeptonicaNativeApi.Native.pixDeskewGeneral(pix.Reference, sweep.Reduction, sweep.Range, sweep.Delta, redSearch, threshold, out angle, out confidence);
            if(p == IntPtr.Zero) {
                throw new NullReferenceException("failed to deskew.");
            }
            scew = new Scew() {
                Angle = angle,
                Confidence = confidence
            };
            return Pix.Create(p);
        }

        public static Pix FindSkewAndDeskew(Pix pix, int redSearch) {
            float pangle, pconf;
            IntPtr ptr = LeptonicaNativeApi.Native.pixFindSkewAndDeskew(pix.Reference, redSearch, out pangle, out pconf);
            if(ptr == IntPtr.Zero) {
                throw new NullReferenceException("failed to findSkewAndDeskew");
            }
            return Pix.Create(ptr);
        }

        public static Pix FindSkewAndDeskew(Pix pix) {
            return FindSkewAndDeskew(pix, 4);
        }
    }

    public class Sweep {

        public static Sweep Default = new Sweep(DefaultReduction, DefaultRange, DefaultDelta);

        public const int DefaultReduction = 4; 
        public const float DefaultRange = 7.0F;
        public const float DefaultDelta = 1.0F;

        public Sweep(int reduction = DefaultReduction, float range = DefaultRange, float delta = DefaultDelta) {
            this.Reduction = reduction;
            this.Range = range;
            this.Delta = delta;
        }

        public int Reduction { get; set; }
        public float Range { get; set; }
        public float Delta { get; set; }
    }

    public class Scew {
        
        public float Angle { get; set; }
        public float Confidence { get; set; }
    }
}
