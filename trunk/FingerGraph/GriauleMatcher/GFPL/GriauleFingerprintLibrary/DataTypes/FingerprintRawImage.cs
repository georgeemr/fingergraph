namespace GriauleFingerprintLibrary.DataTypes
{
    using GriauleFingerprintLibrary;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class FingerprintRawImage
    {
        public int height;
        public byte[] rawImage;
        public int res;
        public int width;

        public FingerprintRawImage() { }

        public FingerprintRawImage(IntPtr rawImage, int width, int height, int res)
        {
            this.rawImage = new byte[width * height];
            try
            {
                Marshal.Copy(rawImage, this.rawImage, 0, width * height);
                this.width = width;
                this.height = height;
                this.res = res;
                GC.AddMemoryPressure((long) (width * height));
            }
            catch (Exception)
            {
                this.rawImage = null;
            }
        }

        ~FingerprintRawImage()
        {
            GC.RemoveMemoryPressure((long) (this.width * this.height));
        }

        public int Height
        {
            get
            {
                return this.height;
            }
        }

        public Bitmap Image
        {
            get
            {
                Bitmap bitmap;
                IntPtr zero = IntPtr.Zero;
                IntPtr destination = IntPtr.Zero;
                try
                {
                    IntPtr handle = new IntPtr();
                    zero = GrFingerprintProxy.GetDC(IntPtr.Zero);
                    destination = Marshal.AllocCoTaskMem(this.width * this.height);
                    Marshal.Copy(this.rawImage, 0, destination, this.width * this.height);
                    GrFingerprintProxy.GrCapRawImageToHandle(destination, this.Width, this.Height, zero, ref handle);
                    Marshal.FreeCoTaskMem(destination);
                    bitmap = System.Drawing.Image.FromHbitmap(handle);
                }
                catch
                {
                    bitmap = null;
                }
                finally
                {
                    GrFingerprintProxy.ReleaseDC(IntPtr.Zero, zero);
                }
                return bitmap;
            }
        }

        public byte[] RawImage
        {
            get
            {
                return this.rawImage;
            }
        }

        public int Resolution
        {
            get
            {
                return this.res;
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }
        }
    }
}

