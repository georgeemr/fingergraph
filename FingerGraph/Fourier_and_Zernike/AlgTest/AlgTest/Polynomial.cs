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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DescriptionExtractor
{
    public class Polynomial
    {
        protected double[] coefficients;

        public Polynomial(double[] coeff)
        {
            int n = coeff.Length;
            coefficients = new double[n];
            for (int i = 0; i < n; i++)
            {
                coefficients[i] = coeff[i];
            }
        }

        public double Value(double x)
        {
            double sum = coefficients[0];
            for (int i = 1; i < coefficients.Length; i++)
            {
                sum = sum * x + coefficients[i];
            }

            return sum;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int n = coefficients.Length;
            for (int i = 0; i < n; i++)
            {
                double c = coefficients[i];                
                if(c > 0.0)
                    sb.Append("+" + coefficients[i] + "x^" + (n-i-1));
                else if(c < 0.0)
                    sb.Append(coefficients[i] + "x^" + (n-i-1));
            }

            return sb.ToString();
        }
    }
}
