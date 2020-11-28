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

namespace Лабораторная_2
{
    class Func
    {
        public VideoCapture capture;
        CascadeClassifier face;
        Mat image = new Mat();
        Image<Bgr, byte> input;
        Mat frame;

        public Image<Bgr, byte> sourceImage; //глобальная переменная
        public List<Rectangle> rois = new List<Rectangle>();

        public void Source(string fileName)
        {
            sourceImage = new Image<Bgr, byte>(fileName);

        }

        public Image<Bgr, byte> Binarization()
        {
            var resultImage = sourceImage.CopyBlank();

            var grayImage = sourceImage.Convert<Gray, byte>();
            grayImage._ThresholdBinaryInv(new Gray(100), new Gray(255));
            grayImage._Dilate(5);


            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(grayImage, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

            var output = sourceImage.Copy();
            for (int i = 0; i < contours.Size; i++)
            {
                if (CvInvoke.ContourArea(contours[i], false) > 50) //игнорирование маленьких контуров
                {
                    Rectangle rect = CvInvoke.BoundingRectangle(contours[i]);
                    rois.Add(rect);
                    output.Draw(rect, new Bgr(Color.Blue), 1);
                }
            }


            //input.ROI = rect; //установка региона интереса
            //Image<Bgr, byte> roiImg;
            //roiImg = sourceImage.Clone(); //копирование участка изображения, содержащего текст
            //sourceImage.ROI = System.Drawing.Rectangle.Empty;

            resultImage = output.Convert<Bgr, byte>();

            return resultImage;
        }

        public Image<Bgr, byte> ROI(int idx)
        {
            var resultImage = sourceImage.Copy();
            Rectangle rect = rois[idx];
            resultImage.Draw(rect, new Bgr(Color.Blue), 1);
            return resultImage;
        }

        public Image<Bgr, byte> ROIOut(int idx)
        {
            var resultImage = sourceImage.Copy();
            resultImage.ROI = rois[idx];

            return resultImage;
        }


        public String Translate(Image<Bgr, byte> roiImg, string language)
        {
            Tesseract _ocr = new Tesseract("D:\\AOIClab5", language, OcrEngineMode.TesseractLstmCombined);

            _ocr.SetImage(roiImg); //фрагмент изображения, содержащий текст
            _ocr.Recognize(); //распознание текста
            Tesseract.Character[] words = _ocr.GetCharacters(); //получение найденных символов

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < words.Length; i++)
            {
                strBuilder.Append(words[i].Text);
            }

            return strBuilder.ToString();

        }





        public void Web()
        {
            // инициализация веб-камеры
            capture = new VideoCapture();
            capture.ImageGrabbed += ProcessFrame;
            capture.Start();
            //return capture;
        }

        public void ProcessFrame(object sender, EventArgs e)
        {
            capture.Retrieve(image);

            input = image.ToImage<Bgr, byte>();

            List<Rectangle> faces = new List<Rectangle>();

                //Mat ugray = new Mat();
                //CvInvoke.CvtColor(image, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                //Rectangle[] facesDetected = face.DetectMultiScale(ugray, 1.1, 10, new Size(20, 20));
                //faces.AddRange(facesDetected);

            Image<Bgra, byte> res = input.Convert<Bgra, byte>();

            using (CascadeClassifier face = new CascadeClassifier("D:\\AOIClab5\\haarcascade_frontalface_default.xml"))
            {
                using (Mat ugray = new Mat())
                {
                    CvInvoke.CvtColor(image, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                    Rectangle[] facesDetected = face.DetectMultiScale(ugray, 1.1, 10, new Size(20, 20));
                    faces.AddRange(facesDetected);
                }
            }
            foreach (Rectangle rect in faces)
                input.Draw(rect, new Bgr(Color.Yellow), 2);


            foreach (Rectangle rect in faces) //для каждого лица
                {
                    res.ROI = rect; //для области содержащей лицо
                    Image<Bgra, byte> small = frame.ToImage<Bgra, byte>().Resize(rect.Width, rect.Height, Inter.Nearest); //создание
                                                                                                                          //копирование изображения small на изображение res с использованием маски копирования mask
                    CvInvoke.cvCopy(small, res, small.Split()[3]);
                    res.ROI = System.Drawing.Rectangle.Empty;
                }

               // capture = res;
        }

        public Mat loadFace()
        {
            face = new CascadeClassifier("D:\\AOIClab5\\haarcascade_frontalface_default.xml");

            OpenFileDialog f = new OpenFileDialog();
            f.ShowDialog();

            frame = CvInvoke.Imread(f.FileName, ImreadModes.Unchanged);

            return frame.Split()[3];
        }
    }
}