using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leptonica {

    public enum RotationFill : int {
        /// <summary>
        /// Bring in white pixels from the outside.
        /// </summary>
        White = 1,
        /// <summary>
        /// Bring in black pixels from the outside.
        /// </summary>
        Black = 2
    }
}
