using Leptonica.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Leptonica {
    public sealed class Pixa : AbstractDisposeable {

        private HandleRef reference;

        public HandleRef Reference {
            get {
                return this.reference;
            }
        }

        private Pixa(IntPtr ptr) {
            if(ptr == IntPtr.Zero) {
                throw new ArgumentNullException("ptr is zero, no memeory address");
            }
            this.reference = new HandleRef(this, ptr);
        }

        public static Pixa Create(IntPtr ptr) {
            return new Pixa(ptr);
        }

        protected override void Dispose(bool isDisposing) {
            IntPtr ptr = this.reference.Handle;
            LeptonicaNativeApi.Native.pixaDestroy(ref ptr);
            this.reference = new HandleRef(this, IntPtr.Zero);
        }
    }
}
