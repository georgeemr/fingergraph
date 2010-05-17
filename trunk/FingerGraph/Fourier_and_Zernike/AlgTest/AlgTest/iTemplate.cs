using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Exocortex.DSP;
using DescriptionExtractor;
using ImageFilterApp;

/*
 * TODO : Increase speed of creating template
 */
namespace AlgTest
{
    class iTemplate
    {
        #region Very slow, but very powerful template generator and matcher
        const int zerSize = 256;
        private double[] zerCoefficients;

        public iTemplate()
        { 
        }

        public iTemplate(Bitmap myImage)
        {
            //BitmapFilter.GrayScale(myImage);
            BitmapFilter.FixHistogram(ref myImage);
         //   myImage.Save("FixedHistogram.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapFilter.Binarize(ref myImage, 110);
        //    myImage.Save("Binarized.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            //BitmapFilter.GaussianBlur(myImage, 1);


            /******************************************************/
            CImage cimage = new CImage(myImage);
            float scale = 1f / (float)Math.Sqrt(cimage.Width * cimage.Height);
            ComplexF[] data = cimage.Data;

            int offset = 0;
            for (int y = 0; y < cimage.Height; y++)
            {
                for (int x = 0; x < cimage.Width; x++)
                {
                    if (((x + y) & 0x1) != 0)
                    {
                        data[offset] *= -1;
                    }
                    offset++;
                }
            }

            Fourier.FFT2(data, cimage.Width, cimage.Height, FourierDirection.Forward);

            cimage.FrequencySpace = true;

            for (int i = 0; i < data.Length; i++)
            {
                data[i] *= scale;
            }

            myImage = cimage.ToBitmap();

     //       myImage.Save("FFT.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            //pictureBoxFiltred.Size = new System.Drawing.Size(myImage.Width, myImage.Height);
            /******************************************************/

            
            Bitmap zerImage = new Bitmap(zerSize, zerSize);
            int ow, oh;

            ow = myImage.Width / 2 - zerSize / 2;
            oh = myImage.Height / 2 - zerSize / 2;

            for (int y = 0; y < zerSize; y++)
            {
                for (int x = 0; x < zerSize; x++)
                {
                    zerImage.SetPixel(x, y, myImage.GetPixel(x + ow, y + oh));
                }
            }

            ZernikeDesc zernikeDesc = new ZernikeDesc(zerImage);
            zerCoefficients = zernikeDesc.Process();
        }

        public static double Match(iTemplate a, iTemplate b)
        {
            double[] tmp = new double[a.zerCoefficients.Length];

            for (int i = 0; i < a.zerCoefficients.Length; i++)
            {
                tmp[i] = a.zerCoefficients[i] - b.zerCoefficients[i];
            }

            return MathUtils.GetAverage(tmp);
        }
        #endregion
    }
}
