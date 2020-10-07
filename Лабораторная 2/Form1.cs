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
            imageBox2.Image = func.Blue();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            imageBox2.Image = func.Green();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            imageBox2.Image = func.Red();
        }

        private void button5_Click(object sender, EventArgs e)
        {
           
            imageBox2.Image = func.Mono();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            imageBox2.Image = func.Sepia();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            imageBox2.Image = func.Bridght(trackBar1.Value, trackBar2.Value);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            imageBox2.Image = func.Bridght(trackBar1.Value, trackBar2.Value);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла
            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = openFileDialog.FileName;
                func.secondImg(fileName);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            imageBox2.Image = func.Colapse(trackBar3.Value);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            imageBox2.Image = func.AntiColapse(trackBar3.Value);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            imageBox2.Image = func.What(trackBar3.Value);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            imageBox2.Image = func.Sharp();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            imageBox2.Image = func.Embos();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            imageBox2.Image = func.Grani();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            imageBox2.Image = func.HSV(trackBar4.Value);

        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            imageBox2.Image = func.HSV(trackBar4.Value);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            imageBox2.Image = func.Blur();
        }

        private void button16_Click(object sender, EventArgs e)
        {

            int f1 = Convert.ToInt32(textBox1.Text);
            int f2 = Convert.ToInt32(textBox2.Text);
            int f3 = Convert.ToInt32(textBox3.Text);
            int f4 = Convert.ToInt32(textBox4.Text);
            int f5 = Convert.ToInt32(textBox5.Text);
            int f6 = Convert.ToInt32(textBox6.Text);
            int f7 = Convert.ToInt32(textBox7.Text);
            int f8 = Convert.ToInt32(textBox8.Text);
            int f9 = Convert.ToInt32(textBox9.Text);

            imageBox2.Image = func.Filter(f1, f2, f3, f4, f5, f6, f7, f8, f9);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            imageBox2.Image = func.Aqwa(trackBar1.Value, trackBar2.Value, trackBar3.Value);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            imageBox2.Image = func.cartoon(trackBar3.Value);
        }
    }
}
