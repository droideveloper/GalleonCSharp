using MuPDFLibrary.Interop;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace MuPDFLibrary {
    /// <summary>
    /// Interaction logic for MuPDFPanel.xaml
    /// </summary>
    public partial class MuPDFPanel : UserControl {

        private const string PAGE_MARGIN        = "PageMargin";
        private const string ZOOM_STEP          = "ZoomStep";
        private const string MIN_ZOOM_FACTOR    = "MinZoomFactor";
        private const string MAX_ZOOM_FACTOR    = "MaxZoomFactor";
        private const string VIEW_TYPE          = "ViewType";
        private const string PAGE_ROTATION      = "PageRotation";
        private const string PAGE_DISPLAY       = "PageDisplay";

        public event EventHandler PDFLoaded;
        public event EventHandler ZoomChanged;
        public event EventHandler ViewChanged;
        public event EventHandler DisplayChanged;

        private ZoomType        zoom = ZoomType.FIXED;
        private IPDFPanel       pdf;
        private PageBound[]     pageBounds;
        private DispatcherTimer timer;

        public static readonly DependencyProperty PageMarginProperty = 
            DependencyProperty.Register(PAGE_MARGIN, typeof(Thickness), typeof(MuPDFPanel), new FrameworkPropertyMetadata(new Thickness(0, 2, 4, 2)));

        public static readonly DependencyProperty ZoomStepProperty =
            DependencyProperty.Register(ZOOM_STEP, typeof(double), typeof(MuPDFPanel), new FrameworkPropertyMetadata(0.25));

        public static readonly DependencyProperty MinZoomFactorProperty = 
            DependencyProperty.Register(MIN_ZOOM_FACTOR, typeof(double), typeof(MuPDFPanel), new FrameworkPropertyMetadata(0.15));

        public static readonly DependencyProperty MaxZoomFactorProperty =
            DependencyProperty.Register(MAX_ZOOM_FACTOR, typeof(double), typeof(MuPDFPanel), new FrameworkPropertyMetadata(6.0));

        public static readonly DependencyProperty ViewTypeProperty =
            DependencyProperty.Register(VIEW_TYPE, typeof(ViewType), typeof(MuPDFPanel), new FrameworkPropertyMetadata(ViewType.SINGLE_PAGE));

        public static readonly DependencyProperty PageRotationProperty =
            DependencyProperty.Register(PAGE_ROTATION, typeof(ImageRotation), typeof(MuPDFPanel), new FrameworkPropertyMetadata(ImageRotation.NONE));

        public static readonly DependencyProperty PageDisplayProperty =
            DependencyProperty.Register(PAGE_DISPLAY, typeof(PageDisplayType), typeof(MuPDFPanel), new FrameworkPropertyMetadata(PageDisplayType.SINGLE));

        public Thickness PageMargin {
            get { return (Thickness) GetValue(PageMarginProperty); }
            set { SetValue(PageMarginProperty, value); }
        }

        public double ZoomStep {
            get { return (double) GetValue(ZoomStepProperty); }
            set { SetValue(ZoomStepProperty, value); }
        }

        public double MinZoomFactor {
            get { return (double) GetValue(MinZoomFactorProperty); }
            set { SetValue(MinZoomFactorProperty, value); }
        }

        public double MaxZoomFactor {
            get { return (double) GetValue(MaxZoomFactorProperty); }
            set { SetValue(MaxZoomFactorProperty, value); }
        }

        public ViewType ViewType {
            get { return (ViewType) GetValue(ViewTypeProperty); }
            set { SetValue(ViewTypeProperty, value); }
        }

        public ImageRotation PageRotation {
            get { return (ImageRotation) GetValue(PageRotationProperty); }
            set { SetValue(PageRotationProperty, value); }
        }

        public PageDisplayType PageDisplay {
            get { return (PageDisplayType) GetValue(PageDisplayProperty); }
            set { SetValue(PageDisplayProperty, value); }
        }

        public double HMargin           { get; private set; }
        public IPDFSource ContentSource { get; private set; }
        public int PageCount            { get; private set; }

        public ZoomType Zoom {
            get {
                return zoom;
            }
            private set {
                bool fireEvent = zoom != value;
                zoom = fireEvent ? value : zoom;

                if(ZoomChanged != null && fireEvent) {
                    ZoomChanged(this, EventArgs.Empty);
                }
            }
        }

        public float CurrentZoom {
            get {
                return pdf.CurrentZoom;
            }
        }

        internal PageBound[] PageBounds { 
            get {
                return pageBounds;
            } 
        }

        internal ScrollViewer ScrollViewer {
            get {
                return pdf.ScrollViewer;
            }
        }

        public MuPDFPanel() {
            InitializeComponent();

            NewPageDisplay(PageDisplay);

            SizeChanged += (s, e) => {
                if(ContentSource == null) { return; }
                //reset timer
                timer.Stop();
                timer.Start();
            };

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(150);
            timer.Tick += (s, e) => {
                timer.Stop();
                if(ContentSource == null) { return; }
                if(Zoom == ZoomType.FIT_WIDTH) { ZoomToWidth(); } 
                else if(Zoom == ZoomType.FIT_HEIGHT) { ZoomToHeight(); }
            };
        }

        public void OpenFile(string filename, string password = null) {
            if(!File.Exists(filename)) {
                throw new ArgumentNullException("no such file exists.");
            }
            Open(new FileSource(filename), password);
        }

        public void Open(IPDFSource source, string password = null) {
            LoadPdf(source, password);

            if(PDFLoaded != null) {
                PDFLoaded(this, EventArgs.Empty);
            }
        }

        public void UnloadPdf() {
            ContentSource = null;
            PageCount = 0;
            pdf.Unload();

            if(PDFLoaded != null) {
                PDFLoaded(this, EventArgs.Empty);
            }
        }

        public int GetPagePer() {
            return ViewType == ViewType.SINGLE_PAGE ? 1 : 2;
        }

        public int GetPageNumber() {
            return pdf != null ? pdf.GetCurrentPageIndex(ViewType) + 1 : -1;
        }

        public void ZoomToHeight() {
            pdf.ZoomToHeight();
            Zoom = ZoomType.FIT_HEIGHT;
        }

        public void ZoomToWidth() {
            pdf.ZoomToWidth();
            Zoom = ZoomType.FIT_WIDTH;
        }

        public void ZoomIn() {
            pdf.ZoomIn();
            SetFixedZoom();
        }

        public void ZoomOut() {
            pdf.ZoomOut();
            SetFixedZoom();
        }

        public void SetZoom(double zoomFactor) {
            pdf.Zoom(zoomFactor);
            SetFixedZoom();
        }

        public void SetFixedZoom() {
            Zoom = ZoomType.FIXED;
        }

        public void GotoPreviousPage() {
            pdf.GotoPreviousPage();
        }

        public void GotoNextPage() {
            pdf.GotoNextPage();
        }

        public void GotoPage(int index) {
            pdf.GotoPage(index);
        }

        public void GotoFirstPage() {
            GotoPage(1);
        }

        public void GotoLastPage() {
            GotoPage(PageCount);
        }

        public void RotateRight() {
            PageRotation = (PageRotation != ImageRotation.ROTATE_270) ? (ImageRotation) PageRotation + 1 : ImageRotation.NONE;
        }

        public void RotateLeft() {
            PageRotation = ((int) PageRotation > 0) ? (ImageRotation) PageRotation - 1 : ImageRotation.ROTATE_270;
        }

        public void TogglePageDisplay() {
            PageDisplay = PageDisplay == PageDisplayType.SINGLE ? PageDisplayType.CONTINOUS : PageDisplayType.SINGLE;
        }

        public void Rotation(ImageRotation rotation) {
            int page = pdf.GetCurrentPageIndex(ViewType) + 1;
            LoadPdf(ContentSource);
            pdf.GotoPage(page);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e) {
            base.OnPropertyChanged(e);
            string property = e.Property.Name;
            if(property.Equals(PAGE_DISPLAY)) {
                NewPageDisplay((PageDisplayType) e.NewValue);
            } else if(property.Equals(PAGE_ROTATION)) {
                Rotation((ImageRotation) e.NewValue);
            } else if(property.Equals(VIEW_TYPE)) {
                NewViewType((ViewType) e.OldValue);
            }
        }

        private PageBound[] CalcPageBounds(Size[] bounds, ViewType view) {
            int perPage = Math.Min(GetPagePer(), bounds.Length);
            double vOffset = PageMargin.Top + PageMargin.Bottom;

            if(view == ViewType.SINGLE_PAGE) {
                return bounds.Select(b => new PageBound(b, vOffset, 0))
                             .ToArray();
            } else {
                List<PageBound> pageBounds = new List<PageBound>();
                double hOffset = HMargin;
                for(int i = 0; i < bounds.Length; i++) {
                    if(i == 0 && view == ViewType.BOOK) {
                        pageBounds.Add(new PageBound(bounds[0], vOffset, 0));
                        continue;
                    }

                    List<Size> subset = bounds.Take(i, perPage).ToList();

                    pageBounds.Add(new PageBound(new Size(subset.Sum(f => f.Width), subset.Max(f => f.Height)), vOffset, hOffset));
                    i += (perPage - 1);
                }
                return pageBounds.ToArray();
            }
        }

        private void LoadPdf(IPDFSource source, string password = null) {
            pageBounds = CalcPageBounds(MuPDF.GetPageBounds(source, PageRotation), ViewType);
            PageCount = pageBounds.Length;
            pdf.Load(source);
        }

        private void NewViewType(ViewType exView) {
            Refresh(() => { }, exView);

            if(ViewChanged != null) {
                ViewChanged(this, EventArgs.Empty);
            }
        }

        private void NewPageDisplay(PageDisplayType pageDisplay) {
            Refresh(() => {
                MainPDFPanel.Children.Clear();
                pdf = (pageDisplay == PageDisplayType.SINGLE) ? (IPDFPanel) new SinglePagePDFPanel(this) : (IPDFPanel) new ContinuousPDFPanel(this);

                MainPDFPanel.Children.Add(pdf.Instance);
            }, ViewType);

            if(DisplayChanged != null) {
                DisplayChanged(this, EventArgs.Empty);
            }
        }

        private void Refresh(Action refresh, ViewType view) {
            int page = ContentSource != null ? pdf.GetCurrentPageIndex(view) + 1 : -1;
            float zoomLevel = ContentSource != null ? pdf.CurrentZoom : 1.0f;

            refresh();

            if(page > -1) {
                Action reload = () => {
                    LoadPdf(ContentSource);
                    pdf.Zoom(zoomLevel);
                    pdf.GotoPage(page);
                };
                if(pdf.Instance.IsLoaded) {
                    reload();
                } else {
                    pdf.Instance.Loaded += (s, e) => { reload(); };
                }
            }
        }
    }
}
