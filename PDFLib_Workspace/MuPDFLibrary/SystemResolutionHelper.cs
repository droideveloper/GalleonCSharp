using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MuPDFLibrary {
    
    internal static class SystemResolutionHelper {

        public const float DEFAULT_DPI = 96;

        private const string GDI_DLL = "gdi32.dll";


        public static Dpi GetCurrentDpi() {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();

            return new Dpi() {
                HDpi = GetDeviceCaps(desktop, (int) DeviceCap.LOGPIXELSX),
                VDpi = GetDeviceCaps(desktop, (int) DeviceCap.LOGPIXELSY)
            };
        }

        [DllImport(GDI_DLL, CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int GetDeviceCaps(IntPtr hDC, int nIndex);
    }

    public enum DeviceCap : int {         
        LOGPIXELSX = 88,
        LOGPIXELSY = 90
    }

    public sealed class Dpi {
        public float HDpi { get; set; }
        public float VDpi { get; set; }
    }
}
