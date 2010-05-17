using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace AlgTest
{
    /*
     * TODO : Rewrite some filters with LockBits
     */

    public class ConvMatrix
	{
		public int TopLeft = 0, TopMid = 0, TopRight = 0;
		public int MidLeft = 0, Pixel = 1, MidRight = 0;
		public int BottomLeft = 0, BottomMid = 0, BottomRight = 0;
		public int Factor = 1;
		public int Offset = 0;
		public void SetAll(int nVal)
		{
			TopLeft = TopMid = TopRight = MidLeft = Pixel = MidRight = BottomLeft = BottomMid = BottomRight = nVal;
		}
	}

	public class BitmapFilter
	{
        private static int pMin = int.MaxValue;
        private static int pMax = int.MinValue; 

        public static void Binarize(ref Bitmap b, int threshold)
        {
            Bitmap temp = new Bitmap(b);

            for (int y = 0; y < b.Height; y++)
                for (int x = 0; x < b.Width; x++)
                {
                    if (b.GetPixel(x, y).R >= threshold)
                        temp.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    else
                        temp.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                }

            b = temp;
        }

        public static void FixHistogram(ref Bitmap b)
        {
            Bitmap temp = new Bitmap(b);
            Color col;
            int mid;

            GetMinMax(b);

            for (int y = 0; y < b.Height; y++)
                for (int x = 0; x < b.Width; x++)
                {
                    col = temp.GetPixel(x, y);

                    if (col.R <= pMin)
                        temp.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    else if (col.R >= pMax)
                        temp.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    else
                    {
                        mid = 255 * (col.R - pMin) / (pMax - pMin);
                        temp.SetPixel(x, y, Color.FromArgb(mid, mid, mid));
                    }
                }

            b = temp;
        }

        private static void GetMinMax(Bitmap gs)
        {
            Color col;

            for (int y = 0; y < gs.Height; y++)
                for (int x = 0; x < gs.Width; x++)
                {
                    col = gs.GetPixel(x, y);

                    if (col.R < pMin)
                        pMin = col.R;
                    if (col.R > pMax)
                        pMax = col.R;
                }
        }

		public static bool Invert(Bitmap b)
		{
			// GDI+ still lies to us - the return format is BGR, NOT RGB.
			BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;

				int nOffset = stride - b.Width*3;
				int nWidth = b.Width * 3;
	
				for(int y=0;y<b.Height;++y)
				{
					for(int x=0; x < nWidth; ++x )
					{
						p[0] = (byte)(255-p[0]);
						++p;
					}
					p += nOffset;
				}
			}

			b.UnlockBits(bmData);

			return true;
		}

		public static bool GrayScale(Bitmap b)
		{
			// GDI+ still lies to us - the return format is BGR, NOT RGB.
			BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;

				int nOffset = stride - b.Width*3;

				byte red, green, blue;
	
				for(int y=0;y<b.Height;++y)
				{
					for(int x=0; x < b.Width; ++x )
					{
						blue = p[0];
						green = p[1];
						red = p[2];

						p[0] = p[1] = p[2] = (byte)(.299 * red + .587 * green + .114 * blue);

						p += 3;
					}
					p += nOffset;
				}
			}

			b.UnlockBits(bmData);

			return true;
		}

		public static bool Brightness(Bitmap b, int nBrightness)
		{
			if (nBrightness < -255 || nBrightness > 255)
				return false;

			// GDI+ still lies to us - the return format is BGR, NOT RGB.
			BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;

			int nVal = 0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;

				int nOffset = stride - b.Width*3;
				int nWidth = b.Width * 3;

				for(int y=0;y<b.Height;++y)
				{
					for(int x=0; x < nWidth; ++x )
					{
						nVal = (int) (p[0] + nBrightness);
		
						if (nVal < 0) nVal = 0;
						if (nVal > 255) nVal = 255;

						p[0] = (byte)nVal;

						++p;
					}
					p += nOffset;
				}
			}

			b.UnlockBits(bmData);

			return true;
		}

		public static bool Contrast(Bitmap b, sbyte nContrast)
		{
			if (nContrast < -100) return false;
			if (nContrast >  100) return false;

			double pixel = 0, contrast = (100.0+nContrast)/100.0;

			contrast *= contrast;

			int red, green, blue;
			
			// GDI+ still lies to us - the return format is BGR, NOT RGB.
			BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;

				int nOffset = stride - b.Width*3;

				for(int y=0;y<b.Height;++y)
				{
					for(int x=0; x < b.Width; ++x )
					{
						blue = p[0];
						green = p[1];
						red = p[2];
				
						pixel = red/255.0;
						pixel -= 0.5;
						pixel *= contrast;
						pixel += 0.5;
						pixel *= 255;
						if (pixel < 0) pixel = 0;
						if (pixel > 255) pixel = 255;
						p[2] = (byte) pixel;

						pixel = green/255.0;
						pixel -= 0.5;
						pixel *= contrast;
						pixel += 0.5;
						pixel *= 255;
						if (pixel < 0) pixel = 0;
						if (pixel > 255) pixel = 255;
						p[1] = (byte) pixel;

						pixel = blue/255.0;
						pixel -= 0.5;
						pixel *= contrast;
						pixel += 0.5;
						pixel *= 255;
						if (pixel < 0) pixel = 0;
						if (pixel > 255) pixel = 255;
						p[0] = (byte) pixel;					

						p += 3;
					}
					p += nOffset;
				}
			}

			b.UnlockBits(bmData);

			return true;
		}
	
		public static bool Gamma(Bitmap b, double red, double green, double blue)
		{
			if (red < .2 || red > 5) return false;
			if (green < .2 || green > 5) return false;
			if (blue < .2 || blue > 5) return false;

			byte [] redGamma = new byte [256];
			byte [] greenGamma = new byte [256];
			byte [] blueGamma = new byte [256];

			for (int i = 0; i< 256; ++i)
			{
				redGamma[i] = (byte)Math.Min(255, (int)(( 255.0 * Math.Pow(i/255.0, 1.0/red)) + 0.5));
				greenGamma[i] = (byte)Math.Min(255, (int)(( 255.0 * Math.Pow(i/255.0, 1.0/green)) + 0.5));
				blueGamma[i] = (byte)Math.Min(255, (int)(( 255.0 * Math.Pow(i/255.0, 1.0/blue)) + 0.5));
			}

			// GDI+ still lies to us - the return format is BGR, NOT RGB.
			BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;

				int nOffset = stride - b.Width*3;

				for(int y=0;y<b.Height;++y)
				{
					for(int x=0; x < b.Width; ++x )
					{
						p[2] = redGamma[ p[2] ];
						p[1] = greenGamma[ p[1] ];
						p[0] = blueGamma[ p[0] ];

						p += 3;
					}
					p += nOffset;
				}
			}

			b.UnlockBits(bmData);

			return true;
		}

		/*public static bool Color(Bitmap b, int red, int green, int blue)
		{
			if (red < -255 || red > 255) return false;
			if (green < -255 || green > 255) return false;
			if (blue < -255 || blue > 255) return false;

			// GDI+ still lies to us - the return format is BGR, NOT RGB.
			BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;

				int nOffset = stride - b.Width*3;
				int nPixel;

				for(int y=0;y<b.Height;++y)
				{
					for(int x=0; x < b.Width; ++x )
					{
						nPixel = p[2] + red;
						nPixel = Math.Max(nPixel, 0);
						p[2] = (byte)Math.Min(255, nPixel);

						nPixel = p[1] + green;
						nPixel = Math.Max(nPixel, 0);
						p[1] = (byte)Math.Min(255, nPixel);

						nPixel = p[0] + blue;
						nPixel = Math.Max(nPixel, 0);
						p[0] = (byte)Math.Min(255, nPixel);

						p += 3;
					}
					p += nOffset;
				}
			}

			b.UnlockBits(bmData);

			return true;
		}*/

		public static bool Conv3x3(Bitmap b, ConvMatrix m)
		{
			// Avoid divide by zero errors
			if (0 == m.Factor) return false;

			Bitmap bSrc = (Bitmap)b.Clone(); 

			// GDI+ still lies to us - the return format is BGR, NOT RGB.
			BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			int stride2 = stride * 2;
			System.IntPtr Scan0 = bmData.Scan0;
			System.IntPtr SrcScan0 = bmSrc.Scan0;

			unsafe
			{
				byte * p = (byte *)(void *)Scan0;
				byte * pSrc = (byte *)(void *)SrcScan0;

				int nOffset = stride + 6 - b.Width*3;
				int nWidth = b.Width - 2;
				int nHeight = b.Height - 2;

				int nPixel;

				for(int y=0;y < nHeight;++y)
				{
					for(int x=0; x < nWidth; ++x )
					{
						nPixel = ( ( ( (pSrc[2] * m.TopLeft) + (pSrc[5] * m.TopMid) + (pSrc[8] * m.TopRight) +
							(pSrc[2 + stride] * m.MidLeft) + (pSrc[5 + stride] * m.Pixel) + (pSrc[8 + stride] * m.MidRight) +
							(pSrc[2 + stride2] * m.BottomLeft) + (pSrc[5 + stride2] * m.BottomMid) + (pSrc[8 + stride2] * m.BottomRight)) / m.Factor) + m.Offset); 

						if (nPixel < 0) nPixel = 0;
						if (nPixel > 255) nPixel = 255;

						p[5 + stride]= (byte)nPixel;

						nPixel = ( ( ( (pSrc[1] * m.TopLeft) + (pSrc[4] * m.TopMid) + (pSrc[7] * m.TopRight) +
							(pSrc[1 + stride] * m.MidLeft) + (pSrc[4 + stride] * m.Pixel) + (pSrc[7 + stride] * m.MidRight) +
							(pSrc[1 + stride2] * m.BottomLeft) + (pSrc[4 + stride2] * m.BottomMid) + (pSrc[7 + stride2] * m.BottomRight)) / m.Factor) + m.Offset); 

						if (nPixel < 0) nPixel = 0;
						if (nPixel > 255) nPixel = 255;
							
						p[4 + stride] = (byte)nPixel;

						nPixel = ( ( ( (pSrc[0] * m.TopLeft) + (pSrc[3] * m.TopMid) + (pSrc[6] * m.TopRight) +
							(pSrc[0 + stride] * m.MidLeft) + (pSrc[3 + stride] * m.Pixel) + (pSrc[6 + stride] * m.MidRight) +
							(pSrc[0 + stride2] * m.BottomLeft) + (pSrc[3 + stride2] * m.BottomMid) + (pSrc[6 + stride2] * m.BottomRight)) / m.Factor) + m.Offset); 

						if (nPixel < 0) nPixel = 0;
						if (nPixel > 255) nPixel = 255;

						p[3 + stride] = (byte)nPixel;

						p += 3;
						pSrc += 3;
					}

					p += nOffset;
					pSrc += nOffset;
				}
			}

			b.UnlockBits(bmData);
			bSrc.UnlockBits(bmSrc);

			return true;
		}
		public static bool Smooth(Bitmap b, int nWeight /* default to 1 */)
		{
			ConvMatrix m = new ConvMatrix();
			m.SetAll(1);
			m.Pixel = nWeight;
			m.Factor = nWeight + 8;

			return  BitmapFilter.Conv3x3(b, m);
		}

		public static bool GaussianBlur(Bitmap b, int nWeight /* default to 4*/)
		{
			ConvMatrix m = new ConvMatrix();
			m.SetAll(1);
			m.Pixel = nWeight;
			m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 2;
			m.Factor = nWeight + 12;

			return  BitmapFilter.Conv3x3(b, m);
		}
		public static bool MeanRemoval(Bitmap b, int nWeight /* default to 9*/ )
		{
			ConvMatrix m = new ConvMatrix();
			m.SetAll(-1);
			m.Pixel = nWeight;
			m.Factor = nWeight - 8;

			return BitmapFilter.Conv3x3(b, m);
		}
		public static bool Sharpen(Bitmap b, int nWeight /* default to 11*/ )
		{
			ConvMatrix m = new ConvMatrix();
			m.SetAll(0);
			m.Pixel = nWeight;
			m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = -2;
			m.Factor = nWeight - 8;

			return  BitmapFilter.Conv3x3(b, m);
		}
		public static bool EmbossLaplacian(Bitmap b)
		{
			ConvMatrix m = new ConvMatrix();
			m.SetAll(-1);
			m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 0;
			m.Pixel = 4;
			m.Offset = 127;

			return  BitmapFilter.Conv3x3(b, m);
		}	
		public static bool EdgeDetectQuick(Bitmap b)
		{
			ConvMatrix m = new ConvMatrix();
			m.TopLeft = m.TopMid = m.TopRight = -1;
			m.MidLeft = m.Pixel = m.MidRight = 0;
			m.BottomLeft = m.BottomMid = m.BottomRight = 1;
		
			m.Offset = 127;

			return  BitmapFilter.Conv3x3(b, m);
        }

        #region NotYetAvaliable
        /*
       private double[,] gx, gy;
        private double maxGX = 1, maxGY = 1;

        private const int BLOCK_SIZE = 7;
        readonly int[,] Sy = new int[,] { {3, 10, 3}, 
                                          {0, 0, 0}, 
                                          {-3, -10, -3} };

        readonly int[,] Sx = new int[,] { { 3, 0, -3}, 
                                          { 10, 0, -10 }, 
                                          { 3, 0, -3} };

        private void Sobel(ref Bitmap b, int threshold)
        {          
            Bitmap temp = new Bitmap(b.Width, b.Height);

            angles = new double[b.Width, b.Height];
            gx = new double[b.Width, b.Height];
            gy = new double[b.Width, b.Height];
            double fx, fy;

            for (int x = 1; x < b.Width - 1; x++)
                for (int y = 1; y < b.Height - 1; y++)
                {
                    gy[x, y] = gx[x, y] = 0;

                    for (int i = -1; i <= 1; i++ )
                        for (int j = -1; j <= 1; j++)
                        {
                            gy[x, y] += Sy[i + 1, j + 1] * b.GetPixel(x + i,y + j).R;
                            gx[x, y] += Sx[i + 1, j + 1] * b.GetPixel(x + i, y + j).R;
                        }                   
                }

            for (int x = 1 + BLOCK_SIZE; x < b.Width - BLOCK_SIZE - 1; x++)
                for (int y = 1 + BLOCK_SIZE; y < b.Height - BLOCK_SIZE - 1; y++)
                {
                    fx = 0.0;
                    fy = 0.0;

                   for(long j=-BLOCK_SIZE; j<=BLOCK_SIZE; j++)
                   {
                       for(long i=-BLOCK_SIZE; i<=BLOCK_SIZE; i++)                       
                       {
                           fy += 2*gx[x + i, j + y]*gy[x + i, j + y];
                           fx += (gx[x + i, j + y]*gx[x + i, j + y]-gy[x + i, j + y]*gy[x + i, j + y]);
                       }
                   }


                   // to angle
                   if (fx > 0)
                       angles[x, y] = Math.Atan2(fy, fx) / 2 + 1.5707963;
                   else if (fx < 0 && fy > 0)
                       angles[x, y] = Math.Atan2(fy, fx) / 2 + 3.1415926;
                   else if (fx < 0 && fy < 0)
                       angles[x, y] = Math.Atan2(fy, fx) / 2;
                   else if (fy != 0 && fx == 0)
                       angles[x, y] = 1.5707963;
                }

            //b = temp.Clone(new RectangleF(0, 0, b.Width, b.Height), new System.Drawing.Imaging.PixelFormat());
        }
        */
        //private double[,] angles;
        /*  double[,] midx;
      double[,] midy;
      */
        /*  private void MidGrad(ref Bitmap b)
          {
              midx = new double[b.Width, b.Height];
              midy = new double[b.Width, b.Height];

              double midxt, midyt;

              const int size = 2;

              for (int y = 0; y < b.Height; y += size)
              {
                  for (int x = 0; x < b.Width; x += size)
                  {
                      midxt = midyt = 0;
                      for (int yy = y; yy < y + size && yy < b.Height; yy++)
                          for (int xx = x; xx < x + size && xx < b.Width; xx++)
                          {
                              midxt += gx[xx, yy];
                              midyt += gy[xx, yy];
                          }
                      for (int yy = y; yy < y + size && yy < b.Height; yy++)
                          for (int xx = x; xx < x + size && xx < b.Width; xx++)
                          {
                              midx[xx, yy] = midxt / (size * size);
                              midy[xx, yy] = midyt / (size * size);

                              angles[xx, yy] = Math.Atan2(midy[xx, yy], midx[xx, yy]);
                              while (angles[xx, yy] > Math.PI)
                                  angles[xx, yy] -= Math.PI;
                              while (angles[xx, yy] <= 0)
                                  angles[xx, yy] += Math.PI;

                              if (angles[xx, yy] >= Math.PI / 2.0)
                                  angles[xx, yy] = -angles[xx, yy] + Math.PI;
                          }
                  }
              }
          }*/
        /*
        private void DrawGrad(ref Bitmap b)
        {
            Bitmap temp = new Bitmap(b);
            int col;

            for (int y = 0; y < pictureBoxIn.Height; y++)
                for (int x = 0; x < pictureBoxIn.Width; x++)
                {
                  //  col = (int)(255 * Math.Abs(midx[x, y]) / maxGX);
                   // angles[x, y] = Math.Atan2(gy[x, y], gx[x, y]) + Math.PI / 2.0;
                    col = (int)(255 * Math.Abs(gx[x, y]) / maxGX);
                    temp.SetPixel(x, y, Color.FromArgb(col, col, col));

                }
            b = temp;
        }*/
        /*
        private void DrawGradY(ref Bitmap b)
        {
            Bitmap temp = new Bitmap(b);
            int col;

            for (int y = 0; y < pictureBoxIn.Height; y++)
                for (int x = 0; x < pictureBoxIn.Width; x++)
                {
                    //col = (int)(255 * Math.Abs(midy[x, y]) / maxGY);
                    col = (int)(255 * Math.Abs(gy[x, y]) / maxGY);
                    temp.SetPixel(x, y, Color.FromArgb(col, col, col));

                }
            b = temp;
        }*/
        /*
        private void DrawAngles(ref Bitmap b)
        {
            Bitmap temp = new Bitmap(b);
            int col;

            for (int y = 0; y < pictureBoxIn.Height; y++)
                for (int x = 0; x < pictureBoxIn.Width; x++)
                {
                    col = (int)( GetAngle(angles[x, y]) / Math.PI * 255);
                    temp.SetPixel(x, y, Color.FromArgb(col, col, col));

                }
            b = temp;
        }*/
        /*
        private double GetAngle(double p)
        {
            if (p > Math.PI / 2)
                p = Math.PI - p;

            for (int i = 15; i >= 0; i--)
                if (p >= i * Math.PI / 8)
                    return i * Math.PI / 8;
            return 0;
        }
        */
        #endregion
    }
}
