using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageProcessing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ProcessBtn_Click(object sender, EventArgs e)
        {
            Image inputimage = InputImagePb.Image;
            Image outputimage = ProcessImage.Process(inputimage);
            OutputImagePb.Image = outputimage;
        }

        private void ConvertBtn_Click(object sender, EventArgs e)
        {
//            Image im1 = InputImagePb.Image;
//
//            var input = new Bitmap(im1);
//            
//            var output = new Bitmap(im1.Width, im1.Height);
//
//            for (int x = 0; x < im1.Width; x++)
//                for (int y = 0; y < im1.Height; y++)
//                {
//                    int a = input.GetPixel(x,y).ToArgb();
//
//                    output.SetPixel(x, y, Color.FromArgb(a, a, a));
//                }


        }



    }
}
