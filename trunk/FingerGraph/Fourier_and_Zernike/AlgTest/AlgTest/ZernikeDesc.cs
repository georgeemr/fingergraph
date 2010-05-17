#region GPL EULA
// Copyright (c) 2009, Bojan Endrovski, http://furiouspixels.blogspot.com/
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published 
// by the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
#endregion

// Uncomment the next line if you want to reconstruct the images for debugging
// reasons. Twice as slooowww though!
//#define RECONSTRUCT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

namespace DescriptionExtractor
{
    public class ZernikeDesc
    {   
		// Cache the polinomials to cut down on repeative calculation
        protected static Dictionary<String, Polynomial> zernikePolynomials = new Dictionary<string,Polynomial>();

		// Memory if fastest when word addressable anyhow :)
		// Can be optimized to an actual bit-map
        protected int[,] bmap;
		
		// Input must be a square bitmap
        protected int N;

		// Maximal order of the Zernike coefs
		protected static int order = 10;

		/// <summary>
		/// Creates a Zernike descriptor for the given bitmap
		/// </summary>
		/// <param name="bitmap">The given bitmap</param>
        public ZernikeDesc(Bitmap bitmap)
        {
			// Only square!
            if(bitmap.Height != bitmap.Width)
                throw new InvalidOperationException("Bitmap not rectangular!");

            N = bitmap.Height;
            //
			// Placing to local memory and removing calls to GetPixel dramaticaly improves
			// perfomance, 20-30x
            bmap = new int[N, N];
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    int bit;
                    if (y < 0 || y > bitmap.Height || x < 0 || y > bitmap.Width)
                    {
                        bit = 0;
                    }
                    else
                    {
                        Color c = bitmap.GetPixel(x, y);
                        if ((c.A + c.R + c.G + c.B) == 1020)            // White is background (also inverts)
                            bit = 0;
                        else
                            bit = 1;
                    }
                    bmap[x, y] = bit;
                }
            }
        }

		/// <summary>
		/// Hides the implentation of the image access
		/// </summary>
		/// <param name="x">x coord</param>
		/// <param name="y">y coord</param>
		/// <returns>The image color of the [x, y] pixel</returns>
		private int I(int x, int y)
		{		
			return bmap[x, y];
		}

		#region Static Tools

		/// <summary>
		/// Factorial function
		/// </summary>
		/// <returns>The factorial of n</returns>
        private static double Factorial(int n)
        {
            double f = 1;
            for (int i = 2; i <= n; i++)
                f *= i;
            //
            return f;
        }

		/// <summary>
		/// Absolute value, now for integers
		/// </summary>
        private static int abs(int a)
        {
            if (a < 0)
                return -a;
            return a;
        }

		/// <summary>
		/// Zernike polynomial of order n,m in a simple polynomial form
		/// </summary>		
        private static Polynomial RadialPolynomial(int n, int m)
        {
            if (n == 0)
                return new Polynomial(new double[] { 1.0 });
            //
            double[] coeffs = new double[n + 1];
            //
            for (int s = 0; s <= (n - abs(m)) / 2; s++)
            {
                double nMinusCFact = Factorial(n - s);
                double sFact = Factorial(s);
                double term0 = Factorial((n + abs(m)) / 2 - s);
                double term1 = Factorial((n - abs(m)) / 2 - s);
                double c = nMinusCFact / (sFact * term0 * term1);
                c *= Math.Pow(-1, s);
                //
                coeffs[2 * s] = c;
                if (2 * s + 1 < n + 1)
                    coeffs[2 * s + 1] = 0.0;
            }
            Polynomial radialPoly = new Polynomial(coeffs);
            return radialPoly;
        }

		/// <summary>
		/// Zernike polynomial of order n,m for rho
		/// </summary>		
		/// <returns>The value of the polynomial at rho</returns>
        private static double RadialPolynomial(int n, int m, double rho)
        {
			// All of the calculations for the Zernike moments use the same polynomials (as weight functions),
			// so we cache the relusting polynomial for later use (speedup 2.5x for 10-th order)
            String key = "" + m + n;
            Polynomial poly;
            if(zernikePolynomials.ContainsKey(key))
            {
                poly = zernikePolynomials[key];
            }
            else
            {
                poly = RadialPolynomial(n, m);
				//
				// Thread safety
				lock (zernikePolynomials)
				{
					zernikePolynomials[key] = poly;	
				}                
				#if DEBUG
					Console.WriteLine("Polynomial R(" + n + "," + m + "): " + poly + " added to cache.");
				#endif
            }
            //
            return poly.Value(rho);
        }

		/// <summary>
		/// Syntactic sugar for the term V in the formula for the Zernike moments
		/// </summary>	
		private static Complex V(int n, int m, double rho, double theta)
		{
			double radial = RadialPolynomial(n, m, rho);
			double r = radial * Math.Cos(m * theta);
			double i = radial * Math.Sin(m * theta);
			return new Complex(r, i);
		}

		#endregion Static Tools

		/// <summary>
		/// Zernike moment of order n,m for the given bitmap
		/// </summary>
		private Complex ZernikeMoment(int n, int m)
        {
            double weight = 0;                 // TODO: The normalization value is subject to change
            //
			Complex sum = new Complex(0.0, 0.0);
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                    double xn = 2*x-N+1;        // Normalized x
                    double yn = N-1-2*y;        // Normalized y
                    double rho = Math.Sqrt( xn * xn + yn * yn ) / N;        // Go polar, Rho
                    if (rho <= 1.0)
                    {
						if (I(x, y) == 1)
						{
							double theta = Math.Atan2(yn, xn);                  // Go polar, Theta
							Complex Vc = V(n, m, rho, theta).Conjugate;
							sum += Vc;
						}
						weight += 1;									// Accum weight
                    }                    
                }
            }
			sum *= (n + 1) / weight;
			return sum;
        }

        private Bitmap Reconstruct(Complex[] Z)
        {
            Bitmap bmp = new Bitmap(N, N);
            //
			double[,] map = new double[N, N];
			//
			double	min = 0,
					max = 0;
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                    double xn = 2 * x - N + 1;                          // Normalized x
                    double yn = N - 1 - 2 * y;                          // Normalized y
                    double rho = Math.Sqrt(xn * xn + yn * yn) / N;      // Go polar, Rho
					double theta = Math.Atan2(yn, xn);                  // Go polar, Theta
                    if (rho <= 1.0)
                    {
						Complex sum = new Complex(0.0, 0.0);
                        int i = 0;
                        for (int n = 0; n <= order; n++)
                        {
                            for (int m = -n; m <= 0; m += 2)
                            {
								Complex znm = Z[i++];
								Complex vnm = V(n, m, rho, theta);
								Complex result;
								if(m != 0)
									result = znm * vnm + znm.Conjugate * vnm.Conjugate;
								else
									result = znm * vnm;
								//
								sum += result;								
                            }
                        }
						//
						double val = sum.Modulus;
						if (val < min)
							min = val;
						if (val > max)
							max = val;
						map[x, y] = val;
                    }
                }
            }
			//
			//
			for (int y = 0; y < N; y++)
			{
				for (int x = 0; x < N; x++)
				{
					double val = map[x, y];
					val = (val - min) / (max - min);
					int intensity = (int)(val * 255.0);
					//
					Color c = Color.FromArgb(255, intensity, intensity, intensity);
					bmp.SetPixel(x, y, c);
				}
			}

            return bmp;
        }

		/// <summary>
		/// Porcesses the bitmap, extracting the Zernike moments up to the n-th order (defined in class)
		/// </summary>
		/// <returns>Sequence of the modulos of the complex Zernike moments</returns>
        public double[] Process()
        {                         
            double[] coef = new double[36];
            //
            Complex[] Z = new Complex[order * order];
            int i = 0;
            for (int n = 0; n <= order; n++)
            {
                for (int m = -n; m <= 0; m += 2)
                {                    
                    Z[i] = ZernikeMoment(n, m);
                    coef[i] = Z[i].Modulus;
                    i++;
                }
            }
            						
			#if RECONSTRUCT
				// Debug tool
				Bitmap bmp = Reconstruct(Z);
				bmp.Save("zernike" + c++ + ".png");
			#endif

            return coef;
        }
    }
}
