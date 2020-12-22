using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV.OCR; //модуль оптического распознавания символов
using Emgu.CV.Features2D;

namespace Лабораторная_2
{
    class Func
    {
        public Image<Bgr, byte> baseImage; //глобальная переменная
        public Image<Bgr, byte> twistedImage; //глобальная переменная

        public void Source1(string fileName)
        {
            baseImage = new Image<Bgr, byte>(fileName);
        }

        public void Source2(string fileName)
        {
            twistedImage = new Image<Bgr, byte>(fileName);
        }

        public Image<Bgr, byte> GFTT()
        {
            GFTTDetector detector = new GFTTDetector(40, 0.01, 5, 3, true);            MKeyPoint[] GFP1 = detector.Detect(baseImage.Convert<Gray, byte>().Mat);

            //создание массива характерных точек исходного изображения (только позиции)
            PointF[] srcPoints = new PointF[GFP1.Length];
            for (int i = 0; i < GFP1.Length; i++)
                srcPoints[i] = GFP1[i].Point;
            PointF[] destPoints; //массив для хранения позиций точек на изменённом изображении
            byte[] status; //статус точек (найдены/не найдены)
            float[] trackErrors; //ошибки
                                 //вычисление позиций характерных точек на новом изображении методом Лукаса-Канаде
            CvInvoke.CalcOpticalFlowPyrLK(
             baseImage.Convert<Gray, byte>().Mat, //исходное изображение
             twistedImage.Convert<Gray, byte>().Mat,//изменённое изображение
             srcPoints, //массив характерных точек исходного изображения
             new Size(20, 20), //размер окна поиска
             5, //уровни пирамиды
             new MCvTermCriteria(20, 1), //условие остановки вычисления оптического потока
             out destPoints, //позиции характерных точек на новом изображении
             out status, //содержит 1 в элементах, для которых поток был найден
             out trackErrors //содержит ошибки
             );            var output = baseImage.Clone();

            foreach (MKeyPoint p in GFP1)
            {
                CvInvoke.Circle(output, Point.Round(p.Point), 3, new Bgr(Color.Blue).MCvScalar, 2);
            }

            var output2 = twistedImage.Clone();

            foreach (MKeyPoint p in GFP1)
            {
                CvInvoke.Circle(output2, Point.Round(p.Point), 3, new Bgr(Color.Blue).MCvScalar, 2);
            }
            return output.Resize(640, 480, Inter.Linear);
        }
    }
}