using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;

namespace MuPDFLibrary {

    internal static class Extensions {

        #region BitmapExtensions

        public static BitmapSource ToBitmapSource(this Bitmap bitmap) {
            Rectangle bound = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(bound, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            int sinkSize = bitmapData.Stride * bitmap.Height;
            WriteableBitmap writeBitmap = new WriteableBitmap(bitmap.Width, bitmap.Height, bitmap.HorizontalResolution, bitmap.VerticalResolution, PixelFormats.Bgr32, null);
            writeBitmap.WritePixels(new Int32Rect(0, 0, bitmap.Width, bitmap.Height), bitmapData.Scan0, sinkSize, bitmapData.Stride);
            bitmap.UnlockBits(bitmapData);
            return writeBitmap;
        }

        #endregion

        #region ListExtensions

        public static IEnumerable<T> Take<T>(this IList<T> list, int start, int lenght) {
            for(int i = start; i < Math.Min(list.Count, start + lenght); i++) {
                yield return list[i];
            }
        }

        #endregion

        #region PageHelperExtensions

        public static int GetPreviousPageIndex(this int index, ViewType type) {
            int subtract = type == ViewType.FACING ? (index % 2 == 0 ? 2 : 3) : type == ViewType.BOOK ? (index % 2 == 0 ? 3 : 2) : 1;
            return (Math.Max(0, index - subtract) == 0 && index == 0) ? -1 : Math.Max(0, index - subtract); 
        }

        public static int GetNextPageIndex(this int index, int totalSize, ViewType type) {
            int add = type == ViewType.FACING ? (index % 2 == 0 ? 2 : 1) : type == ViewType.BOOK ? (index == 0 || index % 2 == 0 ? 1 : 2) : 1;
            return (index + add >= totalSize) ? -1 : index + add;
        }

        public static int GetVisibleIndexFromPageIndex(this int index, ViewType type) {
            return type == ViewType.FACING ? ((index + (index % 2 == 0 ? 3 : 1)) / 2) - 1 : type == ViewType.BOOK && index > 0 ? (index + (index % 2 == 0 ? 0 : 1)) / 2 : index;
        }

        #endregion

        #region VisualTreeGelperExtensions

        public static T FindChild<T>(this DependencyObject o) where T : DependencyObject {
            return o is T ? (T) o : invoke<T>(() => {
                for(int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++) {
                    DependencyObject child = VisualTreeHelper.GetChild(o, i);
                    T result = child.FindChild<T>();
                    if(result != null) { return result; }
                }
                return null;
            });  
        }

        #endregion

        private static T invoke<T>(Func<T> func) { return func(); }
    }
}
