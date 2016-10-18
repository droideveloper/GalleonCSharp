using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leptonica.Interop;
using System.Threading.Tasks;

namespace Leptonica {

    public class Edge {

        public static Pix SobelEdgeFilter(Pix pix, EdgeDirection orientflag = EdgeDirection.All) {
            if(pix.Depth != 8) {
                pix = Convert.ConvertTo8(pix);
            }
            IntPtr p = LeptonicaNativeApi.Native.pixSobelEdgeFilter(pix.Reference, orientflag);
            if(p == IntPtr.Zero) {
                throw new NullReferenceException("failed to edge.");
            }
            return Pix.Create(p);
        }
    }
}
