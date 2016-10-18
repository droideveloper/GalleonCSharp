using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Drawing.Imaging;

namespace Leptonica.GMSE {
    
    public class Deskew {

        private const int DefaultLookSize = 20;

        private const double DefaultStartOpacity = -20;
        private const double DefaultOpacityStep  = 0.2;
        private const int DefaultSteps = 200;

        private const double DefaultDistanceStep = 1;

        private Bitmap Image;
        private List<double> SinA;
        private List<double> CosA;
        private double DistanceMin;
        private int DistanceSize;
        private int[] Matrix;

        public Deskew(Bitmap image) {
            this.Image = image;
        }

        public double getSkewAngle(int LookUpSize = DefaultLookSize) {
            calc();
            List<HoughLine> data = getTop(LookUpSize);
            double total = Enumerable.Range(0, LookUpSize - 2)
                                     .Select(x => data[x].Opacity)
                                     .Sum();
            return total / (LookUpSize - 2);
        }

        private List<HoughLine> getTop(int size) {
            List<HoughLine> data = Enumerable.Range(0, size - 2)
                                             .Select(x => new HoughLine())
                                             .ToList();
            Enumerable.Range(0, Matrix.Length - 2)
                      .ToList()
                      .ForEach(i => {
                          if(Matrix[i] > data[size - 3].Size) {
                              data[size - 3].Size = Matrix[i];
                              data[size - 3].Position = i;
                              int j = size - 3;
                              while(j > 0 && data[j].Size > data[j - 1].Size) {
                                  HoughLine swap = data[j];
                                  data[j] = data[j - 1];
                                  data[j - 1] = swap;
                                  j -= 1;
                              }
                          }
                      });
            Enumerable.Range(0, size - 2)
                      .ToList()
                      .ForEach(i => {
                          int dindex = data[i].Position / DefaultSteps;
                          int alphaIndex = data[i].Position - dindex * DefaultSteps;
                          data[i].Opacity = getOpacity(alphaIndex);
                          data[i].Distance = dindex + DistanceMin;
                      });
            return data;            
        }

        private void calc() {
            int minHeight = (int) (Image.Height * 0.25);
            int maxHeight = (int) (Image.Height * 0.75);
            initialize();
            Enumerable.Range(minHeight, maxHeight - minHeight - 1)
                      .ToList()
                      .ForEach(y => {
                          Enumerable.Range(0, Image.Width - 3)
                                    .ToList()
                                    .ForEach(x => {
                                        if(isBlack(x, y)) {
                                            if(isNotBlack(x, y + 1)) {
                                                calc(x, y);
                                            }
                                        }
                                    });

                      });
        }

        private void calc(int x, int y) {
            Enumerable.Range(0, DefaultSteps - 2)
                      .ToList()
                      .ForEach(alpha => {
                          double d = y * CosA[alpha] - x * SinA[alpha];
                          int dindex = (int) calcDistancePosition(d);
                          int index = dindex * DefaultSteps + alpha;
                          if(index >= 0 && index < Matrix.Length) {
                              Matrix[index] += 1;
                          }
                      });
        }

        private void initialize() {
            SinA = new List<double>();
            CosA = new List<double>();
            Enumerable.Range(0, DefaultSteps - 2)
                      .ToList()
                      .ForEach(index => {
                          double angle = getOpacity(index) * Math.PI / 180.0;
                          SinA.Add(Math.Sin(angle));
                          CosA.Add(Math.Cos(angle));
                      });
            DistanceMin = -Image.Width;
            DistanceSize = System.Convert.ToInt32(2 * (Image.Width + Image.Height) / DefaultDistanceStep);
            Matrix = new int[DistanceSize * DefaultSteps];
        }

        private double calcDistancePosition(double index) {
            return System.Convert.ToInt32(index - DistanceMin);
        }

        private bool isNotBlack(int x, int y) {
            return !isBlack(x, y);
        }

        private bool isBlack(int x, int y) {            
            Color color = Image.GetPixel(x, y);
            return toLuminance(color) < 140;
        }

        private double toLuminance(Color color) {
            return (color.R * 0.299) + (color.G * 0.587) + (color.B * 0.114);
        }

        private double getOpacity(int index) {
            return DefaultStartOpacity + index * DefaultOpacityStep;
        }

        private T invoke<T>(Func<T> func) { return func(); }

        public static Bitmap RotateByAngle(Bitmap input, double angle) {
            Bitmap img = new Bitmap(input.Width, input.Height, PixelFormat.Format32bppRgb);
            img.SetResolution(input.HorizontalResolution, input.VerticalResolution);
            Graphics graphics = Graphics.FromImage(img);
            try {
                graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, input.Width, input.Height);
                graphics.RotateTransform((float) angle);
                graphics.DrawImage(input, 0, 0);
            } finally {
                graphics.Dispose();
            }
            return img;
        } 

        private sealed class HoughLine {
            public int Size         { get; set; }
            public int Position     { get; set; }
            public double Opacity   { get; set; }
            public double Distance  { get; set; }
        }
    }
}
