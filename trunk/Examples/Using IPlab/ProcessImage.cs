using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Imaging.Filters;

namespace ImageProcessing
{
    class ProcessImage
    {

        // Apply filter on the image
        private static void ApplyFilter(IFilter filter, ref Bitmap image)
        {
            if (filter is IFilterInformation)
            {
                IFilterInformation filterInfo = (IFilterInformation)filter;
                if (!filterInfo.FormatTransalations.ContainsKey(image.PixelFormat))
                {
                    if (filterInfo.FormatTransalations.ContainsKey(PixelFormat.Format24bppRgb))
                    {
                        MessageBox.Show("The selected image processing routine may be applied to color image only.",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(
                            "The selected image processing routine may be applied to grayscale or binary image only.\n\nUse grayscale (and threshold filter if required) before.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }
            }
            try
            {
                // apply filter to the image
                image = filter.Apply(image);
            }
            catch
            {
                MessageBox.Show("Error occured applying selected filter to the image", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }


        public static Image Process(Image inputimage)
        {
            Bitmap bmpimage = new Bitmap(inputimage);
            ApplyFilter(new GrayscaleBT709(), ref bmpimage);            //grayscaling
            //ApplyFilter(new GaussianBlur(10, 7), ref bmpimage);             //bluring
            ApplyFilter(new Threshold(128), ref bmpimage); //binarizing

            return bmpimage;
        }


    }
}
