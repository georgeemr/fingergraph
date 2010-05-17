using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace AlgTest
{
    public partial class Form1 : Form
    {
        #region Don't look here!
        #region Which part of "Don't look here!" you don't understood?
        #region You have been warned!
        Bitmap myImage1, myImage2;

        public Form1()
        {
            InitializeComponent();
        }

      
        private void buttonTest_Click(object sender, EventArgs e)
        {
            if (openFileDialogImg.ShowDialog() == DialogResult.OK)
            {
                pictureBoxIn.Image = Image.FromFile(openFileDialogImg.FileName);
                myImage1 = new Bitmap(pictureBoxIn.Image);

                pictureBoxIn.Size = new Size(pictureBoxIn.Image.Width, pictureBoxIn.Image.Height);
                pictureBoxFiltred.Size = pictureBoxIn.Size;

                pictureBoxFiltred.Location = new Point(pictureBoxIn.Location.X + pictureBoxIn.Image.Width,
                                                       pictureBoxIn.Location.Y);
                buttonTest.Location = new Point(pictureBoxIn.Location.X, pictureBoxIn.Location.Y + pictureBoxIn.Image.Height);
                pictureBoxFiltred.Image = myImage1;
            }
        }        

        private void pictureBoxIn_Click(object sender, EventArgs e)
        {
            iTemplate template1 = new iTemplate(myImage1);
            iTemplate template2 = new iTemplate(myImage2);

            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //for (int i = 0; i < 100; i++ )
             double res =  iTemplate.Match(template1, template2);
            //sw.Stop();

            //MessageBox.Show(String.Format("{0} ms", sw.ElapsedMilliseconds), "Time");
             MessageBox.Show(String.Format("{0}", res), "Matching");

           // pictureBoxFiltred.Size = new System.Drawing.Size(myImage.Width, myImage.Height);

            //pictureBoxFiltred.Image = myImage;
            //pictureBoxFiltred.Refresh();            
        }

        private void testBtn2_Click(object sender, EventArgs e)
        {
            if (openFileDialogImg.ShowDialog() == DialogResult.OK)
            {
                pictureBoxFiltred.Image = Image.FromFile(openFileDialogImg.FileName);
                myImage2 = new Bitmap(pictureBoxFiltred.Image);

                testBtn2.Location = new Point(pictureBoxFiltred.Location.X, pictureBoxFiltred.Location.Y + pictureBoxFiltred.Image.Height);
                pictureBoxFiltred.Image = myImage2;
            }
        }
        #endregion
        #endregion
        #endregion
    }
}