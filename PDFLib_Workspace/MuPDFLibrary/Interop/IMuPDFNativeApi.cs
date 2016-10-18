using MuPDFLibrary.InteropNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MuPDFLibrary.Interop {
    
    public interface IMuPDFNativeApi {

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZNewContext)]
        IntPtr NewContext(IntPtr alloc, IntPtr locks, uint maxStore);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZFreeContext)]
        IntPtr FreeContext(IntPtr context);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = MuPDFConstants.MethodFZOpenFileW)]
        IntPtr OpenFile(IntPtr context, string fileName);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZOpenDocumentWithStream)]
        IntPtr OpenDocumentStream(IntPtr context, string magic, IntPtr stream);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZClose)]
        IntPtr CloseStream(IntPtr stream);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZCloseDocument)]
        IntPtr CloseDocument(IntPtr document);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZCountPages)]
        int CountPages(IntPtr document);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZBoundPage)]
        Rectangle BoundPage(IntPtr document, IntPtr page);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZClearPixmapWithValue)]
        void ClearPixmap(IntPtr context, IntPtr pix, int byteValue);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZFindDeviceColorspace)]
        IntPtr FindDeviceColorSpace(IntPtr context, string colorSpace);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZFreeDevice)]
        void FreeDevice(IntPtr device);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZFreePage)]
        void FreePage(IntPtr document, IntPtr page);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZLoadPage)]
        IntPtr LoadPage(IntPtr document, int pageNumber);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZNewDrawDevice)]
        IntPtr NewDrawDevice(IntPtr context, IntPtr pix);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZNewPixmap)]
        IntPtr NewPixmap(IntPtr context, IntPtr colorSpace, int width, int height);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZRunPage)]
        void RunPage(IntPtr document, IntPtr page, IntPtr device, Matrix transform, IntPtr cookie);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZDropPixmap)]
        void DropPixmap(IntPtr context, IntPtr pix);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZPixmapSamples)]
        IntPtr GetSamples(IntPtr context, IntPtr pix);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZNeedsPassword)]
        int NeedsPassword(IntPtr document);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZAuthenticatePassword)]
        int AuthenticatePassword(IntPtr document, string password);

        [RuntimeDllImport(MuPDFConstants.DLLName, CallingConvention = CallingConvention.Cdecl, EntryPoint = MuPDFConstants.MethodFZOpenMemory)]
        IntPtr OpenStream(IntPtr context, IntPtr data, int length);
    }

    public class MuPDFConstants {

        public const string DLLName = "libmupdf";

        #region MethodNames

        public const string MethodFZNewContext              = "fz_new_context";//
        public const string MethodFZFreeContext             = "fz_free_context";//
        public const string MethodFZOpenFileW               = "fz_open_file_w";//
        public const string MethodFZOpenDocumentWithStream  = "fz_open_document_with_stream";//
        public const string MethodFZClose                   = "fz_close";//
        public const string MethodFZCloseDocument           = "fz_close_document";//
        public const string MethodFZCountPages              = "fz_count_pages";//
        public const string MethodFZBoundPage               = "fz_bound_page";//
        public const string MethodFZClearPixmapWithValue    = "fz_clear_pixmap_with_value";//
        public const string MethodFZFindDeviceColorspace    = "fz_find_device_colorspace";//
        public const string MethodFZFreeDevice              = "fz_free_device";//
        public const string MethodFZFreePage                = "fz_free_page";//
        public const string MethodFZLoadPage                = "fz_load_page";//
        public const string MethodFZNewDrawDevice           = "fz_new_draw_device";//
        public const string MethodFZNewPixmap               = "fz_new_pixmap";//
        public const string MethodFZRunPage                 = "fz_run_page";//
        public const string MethodFZDropPixmap              = "fz_drop_pixmap";//
        public const string MethodFZPixmapSamples           = "fz_pixmap_samples";//
        public const string MethodFZNeedsPassword           = "fz_needs_password";//
        public const string MethodFZAuthenticatePassword    = "fz_authenticate_password";//
        public const string MethodFZOpenMemory              = "fz_open_memory";//

        #endregion
    }

    public struct Rectangle {
        
        public float Left, Top, Right, Bottom;

        public float Width {
            get { 
                return Right - Left;
            }
        }

        public float Height {
            get {
                return Bottom - Top;
            }
        }
    }

    public struct BBox {
        public float Left, Top, Right, Bottom;
    }

    public struct Matrix {
        public float A, B, C, D, E, F;
    }

    public interface IPDFSource { }

    public class FileSource : IPDFSource {

        public string Filename { get; private set; }

        public FileSource(string filename) {
            Filename = filename;
        }
    }

    public class ByteSource : IPDFSource {

        public byte[] Data { get; private set; }

        public ByteSource(byte[] data) {
            Data = data;
        }
    }

    unsafe public static class MuPDFNativeApi {

        private static IMuPDFNativeApi native;

        public static IMuPDFNativeApi Native {
            get {
                if(native == null) {
                    NativeInit();
                }
                return native;
            }
        }

        public static void NativeInit() {
            if(native == null) {
                native = InteropRuntimeImplementer.CreateInstance<IMuPDFNativeApi>();
            }
        }
    }
}
