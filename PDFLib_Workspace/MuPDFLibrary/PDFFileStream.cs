using MuPDFLibrary.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MuPDFLibrary {
    
    public unsafe sealed class PDFFileStream : AbstractDisposable {

        public const uint FZ_STORE_DEFAULT = 256 << 20;
        public const string PDF_EXTENSION = ".pdf";

        public IntPtr Context   { get; private set; }
        public IntPtr Stream { get; private set; }
        public IntPtr Document { get; private set; }

        public PDFFileStream(IPDFSource source) {
            if(source is FileSource) {
                FileSource fileSource = (FileSource) source;
                Context = MuPDFNativeApi.Native.NewContext(IntPtr.Zero, IntPtr.Zero, FZ_STORE_DEFAULT);
                Stream = MuPDFNativeApi.Native.OpenFile(Context, fileSource.Filename);
                Document = MuPDFNativeApi.Native.OpenDocumentStream(Context, PDF_EXTENSION, Stream);
            } else if(source is ByteSource) {
                ByteSource byteSource = (ByteSource) source;
                Context = MuPDFNativeApi.Native.NewContext(IntPtr.Zero, IntPtr.Zero, FZ_STORE_DEFAULT);
                GCHandle pinnedArray = GCHandle.Alloc(byteSource.Data, GCHandleType.Pinned);
                IntPtr ptr = pinnedArray.AddrOfPinnedObject();
                Stream = MuPDFNativeApi.Native.OpenStream(Context, ptr, byteSource.Data.Length);
                Document = MuPDFNativeApi.Native.OpenDocumentStream(Context, PDF_EXTENSION, Stream);
                pinnedArray.Free();
            }
        }

        protected override void Dispose(bool isDisposing) {
            MuPDFNativeApi.Native.CloseDocument(Document);
            MuPDFNativeApi.Native.CloseStream(Stream);
            MuPDFNativeApi.Native.FreeContext(Context);
        }
    }
}
