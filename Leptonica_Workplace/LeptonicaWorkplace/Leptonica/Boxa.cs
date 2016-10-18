using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Leptonica.Interop;
using System.Threading.Tasks;

namespace Leptonica {
    public sealed class Boxa : AbstractDisposeable {

        private HandleRef reference;

        private Boxa(IntPtr ptr) {
            if(ptr == IntPtr.Zero) {
                throw new ArgumentNullException("ptr is zero");
            }
            this.reference = new HandleRef(this, ptr);
        }

        public HandleRef Reference {
            get { 
                return reference; 
            }
        }

        public static Boxa Create(IntPtr ptr) {
            return new Boxa(ptr);
        }

        protected override void Dispose(bool isDisposing) {
            IntPtr ptr = reference.Handle;
            LeptonicaNativeApi.Native.boxaDestroy(ref ptr);
            reference = new HandleRef(this, IntPtr.Zero);
        }
    }

    public enum L_SORT_BY : int { 
        X = 1,
        Y = 2,
        RIGHT = 3,
        BOT = 4,
        WIDTH = 5,
        HEIGHT = 6,
        MIN_DIMENSION = 7,
        MAX_DIMENSION = 8,
        PERIMETER = 9,
        AREA = 10,
        RATIO = 11
    }

    public enum L_SORT : int {
        INCRESING = 1,
        DECREASING = 2
    }

    public enum L_ACCESS_FLAG : int {
        L_INSERT = 0,
        L_COPY = 1,
        L_CLONE = 2,
        L_COPY_CLONE = 3
    }

    public enum L_TOPHAT : int { 
        WHITE = 0,
        BLACK = 1
    }

    public enum L_SELECT : int { 
        WIDTH = 1,
        HEIGHT = 2,
        IF_EITHER = 3,
        IF_BOTH = 4
    }

    public enum L_SELECT_CONDITON : int { 
        IF_LT = 1,
        IF_GT = 2,
        IF_LTE = 3,
        IF_GTE = 4
    }
}
