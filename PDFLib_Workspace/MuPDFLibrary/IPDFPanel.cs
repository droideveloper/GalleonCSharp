using MuPDFLibrary.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MuPDFLibrary {

    interface IPDFPanel {
        ScrollViewer ScrollViewer { get; }
        UserControl Instance { get; }
        float CurrentZoom { get; }
        void Load(IPDFSource source, string password = null);
        void Unload();
        void Zoom(double zoomFactor);
        void ZoomIn();
        void ZoomOut();
        void ZoomToWidth();
        void ZoomToHeight();
        void GotoPage(int pageNumber);
        void GotoPreviousPage();
        void GotoNextPage();
        int GetCurrentPageIndex(ViewType viewType);
    }
}
