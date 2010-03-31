namespace GriauleFingerprintLibrary.Events
{
    using GriauleFingerprintLibrary.DataTypes;
    using System;

    public class ImageEventArgs : EventArgs
    {
        private string source;
        private FingerprintRawImage tRawImage;

        public ImageEventArgs(string source, IntPtr rawImage, int width, int height, int res)
        {
            this.source = source;
            this.tRawImage = new FingerprintRawImage(rawImage, width, height, res);
        }

        public FingerprintRawImage RawImage
        {
            get
            {
                return this.tRawImage;
            }
        }

        public string Source
        {
            get
            {
                return this.source;
            }
        }
    }
}

