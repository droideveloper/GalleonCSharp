using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leptonica {

    public enum RotationMethod : int {

        /// Use area map rotation, if possible.
        /// <summar>sick compiler of god damn Microsoft</summar>
        AreaMap = 1,
        /// <summary>
        /// Use shear rotation.
        /// </summary>
        Shear = 2,
        /// <summary>
        /// Use sampling.
        /// </summary>
        Sampling = 3
    }
}
