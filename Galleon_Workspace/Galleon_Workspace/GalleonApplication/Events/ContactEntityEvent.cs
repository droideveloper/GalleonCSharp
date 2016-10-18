using GalleonApplication.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Events {
    
    public class ContactEntityEvent : IEvent {

        public ContactEntity ContactEntity { get; set; }
    }
}
