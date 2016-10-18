using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra {
    
    public class DependencyInjector {

        private static StandardKernel mStandardKernel;

        public static T Get<T>() {
            return mStandardKernel.Get<T>();
        }

        public static void Init(params INinjectModule[] modules) {
            if(mStandardKernel == null) {
                mStandardKernel = new StandardKernel(modules);
            }
        }
    }
}
