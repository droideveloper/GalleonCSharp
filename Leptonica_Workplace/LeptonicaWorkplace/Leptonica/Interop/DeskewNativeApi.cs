using Leptonica.InteropNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Leptonica.Interop {

    public interface IDeskewNativeApi {

        [RuntimeDllImport(DeskewNativeConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = DeskewNativeConstants.MethodCreateDeskew)]
        IntPtr CreateDeskew(string filename);

        [RuntimeDllImport(DeskewNativeConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = DeskewNativeConstants.MethodGetSkewAngle)]
        double GetSkewAngle(HandleRef ptr);

        [RuntimeDllImport(DeskewNativeConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = DeskewNativeConstants.MethodDestoryDeskew)]
        void DestroyDeskew(HandleRef ptr);
    }

    public class DeskewNativeConstants {
        public const string DLLName = "libdeskew";

        public const string MethodCreateDeskew  = "CreateDeskew";
        public const string MethodGetSkewAngle  = "GetSkewAngle";
        public const string MethodDestoryDeskew = "DestroDeskew";
    }

    unsafe public static class DeskewNativeApi {

        private static IDeskewNativeApi native;

        public static IDeskewNativeApi Native {
            get {
                if(native == null) {
                    Initialize();
                }
                return native;
            }
        }

        private static void Initialize() {
            if(native == null) {
                native = InteropRuntimeImplementer.CreateInstance<IDeskewNativeApi>();
            }
        }
    }
}
