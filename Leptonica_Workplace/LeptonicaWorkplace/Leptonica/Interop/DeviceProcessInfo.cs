using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leptonica.Interop {
    
    public static class DeviceProcessInfo {

        public static readonly bool Is64Bit;

        static DeviceProcessInfo() {
            Is64Bit = IntPtr.Size == sizeof(int);
        }
    }
}
