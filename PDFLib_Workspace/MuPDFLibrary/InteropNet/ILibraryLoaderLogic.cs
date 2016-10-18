using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuPDFLibrary.InteropNet {
    
    public interface ILibraryLoaderLogic {

        IntPtr LoadLibrary(string fileName);
        bool FreeLibrary(IntPtr libraryHandle);
        IntPtr GetProcAddress(IntPtr libraryHandle, string functionName);
        string FixUpLibraryName(string fileName);            
    }
}
