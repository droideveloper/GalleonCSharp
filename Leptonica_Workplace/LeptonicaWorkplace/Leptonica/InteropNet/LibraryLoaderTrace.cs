using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leptonica.InteropNet {

    public static class LibraryLoaderTrace {

        const bool printToConsole = false;
        readonly static TraceSource trace = new TraceSource("Leptonica");

        private static void Print(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void TraceInformation(string format, params object[] args) {
            if(printToConsole) {
                //Print(string.Format(CultureInfo.CurrentCulture, format, args));
            } else {
                trace.TraceEvent(TraceEventType.Information, 0, string.Format(CultureInfo.CurrentCulture, format, args));
            }
        }

        public static void TraceError(string format, params object[] args) {
            if(printToConsole) {
                //Print(string.Format(CultureInfo.CurrentCulture, format, args));
            } else {
                trace.TraceEvent(TraceEventType.Error, 0, string.Format(CultureInfo.CurrentCulture, format, args));
            }
        }

        public static void TraceWarning(string format, params object[] args) {
            if(printToConsole) {
                //Print(string.Format(CultureInfo.CurrentCulture, format, args));
            } else {
                trace.TraceEvent(TraceEventType.Warning, 0, string.Format(CultureInfo.CurrentCulture, format, args));
            }
        }
    }
}