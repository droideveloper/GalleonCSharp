using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MuPDFLibrary {
    /// <summary>
    /// Interaction logic for SinglePagePDFPanel.xaml
    /// </summary>
    public partial class SinglePagePDFPanel : UserControl, IPDFPanel {

        private MuPDFPanel          parent;
        private ScrollViewer        scrollViewer;
        private PDFImageProvider    imageProvider;
        private int                 pageIndex = 0;
        
        public SinglePagePDFPanel(MuPDFPanel parent) {
            InitializeComponent();
            this.parent = parent;
            SizeChanged += (s, e) => {
                scrollViewer = this.FindChild<ScrollViewer>();
            };
        }

        public ScrollViewer ScrollViewer {
            get {
                return scrollViewer;
            }
        }

        public UserControl Instance {
            get {
                return this;
            }
        }

        public float CurrentZoom {
            get {
                return imageProvider != null ? imageProvider.Settings.ZoomFactor : 1.0f;
            }
        }

        public void Load(Interop.IPDFSource source, string password = null) {
            scrollViewer = this.FindChild<ScrollViewer>();
            imageProvider = new PDFImageProvider(source, parent.PageCount,
                                                 new DisplaySettings(parent.GetPagePer(), parent.ViewType, parent.HMargin, parent.PageRotation), 
                                                 false, password);
            pageIndex = 0;
            if(scrollViewer != null) {
                scrollViewer.Visibility = System.Windows.Visibility.Visible;
            }

            if(parent.Zoom == ZoomType.FIXED) {
                SetItemSource();
            } else if(parent.Zoom == ZoomType.FIT_HEIGHT) {
                ZoomToHeight();
            } else if(parent.Zoom == ZoomType.FIT_WIDTH) {
                ZoomToWidth();
            }
        }

        public void Unload() {
            scrollViewer.Visibility = System.Windows.Visibility.Collapsed;
            scrollViewer.ScrollToHorizontalOffset(0);
            scrollViewer.ScrollToVerticalOffset(0);
            pageIndex = 0;
            imageProvider = null;
        }

        public void Zoom(double zoomFactor) {
            ZoomInternal(zoomFactor);
        }

        public void ZoomIn() {
            ZoomInternal(CurrentZoom + parent.ZoomStep);
        }

        public void ZoomOut() {
            ZoomInternal(CurrentZoom - parent.ZoomStep);
        }

        public void ZoomToWidth() {
            double scrollBarWidth = scrollViewer.ComputedVerticalScrollBarVisibility == System.Windows.Visibility.Visible ? SystemParameters.VerticalScrollBarWidth : 0;
            double zoomFactor = (parent.ActualWidth - scrollBarWidth) / parent.PageBounds[pageIndex].SizeIncludingOffset.Width;
            PageBound pageBound = parent.PageBounds[pageIndex];

            if(scrollBarWidth == 0 && ((pageBound.Size.Height * zoomFactor) + pageBound.VOffset) >= parent.ActualHeight) {
                scrollBarWidth += SystemParameters.VerticalScrollBarWidth;
            }

            scrollBarWidth += 2; 
            zoomFactor = (parent.ActualWidth - scrollBarWidth) / parent.PageBounds[pageIndex].SizeIncludingOffset.Width;
            ZoomInternal(zoomFactor);
        }

        public void ZoomToHeight() {
            double scrollBarHeight = scrollViewer.ComputedHorizontalScrollBarVisibility == System.Windows.Visibility.Visible ? SystemParameters.HorizontalScrollBarHeight : 0;
            double zoomFactor = (parent.ActualHeight - scrollBarHeight) / parent.PageBounds[pageIndex].SizeIncludingOffset.Height;
            PageBound pageBound = parent.PageBounds[pageIndex];

            if(scrollBarHeight == 0 && ((pageBound.Size.Width * zoomFactor) + pageBound.HOffset) >= parent.ActualWidth) {
                scrollBarHeight += SystemParameters.HorizontalScrollBarHeight;
            }

            zoomFactor = (parent.ActualHeight - scrollBarHeight) / parent.PageBounds[pageIndex].SizeIncludingOffset.Height;
            ZoomInternal(zoomFactor);
        }

        public void GotoPage(int pageNumber) {
            pageIndex = pageIndex - 1;
            SetItemSource();
            if(scrollViewer != null) {
                scrollViewer.ScrollToTop();
            }
        }

        public void GotoPreviousPage() {
            int prevPageIndex = pageIndex.GetPreviousPageIndex(parent.ViewType);
            if(prevPageIndex == -1) { return; }
            pageIndex = prevPageIndex;
            SetItemSource();
            if(scrollViewer != null) {
                scrollViewer.ScrollToTop();
            }
        }

        public void GotoNextPage() {
            int nextPageIndex = pageIndex.GetNextPageIndex(parent.PageCount, parent.ViewType);
            if(nextPageIndex == -1) { return; }
            pageIndex = nextPageIndex;
            SetItemSource();
            if(scrollViewer != null) {
                scrollViewer.ScrollToTop();
            }
        }

        public int GetCurrentPageIndex(ViewType viewType) {
            return pageIndex;
        }

        private void SetItemSource() {
            int startIndex = pageIndex.GetVisibleIndexFromPageIndex(parent.ViewType);
            itemsControl.ItemsSource = imageProvider.FetchRange(startIndex, parent.GetPagePer())
                                                    .FirstOrDefault();
        }

        private void ZoomInternal(double zoomFactor) {
            zoomFactor = zoomFactor > parent.MaxZoomFactor ? parent.MaxZoomFactor : zoomFactor < parent.MinZoomFactor ? parent.MinZoomFactor : zoomFactor;
            imageProvider.Settings.ZoomFactor = (float) zoomFactor;
            SetItemSource();
        }
    }
}
