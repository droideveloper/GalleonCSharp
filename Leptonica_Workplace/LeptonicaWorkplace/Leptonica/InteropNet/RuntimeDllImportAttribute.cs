using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Leptonica.InteropNet {
    
    [ComVisible(true)]
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class RuntimeDllImportAttribute : Attribute {
        public string EntryPoint;

        public CallingConvention CallingConvention;

        public CharSet CharSet;

        public bool SetLastError;

        public bool BestFitMapping;

        public bool ThrowOnUnmappableChar;

        public string LibraryFileName { get; private set; }

        public RuntimeDllImportAttribute(string libraryFileName) {
            LibraryFileName = libraryFileName;
        }
    }
}