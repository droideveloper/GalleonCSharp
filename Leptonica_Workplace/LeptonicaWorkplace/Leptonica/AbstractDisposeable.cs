using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leptonica {

    public abstract class AbstractDisposeable : IDisposable {

        static readonly TraceSource trace = new TraceSource("Leptonica");

        protected AbstractDisposeable() {
            IsDisposed = false;
        }

        ~AbstractDisposeable() {
            Dispose(false);
            trace.TraceEvent(TraceEventType.Warning, 0, "{0} was not disposed off.", this);
        }

        public void Dispose() {
            Dispose(true);

            IsDisposed = true;
            GC.SuppressFinalize(this);

            if(Disposed != null) {
                Disposed(this, EventArgs.Empty);
            }
        }

        public bool IsDisposed { get; private set; }

        public event EventHandler<EventArgs> Disposed;

        protected virtual void VerifyNotDisposed() {
            if(IsDisposed) {
                throw new ObjectDisposedException(ToString());
            }
        }

        protected abstract void Dispose(bool isDisposing);
    }
}
