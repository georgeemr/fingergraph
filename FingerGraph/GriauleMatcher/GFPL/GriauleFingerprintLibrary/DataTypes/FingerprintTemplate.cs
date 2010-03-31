namespace GriauleFingerprintLibrary.DataTypes
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public class FingerprintTemplate
    {
        public byte[] buffer;
        public int size;
        public int quality;
        public FingerprintTemplate()
        {
            this.quality = -1;
            this.size = 0x2710;
            this.buffer = new byte[this.size];
        }

        public FingerprintTemplate(IntPtr buffer, int size)
        {
            this.quality = -1;
            this.size = size;
            this.buffer = new byte[size];
            Marshal.Copy(buffer, this.buffer, 0, size);
        }

        public FingerprintTemplate(IntPtr buffer, int size, int quality)
        {
            this.quality = -1;
            this.quality = quality;
            this.size = size;
            this.buffer = new byte[size];
            Marshal.Copy(buffer, this.buffer, 0, size);
        }

        public byte[] Buffer
        {
            get
            {
                return this.buffer;
            }
            set
            {
                this.buffer = value;
            }
        }
        public int Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }
        public int Quality
        {
            get
            {
                return this.quality;
            }
            set
            {
                this.quality = value;
            }
        }
    }
}

