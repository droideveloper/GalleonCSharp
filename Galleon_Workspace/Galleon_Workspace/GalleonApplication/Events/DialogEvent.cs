using GalleonApplication.Extra;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Events {
    
    public class DialogEvent : IEvent {

        public bool isCloseEvent { get; set; }
        public object view { get; set; }
        public string identifier { get; set; }
        public DialogClosingEventHandler closeHandler { get; set; }
    }
}
