using GalleonApplication.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Events {
    public class SearchBarEvent : IEvent {

        public bool isShow { get; set; }
        public bool isProgress { get; set; }
        public bool isQueryChange { get; set; }
        public string queryText { get; set; }
    }
}
