using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Leptonica.Interop;

namespace Leptonica {
    
    public unsafe sealed class Box : AbstractDisposeable {

        private int x;
        private int y;
        private int w;
        private int h;
        private HandleRef handle;

        private Box(IntPtr ptr) {
            if(ptr == IntPtr.Zero) {
                throw new ArgumentNullException("ptr is zero");
            }

            this.handle = new HandleRef(this, ptr);
            int result = LeptonicaNativeApi.Native.boxGetGeometry(this.handle, out x, out y, out w, out h);
            if(result == 1) {
                throw new NullReferenceException("failed to retrieve box geometry");
            }
        }

        public static Box Create(int x, int y, int width, int height) {
            (x >= 0).VerifyCondition("x should be non negative");
            (y >= 0).VerifyCondition("y should be not negative");

            IntPtr ptr = LeptonicaNativeApi.Native.boxCreate(x, y, width, height);
            return new Box(ptr);
        }

        public static Box Create(IntPtr boxPtr) {
            return new Box(boxPtr);
        }

        public int X {
            get {
                return x;
            }
        }

        public int Y {
            get {
                return y;
            }
        }

        public int Width {
            get {
                return w;
            }
        }

        public int Height {
            get {
                return h;
            }
        }

        public HandleRef Reference {
            get {
                return handle;
            }
        }

        public Box Clone() {
            IntPtr ptr = LeptonicaNativeApi.Native.boxClone(handle);
            return new Box(ptr);
        }
        
        protected override void Dispose(bool isDisposing) {
            IntPtr ptr = handle.Handle;
            LeptonicaNativeApi.Native.boxDestroy(ref ptr);
            this.handle = new HandleRef(this, IntPtr.Zero);
        }
    }
}
