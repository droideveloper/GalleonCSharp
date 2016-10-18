using GalleonApplication.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Events {
    
    public class SnackbarEvent : IEvent {

        public bool isCloseEvent { get; set; }
        public string textMessage { get; set; }
        public Nullable<TimeSpan> withDuration { get; set; }
    }
}
