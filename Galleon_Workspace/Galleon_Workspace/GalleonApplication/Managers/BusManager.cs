using GalleonApplication.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Managers {
    
    public class BusManager {
        //only use this instance
        private static BusManager IMP = new BusManager();


        private readonly Subject<object> rxBus;
        
        private BusManager() { 
            rxBus = new Subject<object>();
        }    
        
        public void post(object eventObject) {
            if(eventObject is IEvent) {
                rxBus.OnNext(eventObject);
            } else {
                throw new ArgumentException("event object must be IEvent type");
            }
        }

        public IDisposable Register(Action<object> subscribe) {
            return rxBus.Subscribe(subscribe);
        }

        public IDisposable Register(IObserver<object> subscribe) {
            return rxBus.Subscribe(subscribe);
        }

        public void Unregister(IDisposable subscription) {
            if(subscription != null) {
                subscription.Dispose();
            }
        }

        public bool HasObservers() {
            return rxBus.HasObservers;
        }

        public static void Send(object eventObject) {
            IMP.post(eventObject);
        }

        public static IDisposable Add(IObserver<object> subscriber) {
            return IMP.Register(subscriber);
        }

        public static IDisposable Add(Action<object> subscribe) {
            return IMP.Register(subscribe);
        }

        public static void Remove(IDisposable subscription) {
            IMP.Unregister(subscription);
        }
    }
}
