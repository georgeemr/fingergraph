using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace FutronicDrv
{
    struct _FTRSCAN_IMAGE_SIZE
    {
        public int nWidth;
        public int nHeight;
        public int nImageSize;
    }

    class Device : IDisposable
    {
        [DllImport("ftrScanAPI.dll")]
        static extern IntPtr ftrScanOpenDevice();
        [DllImport("ftrScanAPI.dll")]
        static extern void ftrScanCloseDevice(IntPtr ftrHandle);
        [DllImport("ftrScanAPI.dll")]
        static extern bool ftrScanSetDiodesStatus(IntPtr ftrHandle, byte byGreenDiodeStatus, byte byRedDiodeStatus);
        [DllImport("ftrScanAPI.dll")]
        static extern bool ftrScanGetDiodesStatus(IntPtr ftrHandle, out bool pbIsGreenDiodeOn, out bool pbIsRedDiodeOn);
        [DllImport("ftrScanAPI.dll")]
        static extern bool ftrScanGetImageSize(IntPtr ftrHandle, out _FTRSCAN_IMAGE_SIZE pImageSize);
        [DllImport("ftrScanAPI.dll")]
        static extern bool ftrScanGetImage(IntPtr ftrHandle, int nDose, byte[] pBuffer);

        IntPtr device;

        public Device() { }

        public bool Init()
        {
            if (!Connected())
                device = ftrScanOpenDevice();
            return Connected();
        }

        public bool Connected()
        {
            return (device != IntPtr.Zero);
        }

        public void Dispose()
        {
            if (Connected())
            {
                ftrScanCloseDevice(device);
                device = IntPtr.Zero;
            }
        }

        public Bitmap ExportBitMap()
        {
            if (!Connected())
                return null;

            var t = new _FTRSCAN_IMAGE_SIZE();
            ftrScanGetImageSize(device, out t);
            byte[] arr = new byte[t.nImageSize];
            ftrScanGetImage(device, 4, arr);
  
            var b = new Bitmap(t.nWidth, t.nHeight);
            for (int x = 0; x < t.nWidth; x++)
                for (int y = 0; y < t.nHeight; y++)
                {
                    int a = 255 - arr[y * t.nWidth + x];
                    b.SetPixel(x, y, Color.FromArgb(a, a, a));
                }
            return b;
        }

        public void GetDiodesStatus(out bool green, out bool red)
        {
            ftrScanGetDiodesStatus(device, out green, out red);
        }

        public void SetDiodesStatus( bool green, bool red )
        {
            ftrScanSetDiodesStatus(device, (byte)(green ? 255 : 0), (byte)(red ? 255 : 0));
        }
    }
}