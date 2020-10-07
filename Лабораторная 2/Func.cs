using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Лабораторная_2
{
    class Func
    {
        public Image<Bgr, byte> sourceImage; //глобальная переменная
        public Image<Bgr, byte> secondImage;
        public Image<Bgr, byte> timeImage;

        public void Source(string fileName)
        {
            sourceImage = new Image<Bgr, byte>(fileName).Resize(640, 480, Inter.Linear);

        }

        public Image<Bgr, byte> Blue()
        {
            var channel = sourceImage.Split()[2];

            Image<Bgr, byte> destImage = sourceImage.CopyBlank();

            VectorOfMat vm = new VectorOfMat();

            vm.Push(channel.CopyBlank());
            vm.Push(channel.CopyBlank());
            vm.Push(channel);

            CvInvoke.Merge(vm, destImage);
            return destImage;
        }

        public Image<Bgr, byte> Green()
        {
            var channel = sourceImage.Split()[1];

            Image<Bgr, byte> destImage = sourceImage.CopyBlank();

            VectorOfMat vm = new VectorOfMat();

            vm.Push(channel.CopyBlank());
            vm.Push(channel);
            vm.Push(channel.CopyBlank());

            CvInvoke.Merge(vm, destImage);
            return destImage;
        }

        public Image<Bgr, byte> Red()
        {
            var channel = sourceImage.Split()[0];

            Image<Bgr, byte> destImage = sourceImage.CopyBlank();

            VectorOfMat vm = new VectorOfMat();

            vm.Push(channel);
            vm.Push(channel.CopyBlank());
            vm.Push(channel.CopyBlank());

            CvInvoke.Merge(vm, destImage);
            return destImage;
        }

        public Image<Gray, byte> Mono()
        {
            var grayImage = new Image<Gray, byte>(sourceImage.Size);

            for (int y = 0; y < grayImage.Height; y++)
            {
                for (int x = 0; x < grayImage.Width; x++)
                {
                    grayImage.Data[y, x, 0] = Convert.ToByte(0.299 * sourceImage.Data[y, x, 2] + 0.587 * sourceImage.Data[y, x, 1] + 0.114 * sourceImage.Data[y, x, 0]);
                }
            }
            return grayImage;
        }

        private int chek(int chan)
        {
            if (chan > 255) { chan = 255; }
            else if (chan < 0) { chan = 0; }
            return chan;
        }

        private double chek(double chan)
        {
            if (chan > 255) { chan = 255; }
            else if (chan < 0) { chan = 0; }
            return chan;
        }

        public Image<Bgr, byte> Sepia()
        {
            var sepImage = new Image<Bgr, byte>(sourceImage.Size);

            for (int y = 0; y < sepImage.Height; y++)
            {
                for (int x = 0; x < sepImage.Width; x++)
                {
                    sepImage.Data[y, x, 0] = Convert.ToByte(chek(0.272 * sourceImage.Data[y, x, 2] + 0.534 * sourceImage.Data[y, x, 1] + 0.131 * sourceImage.Data[y, x, 0]));
                    sepImage.Data[y, x, 1] = Convert.ToByte(chek(0.349 * sourceImage.Data[y, x, 2] + 0.686 * sourceImage.Data[y, x, 1] + 0.168 * sourceImage.Data[y, x, 0]));
                    sepImage.Data[y, x, 2] = Convert.ToByte(chek(0.393 * sourceImage.Data[y, x, 2] + 0.769 * sourceImage.Data[y, x, 1] + 0.189 * sourceImage.Data[y, x, 0]));
                }
            }
            return sepImage;
        }

        public Image<Bgr, byte> Bridght(int Bridghtness, double Contr)
        {
            Image<Bgr, byte> result = sourceImage.Convert<Bgr, byte>();
            
            for (int chanel = 0; chanel < 3; chanel++)
            {
                for (int y = 0; y < result.Height; y++)
                {
                    for (int x = 0; x < result.Width; x++)
                    {
                        result.Data[y, x, chanel] = Convert.ToByte(chek(sourceImage.Data[y, x, chanel] * (Contr / 10) + Bridghtness));
                    }
                }
            }
            return result;
        }

        public void secondImg(string fileName)
        {
            secondImage = new Image<Bgr, byte>(fileName).Resize(640, 480, Inter.Linear);
        }

        public Image<Bgr, byte> Colapse(double a, Image<Bgr, byte> fil = null)
        {
            var image = (fil != null) ? fil : sourceImage;
            a = a / 10;
            Image<Bgr, byte> result = image.CopyBlank();

            for (int chanel = 0; chanel < 3; chanel++)
            {
                for (int y = 0; y < result.Height; y++)
                {
                    for (int x = 0; x < result.Width; x++)
                    {
                        result.Data[y, x, chanel] = Convert.ToByte(chek(image.Data[y, x, chanel] * a + secondImage.Data[y, x, chanel] * (1 - a)));
                    }
                }
            }
            return result;
        }
        public Image<Bgr, byte> AntiColapse(double a)
        {
            a = a / 10;
            Image<Bgr, byte> result = sourceImage.CopyBlank();

            for (int chanel = 0; chanel < 3; chanel++)
            {
                for (int y = 0; y < result.Height; y++)
                {
                    for (int x = 0; x < result.Width; x++)
                    {
                        result.Data[y, x, chanel] = Convert.ToByte(chek(sourceImage.Data[y, x, chanel] * a - secondImage.Data[y, x, chanel] * (1 - a)));
                    }
                }
            }
            return result;
        }

        public Image<Bgr, byte> What<T>(int a, Image<T, byte> fil = null) where T : struct, IColor
        {
            Image<Bgr, byte> result = sourceImage.CopyBlank();

            for (int chanel = 0; chanel < sourceImage.NumberOfChannels; chanel++)
            {
                int fil_channel = (chanel < fil.NumberOfChannels) ? chanel : fil.NumberOfChannels - 1;

                for (int y = 0; y < result.Height; y++)
                {
                    for (int x = 0; x < result.Width; x++)
                    {
                        result.Data[y, x, chanel] = Convert.ToByte(chek(sourceImage.Data[y, x, chanel] & fil.Data[y, x, fil_channel]));
                    }
                }
            }
            return result;
        }

        public Image<Bgr, byte> What(int a)
        {
            return What(a, secondImage);
        }

        public Image<Bgr, byte> Sharp()
        {
            
            int[,] w = new int[3, 3]
            {
                {-1,-1,-1},
                {-1, 9,-1},
                {-1,-1,-1}
            };
            Image<Bgr, byte> result3 = windowfilter(w);

            return result3;
        }

        public Image<Bgr, byte> Embos()
        {
            int[,] w = new int[3, 3]
            {
                {-4,-2, 0},
                {-2, 1, 2},
                { 0, 2, 4}
            };
            Image<Bgr, byte> result3 = windowfilter(w);

            return result3;
        }

        public Image<Bgr, byte> Grani()
        {
            int[,] w = new int[3, 3]
            {
                { 0,-2, 0},
                {-2, 4, 0},
                { 0, 0, 0}
            };

            Image<Bgr, byte> result3 = windowfilter(w);
            
            return result3;
        }

        public Image<Bgr, byte> Filter(int f1, int f2, int f3, int f4, int f5, int f6, int f7, int f8, int f9)
        {
            int[,] w = new int[3, 3]
            {
                {f1,f2,f3},
                {f4,f5,f6},
                {f7,f8,f9}
            };

            Image<Bgr, byte> result3 = windowfilter(w);

            return result3;
        }

        public Image<Bgr, byte> windowfilter(int [,]w)
        {
            Image<Bgr, byte> result = sourceImage.Convert<Bgr, byte>();
            Image<Bgr, byte> result2 = result.CopyBlank();

            for (int chanel = 0; chanel < 3; chanel++)
            {
                for (int y = 1; y < result.Height - 1; y++)
                {
                    for (int x = 1; x < result.Width - 1; x++)
                    {
                        int a = 0;
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                a += result.Data[i + y, i + x, chanel] * w[i + 1, j + 1];
                            }
                        }
                        result2.Data[y, x, chanel] = Convert.ToByte(chek(a));
                    }
                }
            }
            return result2;
        }

        public Image<Hsv, byte> HSV(int H)
        {
            Image<Hsv, byte> hsvImage = sourceImage.Convert<Hsv, byte>();

            for (int y = 0; y < hsvImage.Height; y++)
            {
                for (int x = 0; x < hsvImage.Width; x++)
                {
                    
                    hsvImage.Data[y, x, 0] = Convert.ToByte(chek(H));
                    //hsvImage.Data[y, x, 1] = Convert.ToByte(chek(S));
                    //hsvImage.Data[y, x, 2] = Convert.ToByte(chek(V));
                        
                   
                }
            }

            return hsvImage;
        }

        public Image<T, byte> Blur<T>(Image<T, byte> fil = null) where T : struct, IColor
        {
            Image<T, byte> result = fil.CopyBlank();

            List<byte> window = new List<byte>();
            int k = 7;

            for (int chanel = 0; chanel < fil.NumberOfChannels; chanel++)
            {
                for (int y = k/2; y < fil.Height - k/2; y++)
                {
                    for (int x = k/2; x < fil.Width - k/2; x++)
                    {
                        window.Clear();
                        for (int i = -k / 2; i <= k / 2; i++)
                        {
                            for (int j = -k / 2; j <= k / 2; j++)
                            {
                                window.Add(fil.Data[i + y, j + x, chanel]);
                            }
                        }
                        window.Sort();

                        result.Data[y, x, chanel] = window[window.Count / 2];
                    }
                }
            }
            return result;
        }

        public Image<Bgr, byte> Blur()
        {
            return Blur(sourceImage);
        }

        public Image<Bgr, byte> Aqwa(int Bridghtness, double Contr, int a)
        {
            Image<Bgr, byte> result = Bridght(Bridghtness, Contr);
            result = Blur(result);
            result = Colapse(a, result);

            return result;
        }


        public Image<Bgr, byte> cartoon(int a)
        {
            Image<Gray, byte> result = Mono();
            result = Blur(result);

            var edges = result.Convert<Gray, byte>();
            edges = edges.ThresholdAdaptive(new Gray(230), AdaptiveThresholdType.MeanC, ThresholdType.Binary, 3, new Gray(0.03));

            var result2 = What(a, edges);

            return result2;
        }
    }
}
