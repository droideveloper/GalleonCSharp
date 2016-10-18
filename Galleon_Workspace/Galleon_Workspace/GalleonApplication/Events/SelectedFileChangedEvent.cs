using GalleonApplication.Extra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Events {

    public class SelectedFileChangedEvent : IEvent {

        public SelectedFileChangedEvent(DocumentEntity doc) {
            Selection = doc;
        }

        public DocumentEntity Selection { get; private set; }
    }
}
