using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leptonica.Interop;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Leptonica.Interop {

    public unsafe sealed class Deskew : AbstractDisposeable {

        private HandleRef reference;
        private string filename;

        private Deskew(IntPtr ptr, string filename) {
            if(ptr == IntPtr.Zero) {
                throw new NullReferenceException("failed to create ptr in native");
            }
            this.reference = new HandleRef(this, ptr);
            this.filename = filename;
        }

        public static Deskew Create(string filename) {
            if(filename.IsNullOrEmpty()) {
                throw new ArgumentNullException("filename is null");
            }
            IntPtr ptr = DeskewNativeApi.Native.CreateDeskew(filename);
            return new Deskew(ptr, filename);
        }
       

        public void SaveDeskewed(string filename, ImageFormat format) {
            float angle = GetSkewAngle();
            Bitmap bitmap = new Bitmap(this.filename);
            bitmap = RotateDeskew(bitmap, angle);
            bitmap.Save(filename, format);
        }

        public static Bitmap RotateDeskew(Bitmap img, float angle) {
            Bitmap bitmap = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppRgb);
            bitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
            Graphics graph = Graphics.FromImage(bitmap);
            try {
                graph.FillRectangle(new SolidBrush(Color.FromArgb(0xD8, 0xD8, 0xD8)), 0, 0, img.Width, img.Height);
                graph.RotateTransform(angle);
                graph.DrawImage(img, 0, 0);
            } finally {
                graph.Dispose();
            }
            return bitmap;
        }

        public float GetSkewAngle() {
            double angle = DeskewNativeApi.Native.GetSkewAngle(this.reference);
            return (float) -angle;
        }

        protected override void Dispose(bool isDisposing) {
            DeskewNativeApi.Native.DestroyDeskew(this.reference);
            this.reference = new HandleRef(this, IntPtr.Zero);
        }
    }
}
