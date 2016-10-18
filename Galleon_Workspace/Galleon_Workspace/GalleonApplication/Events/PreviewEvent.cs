using GalleonApplication.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Events {
    
    public class PreviewEvent : IEvent {

        public bool isDisplayEvent { get; set; }
        public string DisplayFile { get; set; }
    }
}
