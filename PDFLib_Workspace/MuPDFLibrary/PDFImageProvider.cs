using MuPDFLibrary.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MuPDFLibrary {

    internal class PDFImageProvider : IItemsProvider<IEnumerable<PDFImage>> {

        private IPDFSource source;
        private int count = -1;
        private int total;
        private bool preFetch;
        private string password;

        public DisplaySettings Settings { get; private set; }

        public PDFImageProvider(IPDFSource source, int total, DisplaySettings settings, bool preFetch = true, string password = null) {
            this.source = source;
            this.total = total;
            this.Settings = settings;
            this.preFetch = preFetch;
            this.password = password;
        }
        
        public int FetchCount() {
            if(count == -1) {
                count = MuPDF.CountPages(source, password);
            }
            return count;
        }

        public IList<IEnumerable<PDFImage>> FetchRange(int startIndex, int count) {
            int perRow = Settings.ImagesPerRow;
            ViewType type = Settings.ViewType;

            startIndex = (startIndex * perRow) + 1;
            count = preFetch ? count * perRow : count;

            if(type == ViewType.BOOK) {
                if(startIndex == 1) {
                    count = Math.Min(total, preFetch ? (1 + perRow) : 0);
                } else {
                    startIndex--;
                }
            }

            int end = Math.Min(FetchCount(), startIndex + count - 1);
            List<IEnumerable<PDFImage>> content = new List<IEnumerable<PDFImage>>();
            List<PDFImage> rows = new List<PDFImage>(perRow);
            int offset = type == ViewType.BOOK ? 1 : 0;

            for(int i = Math.Min(FetchCount(), startIndex); i <= Math.Min(FetchCount(), Math.Max(startIndex, end)); i++) {
                Thickness margin = new Thickness(0, 0, Settings.HOffsetPage, 0);
                using(Bitmap bitmap = MuPDF.ExtractPage(source, i, Settings.ZoomFactor, password)) {
                    RotateFlipType flip = Settings.Rotation == ImageRotation.ROTATE_180 ? RotateFlipType.Rotate180FlipNone 
                                        : Settings.Rotation == ImageRotation.ROTATE_270 ? RotateFlipType.Rotate270FlipNone 
                                        : Settings.Rotation == ImageRotation.ROTATE_90  ? RotateFlipType.Rotate90FlipNone 
                                        : RotateFlipType.RotateNoneFlipNone;
                    bitmap.RotateFlip(flip);
                    
                    BitmapSource bitmapSource = bitmap.ToBitmapSource();
                    bitmapSource.Freeze();

                    margin.Right = (i == 0 && type == ViewType.BOOK) || ((i + offset) % 2 == 0) ? 0 : margin.Right;
                    PDFImage image = new PDFImage() { ImageSource = bitmapSource, Margin = margin };

                    if(type == ViewType.BOOK && i == 1) {
                        content.Add(new[] { image });
                        continue;
                    }
                    rows.Add(image);
                }

                if(rows.Count % perRow == 0 || i == end) {
                    content.Add(rows);
                    if(i == end && rows.Count % perRow != 0) {
                        PDFImage last = rows.Last();
                        last.Margin = new Thickness(0);
                    }
                    if(i < end) {
                        rows = new List<PDFImage>(perRow);
                    }
                }
            }
            return content;
        }
    }
}
