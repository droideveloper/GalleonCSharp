using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuPDFLibrary {

    public abstract class AbstractDisposable : IDisposable {

        static readonly TraceSource trace = new TraceSource("MuPDFLibrary");

        public bool IsDisposed { get; private set; }

        public event EventHandler<EventArgs> Disposed;

        protected AbstractDisposable() {
            IsDisposed = false;
        }

        ~AbstractDisposable() {
            Dispose(false);
            trace.TraceEvent(TraceEventType.Warning, 0, "{0} was not disposed off.", this);
        }

        public void Dispose() {
            Dispose(true);
            //disposed too late bro
            IsDisposed = true;
            GC.SuppressFinalize(this);
            //notify listeners
            if(Disposed != null) {
                Disposed(this, EventArgs.Empty);
            }
        }

        protected virtual void VerifyNotDisposed() {
            if(IsDisposed) {
                throw new ObjectDisposedException(ToString());
            }
        }

        protected abstract void Dispose(bool isDisposing);
    }
}
