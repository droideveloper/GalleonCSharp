using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MuPDFLibrary.Interop;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace MuPDFLibrary {

    public sealed class MuPDF  {

        public static Bitmap ExtractPage(IPDFSource source, int pageNumber, float zoomFactor = 1.0f, string password = null) {
            int pageNumberIndex = Math.Max(0, pageNumber - 1);
            using(PDFFileStream stream = new PDFFileStream(source)) {
                ValidatePassword(stream.Document, password);
                IntPtr ptr = IntPtr.Zero;
                try {
                    ptr = MuPDFNativeApi.Native.LoadPage(stream.Document, pageNumberIndex);
                    return RenderPage(stream.Context, stream.Document, ptr, zoomFactor);
                } finally {
                    if(ptr != IntPtr.Zero) {
                        MuPDFNativeApi.Native.FreePage(stream.Document, ptr);
                    }
                }
            }
        }

        public static System.Windows.Size[] GetPageBounds(IPDFSource source, ImageRotation rotation = ImageRotation.NONE, string password = null) {
            using(PDFFileStream stream = new PDFFileStream(source)) {
                ValidatePassword(stream.Document, password);
                int pageSize = MuPDFNativeApi.Native.CountPages(stream.Document);
                return Enumerable.Range(0, pageSize - 1)
                                 .Select(x => {
                                     IntPtr ptr = IntPtr.Zero;
                                     try {
                                         ptr = MuPDFNativeApi.Native.LoadPage(stream.Document, x);
                                         MuPDFLibrary.Interop.Rectangle bound = MuPDFNativeApi.Native.BoundPage(stream.Document, ptr);
                                         return SizeCallback(rotation)(bound.Width, bound.Height);
                                     } finally {
                                         if(ptr != IntPtr.Zero) {
                                             MuPDFNativeApi.Native.FreePage(stream.Document, ptr);
                                         }
                                     }
                                 }).ToArray();  
            }
        }

        public static int CountPages(IPDFSource source, string password = null) {
            using(PDFFileStream stream = new PDFFileStream(source)) {
                ValidatePassword(stream.Document, password);
                return MuPDFNativeApi.Native.CountPages(stream.Document);
            }
        }

        public static bool NeedsPassword(IPDFSource source) {
            using(PDFFileStream stream = new PDFFileStream(source)) {
                return NeedsPassword(stream.Document);
            }
        }

        public static bool NeedsPassword(IntPtr document) {
            return MuPDFNativeApi.Native.NeedsPassword(document) != 0;
        }

        public static void ValidatePassword(IntPtr document, string password) {
            if(NeedsPassword(document) && MuPDFNativeApi.Native.AuthenticatePassword(document, password) == 0) {
                throw new ArgumentNullException("Password is invalid or missing, please control value you pass or provide one.");
            }
        }

        public static Bitmap RenderPage(IntPtr context, IntPtr document, IntPtr page, float zoomFactor) {   
            MuPDFLibrary.Interop.Rectangle bound = MuPDFNativeApi.Native.BoundPage(document, page);
            Matrix matrix = new Matrix();
            IntPtr pix = IntPtr.Zero;
            IntPtr device = IntPtr.Zero;

            Dpi dpi = SystemResolutionHelper.GetCurrentDpi();
            float zoomX = zoomFactor * (dpi.HDpi / SystemResolutionHelper.DEFAULT_DPI);
            float zoomY = zoomFactor * (dpi.VDpi / SystemResolutionHelper.DEFAULT_DPI);

            int width = (int) (zoomX * bound.Width);
            int height = (int) (zoomY * bound.Height);

            matrix.A = zoomX;
            matrix.D = zoomY;

            pix = MuPDFNativeApi.Native.NewPixmap(context, MuPDFNativeApi.Native.FindDeviceColorSpace(context, "DeviceRGB"), width, height);
            MuPDFNativeApi.Native.ClearPixmap(context, pix, 0xFF);

            device = MuPDFNativeApi.Native.NewDrawDevice(context, pix);
            MuPDFNativeApi.Native.RunPage(document, page, device, matrix, IntPtr.Zero);
            MuPDFNativeApi.Native.FreeDevice(device);
            device = IntPtr.Zero;

            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            unsafe {
                byte* ptrSrc = (byte*) MuPDFNativeApi.Native.GetSamples(context, pix);
                byte* ptrDest = (byte*) bitmapData.Scan0;
                for(int y = 0; y < height; y++) {
                    byte* pl = ptrDest;
                    byte* sl = ptrSrc;
                    for(int x = 0; x < width; x++) {
                        pl[2] = sl[0];
                        pl[1] = sl[1];
                        pl[0] = sl[2];
                        pl += 3;
                        sl += 4;
                    }
                    ptrDest += bitmapData.Stride;
                    ptrSrc += 4 * width;
                }
            }
            bitmap.UnlockBits(bitmapData);

            MuPDFNativeApi.Native.DropPixmap(context, pix);
            bitmap.SetResolution(dpi.HDpi, dpi.VDpi);
            return bitmap;            
        }

        private static Func<double, double, System.Windows.Size> SizeCallback(ImageRotation rotation) {
            if(rotation == ImageRotation.ROTATE_90 || rotation == ImageRotation.ROTATE_270) {
                return (w, h) => new System.Windows.Size(h, w);
            }
            return (w, h) => new System.Windows.Size(h, w);
        }
    }

    public enum ImageRotation { 
        NONE,
        ROTATE_90,
        ROTATE_180,
        ROTATE_270
    }

    public enum ViewType { 
        SINGLE_PAGE,
        FACING,
        BOOK
    }

    public enum ZoomType { 
        FIXED,
        FIT_WIDTH,
        FIT_HEIGHT
    }

    public enum PageDisplayType { 
        SINGLE,
        CONTINOUS
    }
}
