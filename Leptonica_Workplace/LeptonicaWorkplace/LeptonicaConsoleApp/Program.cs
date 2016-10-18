using Leptonica;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.IO;
using Leptonica.Interop;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace LeptonicaConsoleApp {
    class Program {

        static void Main(string[] args) {
            
            new DirectoryInfo(@"C:\Users\ACER\Desktop\Tool")
                .GetFiles("*.jpg", SearchOption.AllDirectories)               
                .ToList()
                .ForEach(x => {
                    
                    Pix pixs = Pix.LoadFromFile(x.FullName);
                    pixs = pixs.PixBackgroundNormSimple();
                    pixs = pixs.PixConvertRGBToGray();
                    pixs = pixs.PixFindSkewAndDeskew();
                    pixs = pixs.PixTophat();
                    pixs = pixs.PixInvert();
                    pixs = pixs.PixGammaRTC();
                    pixs = pixs.PixThresholdToBinary();
                    /*
                    pixs = Pix.Create(LeptonicaNativeApi.Native.pixBackgroundNormSimple(pixs.Reference, IntPtr.Zero, IntPtr.Zero));
                    pixs = Pix.Create(LeptonicaNativeApi.Native.pixConvertRGBToGray(pixs.Reference, 0.5f, 0.3f, 0.2f));

                    float angle, confidence;
                    pixs = Pix.Create(LeptonicaNativeApi.Native.pixFindSkewAndDeskew(pixs.Reference, 2, out angle, out confidence));

                    if(confidence > 2 && confidence < 3) {
                        angle *= (float) Math.PI / 180.0f;
                        pixs = Pix.Create(LeptonicaNativeApi.Native.pixRotate(pixs.Reference, angle, RotationMethod.Shear, RotationFill.White, pixs.Width, pixs.Height));
                    }
                        
                    pixs = Pix.Create(LeptonicaNativeApi.Native.pixTophat(pixs.Reference, 17, 17, L_TOPHAT.BLACK));
                    LeptonicaNativeApi.Native.pixInvert(pixs.Reference, pixs.Reference);
                    LeptonicaNativeApi.Native.pixGammaTRC(pixs.Reference, pixs.Reference, 1.0f, 170, 245);

                    pixs = Pix.Create(LeptonicaNativeApi.Native.pixThresholdToBinary(pixs.Reference, 35));*/
                    pixs.Save(x.ToNewExtension(".pdf").FullName, ImageSaveFormat.Lpdf);
                    pixs.Dispose();
                });

            Console.WriteLine("Entre key to exit...");
            Console.ReadLine();
        }

        static T Invoke<T>(Func<T> func) { return func();  }
        
    }
}
