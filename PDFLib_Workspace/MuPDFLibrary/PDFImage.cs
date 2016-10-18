using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MuPDFLibrary {

    internal class PDFImage {    
        public ImageSource ImageSource  { get; set; }
        public Thickness   Margin       { get; set; }
    }

    internal class DisplaySettings {

        public int ImagesPerRow         { get; set; }
        public double HOffsetPage       { get; set; }
        public ViewType ViewType        { get; set; }
        public float ZoomFactor         { get; set; }
        public ImageRotation Rotation   { get; set; }

        public DisplaySettings(int imagesPerRow, ViewType viewType, double horizontalOffsetBetweenPages, ImageRotation rotation = ImageRotation.NONE, float zoomFactor = 1.0f) {
			this.ImagesPerRow = imagesPerRow;
			this.ZoomFactor = zoomFactor;
			this.ViewType = viewType;
            this.HOffsetPage = viewType == ViewType.SINGLE_PAGE ? 0 : horizontalOffsetBetweenPages;
			this.Rotation = rotation;
		}
    }

    internal class PageBound {
        public Size Size        { get; set; }
        public double VOffset   { get; set; }
        public double HOffset   { get; set; }

        public Size SizeIncludingOffset {
            get {
                return new Size(Size.Width + HOffset, Size.Height + VOffset);
            }
        }

        public PageBound(Size size, double vOffset, double hOffset) {
            Size = size;
            VOffset = vOffset;
            HOffset = hOffset;
        }
    }
}
