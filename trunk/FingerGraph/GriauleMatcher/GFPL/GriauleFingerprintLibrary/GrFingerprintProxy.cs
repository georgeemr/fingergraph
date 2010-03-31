namespace GriauleFingerprintLibrary
{
    using System;
    using System.Runtime.InteropServices;

    internal static class GrFingerprintProxy
    {
        [DllImport("GrFinger.dll", EntryPoint="GrDecodeBase64")]
        public static extern int DecodeBase64(IntPtr encodedBuffer, int encodedSize, [Out] IntPtr decodedBuffer, ref int decodedSize);
        [DllImport("GrFinger.dll", EntryPoint="GrEncodeBase64")]
        public static extern int EncodeBase64(IntPtr buffer, int bufferSize, [Out] IntPtr encodedBuffer, ref int encodedSize);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr ptr);
        [DllImport("GrFinger.dll")]
        public static extern int GrBiometricDisplay(IntPtr tpt, IntPtr rawImage, int width, int height, int res, IntPtr hdc, ref IntPtr handle, int matchContext);
        [DllImport("GrFinger.dll")]
        public static extern int GrCapFinalize();
        [DllImport("GrFinger.dll")]
        public static extern int GrCapInitialize(GrStatusEventHandler statusEventHandler);
        [DllImport("GrFinger.dll")]
        public static extern int GrCapLoadImageFromFile(string filename, int res);
        [DllImport("GrFinger.dll")]
        public static extern int GrCapRawImageToHandle([In] IntPtr rawImage, int width, int height, IntPtr hdc, ref IntPtr handle);
        [DllImport("GrFinger.dll")]
        public static extern int GrCapSaveRawImageToFile([In] IntPtr rawimage, int width, int height, string fileName, GrCaptureImageFormat imageFormat);
        [DllImport("GrFinger.dll")]
        public static extern int GrCapStartCapture(string idSensor, GrFingerEventHandler fingerEventHandler, GrImageEventHandler imageEventHandler);
        [DllImport("GrFinger.dll")]
        public static extern int GrCapStopCapture(string idSensor);
        [DllImport("GrFinger.dll")]
        public static extern int GrConvertTemplate([In] IntPtr tpt, [Out] IntPtr newTpt, ref int newTptSize, int context, int format);
        [DllImport("GrFinger.dll")]
        public static extern int GrCreateContext(out int contextId);
        [DllImport("GrFinger.dll")]
        public static extern int GrDestroyContext(int contextId);
        [DllImport("GrFinger.dll")]
        public static extern int GrEnroll([In] IntPtr rawimage, int width, int height, int res, [Out] IntPtr tpt, ref int tptSize, ref int quality, int tptFormat, int context);
        [DllImport("GrFinger.dll")]
        public static extern int GrExtract(IntPtr rawimage, int width, int height, int res, [Out] IntPtr referenceTemplate, ref int tptSize, int context);
        [DllImport("GrFinger.dll")]
        public static extern int GrExtractEx(IntPtr rawimage, int width, int height, int res, [Out] IntPtr referenceTemplate, ref int tptSize, int context, int tptFormat);
        [DllImport("GrFinger.dll")]
        public static extern int GrFinalize();
        [DllImport("GrFinger.dll")]
        public static extern int GrGetGrFingerVersion(ref byte majorVersion, ref byte minorVersion);
        [DllImport("GrFinger.dll")]
        public static extern int GrGetIdentifyParameters(out int identifyThreshold, out int identifyRotationTolerance, int context);
        [DllImport("GrFinger.dll")]
        public static extern int GrGetVerifyParameters(out int verifyThreshold, out int verifyRotationTolerance, int context);
        [DllImport("GrFinger.dll")]
        public static extern int GrIdentify(IntPtr referenceTemplate, out int verifyScore, int context);
        [DllImport("GrFinger.dll")]
        public static extern int GrIdentifyPrepare(IntPtr queryTemplate, int context);
        [DllImport("GrFinger.dll")]
        public static extern int GrInitialize();
        [DllImport("GrFinger.dll")]
        public static extern int GrInstallLicense(string productKey);
        [DllImport("GrFinger.dll")]
        public static extern int GrSetBiometricDisplayColors(int minutiaeColor, int minutiaeMatchedColor, int segmentColor, int segmentMatchedColor, int inclinationColor, int inclinationMatchedColor);
        [DllImport("GrFinger.dll")]
        public static extern int GrSetIdentifyParameters(int identifyThreshold, int identifyRotationTolerance, int context);
        [DllImport("GrFinger.dll")]
        public static extern int GrSetLicenseFolder(string licenseFolder);
        [DllImport("GrFinger.dll")]
        public static extern int GrSetVerifyParameters(int verifyThreshold, int verifyRotationTolerance, int context);
        [DllImport("GrFinger.dll")]
        public static extern IntPtr GrStartEnroll(int context);
        [DllImport("GrFinger.dll")]
        public static extern int GrVerify(IntPtr queryTemplate, IntPtr referenceTemplate, out int verifyScore, int context);
        [DllImport("GrFinger.dll", EntryPoint="GrSetLicenseFolder")]
        public static extern bool IsBase64Encoding(IntPtr buffer, int bufferSize);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);
    }
}

