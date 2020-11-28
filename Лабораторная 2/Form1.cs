using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Лабораторная_2
{
    public partial class Form1 : Form
    {
        public VideoCapture capture;
        CascadeClassifier face;
        Mat image = new Mat();
        Image<Bgr, byte> input;
        Mat frame;
        
        private Func func = new Func();

        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла
            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = openFileDialog.FileName;
                func.Source(fileName);

                imageBox1.Image = func.sourceImage;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            imageBox2.Image = func.Binarization();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            imageBox2.Image = func.Binarization();
            for (int i = 0; i < func.rois.Count; i++)
            {
                int k = i + 1;
                listBox2.Items.Add("Область " + k.ToString());
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            imageBox2.Image = func.ROI(listBox2.SelectedIndex);
            imageBox3.Image = func.ROIOut(listBox2.SelectedIndex);
            label1.Text = func.Translate(func.ROIOut(listBox2.SelectedIndex), "eng");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            capture = new VideoCapture();
            capture.ImageGrabbed += ProcessFrame;
            capture.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            face = new CascadeClassifier("D:\\AOIClab5\\haarcascade_frontalface_default.xml");

            OpenFileDialog f = new OpenFileDialog();
            f.ShowDialog();

            frame = CvInvoke.Imread(f.FileName, ImreadModes.Unchanged);

            imageBox2.Image = frame.Split()[3];
        }

        public void ProcessFrame(object sender, EventArgs e)
        {
            capture.Retrieve(image);

            input = image.ToImage<Bgr, byte>();

            List<Rectangle> faces = new List<Rectangle>();
            
            Image<Bgra, byte> res = input.Convert<Bgra, byte>();
            
            using (Mat ugray = new Mat())
            {
                    CvInvoke.CvtColor(image, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                    Rectangle[] facesDetected = face.DetectMultiScale(ugray, 1.1, 10, new Size(10, 10));
                    faces.AddRange(facesDetected);
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

           imageBox1.Image  = res;
        }
    }
}
