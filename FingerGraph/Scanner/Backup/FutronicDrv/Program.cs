using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace FutronicDrv {
    struct _FTRSCAN_IMAGE_SIZE {
        public int nWidth;
        public int nHeight;
        public int nImageSize;
    }

    class Program {

        [DllImport("ftrScanAPI.dll")]
        static extern IntPtr ftrScanOpenDevice();
        [DllImport("ftrScanAPI.dll")]
        static extern bool ftrScanCloseDevice(IntPtr ftrHandle);
        [DllImport("ftrScanAPI.dll")]
        static extern bool ftrScanSetDiodesStatus(IntPtr ftrHandle, byte byGreenDiodeStatus, byte byRedDiodeStatus );
        [DllImport("ftrScanAPI.dll")]
        static extern bool ftrScanGetDiodesStatus(IntPtr ftrHandle, out bool pbIsGreenDiodeOn, out bool pbIsRedDiodeOn); 
        [DllImport("ftrScanAPI.dll")]
        static extern bool ftrScanGetImageSize(IntPtr ftrHandle, out _FTRSCAN_IMAGE_SIZE pImageSize);
        [DllImport("ftrScanAPI.dll")]
        static extern bool ftrScanGetImage(IntPtr ftrHandle, int nDose, byte[] pBuffer);

        static void Main(string[] args) {
            var ptr = ftrScanOpenDevice();
            Console.WriteLine(ptr);
            bool green = true;
            bool red = true;
            bool i = ftrScanGetDiodesStatus(ptr, out green, out red);
            i = ftrScanSetDiodesStatus(ptr, (byte)255, (byte)255);
            i = ftrScanSetDiodesStatus(ptr, (byte)0, (byte)0);
            var t = new _FTRSCAN_IMAGE_SIZE();
            i = ftrScanGetImageSize(ptr, out t);
            byte[] arr = new byte[t.nImageSize];
            i = ftrScanGetImage(ptr, 4, arr);
            i = ftrScanCloseDevice(ptr);
            var b = new Bitmap(t.nWidth, t.nHeight);
            for (int x = 0; x < t.nWidth; x++)
                for (int y = 0; y < t.nHeight; y++) {
                    int a = 255-arr[y * t.nWidth + x];
                    b.SetPixel(x, y, Color.FromArgb(a, a, a));
                }
            b.Save("D:\\finger.bmp");
            Console.ReadKey();

        }
    }
}
