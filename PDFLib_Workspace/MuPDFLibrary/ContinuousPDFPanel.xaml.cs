using MuPDFLibrary.Interop;
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
    /// Interaction logic for ContinuousPDFPanel.xaml
    /// </summary>
    public partial class ContinuousPDFPanel : UserControl, IPDFPanel {

        private MuPDFPanel                                      parent;
        private ScrollViewer                                    scrollViewer;
        private CustomVirtualizingPanel                         virtualPanel;
        private PDFImageProvider                                imageProvider;
        private VirtualizingCollection<IEnumerable<PDFImage>>   virtualPDFPages;

        public ContinuousPDFPanel(MuPDFPanel parent) {
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

        public void Load(IPDFSource source, string password = null) {
            virtualPanel = this.FindChild<CustomVirtualizingPanel>();
            scrollViewer = this.FindChild<ScrollViewer>();
            virtualPanel.PageBounds = parent.PageBounds.Select(b => b.SizeIncludingOffset).ToArray();
            imageProvider = new PDFImageProvider(source, parent.PageCount,
                                                 new DisplaySettings(parent.GetPagePer(), parent.ViewType, parent.HMargin, parent.PageRotation),
                                                 password: password);

            if(parent.Zoom == ZoomType.FIXED) {
                CreateNewItemsSource();
            } else if(parent.Zoom == ZoomType.FIT_HEIGHT) {
                ZoomToHeight();
            } else if(parent.Zoom == ZoomType.FIT_WIDTH) {
                ZoomToWidth();
            }

            if(scrollViewer != null) {
                scrollViewer.Visibility = System.Windows.Visibility.Visible;
                scrollViewer.ScrollToTop();
            }
        }

        public void Unload() {
            scrollViewer.Visibility = System.Windows.Visibility.Collapsed;
            scrollViewer.ScrollToHorizontalOffset(0);
            scrollViewer.ScrollToVerticalOffset(0);
            imageProvider = null;

            if(virtualPDFPages != null) {
                virtualPDFPages.CleanUpAllPages();
                virtualPDFPages = null;
            }

            itemsControl.ItemsSource = null;
        }

        public void Zoom(double zoomFactor) {
            ZoomInternal(zoomFactor);
        }

        public void ZoomIn() {
            ZoomInternal(this.CurrentZoom + this.parent.ZoomStep);
        }

        public void ZoomOut() {
            ZoomInternal(this.CurrentZoom - this.parent.ZoomStep);
        }

        public void ZoomToWidth() {
            var scrollBarWidth = this.scrollViewer.ComputedVerticalScrollBarVisibility == System.Windows.Visibility.Visible ? SystemParameters.VerticalScrollBarWidth : 0;
            scrollBarWidth += 2;
            ZoomInternal((this.ActualWidth - scrollBarWidth) / this.parent.PageBounds.Max(f => f.SizeIncludingOffset.Width));
        }

        public void ZoomToHeight() {
            var scrollBarHeight = this.scrollViewer.ComputedHorizontalScrollBarVisibility == System.Windows.Visibility.Visible ? SystemParameters.HorizontalScrollBarHeight : 0;
            ZoomInternal((this.ActualHeight - scrollBarHeight) / this.parent.PageBounds.Max(f => f.SizeIncludingOffset.Height));
        }

        public void GotoPage(int pageNumber) {
            if(scrollViewer == null) { return; }
            int startIndex = (pageNumber - 1).GetVisibleIndexFromPageIndex(parent.ViewType);
            double vOffset = virtualPanel.GetVerticalOffsetByItemIndex(startIndex);
            scrollViewer.ScrollToVerticalOffset(vOffset);
        }

        public void GotoPreviousPage() {
            if(scrollViewer == null) { return; }
            int page = GetCurrentPageIndex(parent.ViewType);
            if(page == 0) { return; }

            int startIndex = (page - 1).GetVisibleIndexFromPageIndex(parent.ViewType);
            double verticalOffset = virtualPanel.GetVerticalOffsetByItemIndex(startIndex);
            scrollViewer.ScrollToVerticalOffset(verticalOffset);
        }

        public void GotoNextPage() {
            int page = GetCurrentPageIndex(parent.ViewType);
            int next = page.GetNextPageIndex(parent.PageCount, parent.ViewType);
            if(next == -1) { return; }

            GotoPage(next + 1);
        }

        public int GetCurrentPageIndex(ViewType viewType) {
            return scrollViewer == null ? 0 : virtualPanel.GetItemIndexByVerticalOffset(scrollViewer.VerticalOffset) > 0 ?
                    viewType == ViewType.FACING ? virtualPanel.GetItemIndexByVerticalOffset(scrollViewer.VerticalOffset) * 2 :
                    viewType == ViewType.BOOK ? (virtualPanel.GetItemIndexByVerticalOffset(scrollViewer.VerticalOffset) * 2 - 1) :
                    virtualPanel.GetItemIndexByVerticalOffset(scrollViewer.VerticalOffset) 
                : virtualPanel.GetItemIndexByVerticalOffset(scrollViewer.VerticalOffset);
        }

        private void CreateNewItemsSource() {
            TimeSpan timeout = TimeSpan.FromSeconds(2);

            if(virtualPDFPages != null) {
                virtualPDFPages.CleanUpAllPages();
            }

            virtualPDFPages = new AsyncVirtualizingCollection<IEnumerable<PDFImage>>(imageProvider, parent.GetPagePer(), timeout);
            itemsControl.ItemsSource = virtualPDFPages;
        }

        private void ZoomInternal(double zoomFactor) {
            if(zoomFactor > parent.MaxZoomFactor) {
                zoomFactor = parent.MaxZoomFactor;
            } else if(zoomFactor < parent.MinZoomFactor) {
                zoomFactor = parent.MinZoomFactor;
            }

            double yOffset = scrollViewer.VerticalOffset;
            double xOffset = scrollViewer.HorizontalOffset;
            float zoom = CurrentZoom;

            if(Math.Abs(Math.Round(zoom, 2) - Math.Round(zoomFactor, 2)) == 0.0) { return; }

            virtualPanel.PageBounds = parent.PageBounds.Select(f => new Size(f.Size.Width * zoomFactor + f.HOffset, f.Size.Height * zoomFactor + f.VOffset)).ToArray();
            imageProvider.Settings.ZoomFactor = (float) zoomFactor;

            CreateNewItemsSource();

            scrollViewer.ScrollToHorizontalOffset((xOffset / zoom) * zoomFactor);
            scrollViewer.ScrollToVerticalOffset((yOffset / zoom) * zoomFactor);
        }
    }
}
