using Leptonica.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Leptonica {
    
    public unsafe sealed class Pix : AbstractDisposeable {

        public static Pix NULL = new Pix(IntPtr.Zero, true);

        private static readonly List<int> AllowedDepths = new List<int>() { 1, 2, 4, 8, 16, 32 };

        private readonly int depth;
        private readonly int width;
        private readonly int height;
        private HandleRef handle;

        private Pix(IntPtr handle) {
            if(handle == IntPtr.Zero) {
                throw new ArgumentNullException("handle is null.");
            } else {

                this.handle = new HandleRef(this, handle);
                this.width = LeptonicaNativeApi.Native.pixGetWidth(this.handle);
                this.height = LeptonicaNativeApi.Native.pixGetHeight(this.handle);
                this.depth = LeptonicaNativeApi.Native.pixGetDepth(this.handle);
            }
        }

        private Pix(IntPtr ptr, bool zero) {
            if(zero) {
                this.handle = new HandleRef(this, ptr);
            } else {
                if(ptr == IntPtr.Zero) {
                    throw new ArgumentNullException("handle is null.");
                } else {

                    this.handle = new HandleRef(this, ptr);
                    this.width = LeptonicaNativeApi.Native.pixGetWidth(this.handle);
                    this.height = LeptonicaNativeApi.Native.pixGetHeight(this.handle);
                    this.depth = LeptonicaNativeApi.Native.pixGetDepth(this.handle);
                }
            }
        }

        public static Pix Create(int width, int height, int depth) {
            if(!AllowedDepths.Contains(depth)) {
                throw new ArgumentOutOfRangeException("Depth can only be 1, 2, 4, 8, 16, 32");
            }
            if(width <= 0 || height <= 0) {
                throw new ArgumentException("width or height must be greater than 0");
            }
            IntPtr pix = LeptonicaNativeApi.Native.pixCreate(width, height, depth);
            if(pix == IntPtr.Zero) {
                throw new OutOfMemoryException("most likely size of image too large.");
            }
            return new Pix(pix);
        }

        public static Pix Create(IntPtr handle) {
            return new Pix(handle);
        }

        public static Pix LoadFromFile(string filename) {
            IntPtr pix = LeptonicaNativeApi.Native.pixRead(filename);
            if(pix == IntPtr.Zero) {
                throw new IOException(string.Format("Failed to read file, {0}", filename));
            }
            return new Pix(pix);
        }

        public int Width {
            get {
                return width;
            }
        }

        public int Height {
            get {
                return height;
            }
        }

        public int Depth {
            get {
                return depth;
            }
        }

        public HandleRef Reference {
            get {
                return handle;
            }
        }

        public void Save(string filename, ImageSaveFormat format) {
            if(string.IsNullOrEmpty(filename)) {
                throw new ArgumentNullException("specify a file");
            }
            int result = LeptonicaNativeApi.Native.pixWrite(filename, handle, format);
            if(result != 0) {
                throw new IOException(string.Format("failed to write into '{0}'.", filename));
            }
        }

        public Pix Clone() {
            IntPtr pix = LeptonicaNativeApi.Native.pixClone(handle);
            return new Pix(pix);
        }
             
        protected override void Dispose(bool isDisposing) {
            IntPtr pix = handle.Handle;
            if(pix != IntPtr.Zero) {
                LeptonicaNativeApi.Native.pixDestroy(ref pix);
            }
            this.handle = new HandleRef(this, IntPtr.Zero);
        }
    }
}
