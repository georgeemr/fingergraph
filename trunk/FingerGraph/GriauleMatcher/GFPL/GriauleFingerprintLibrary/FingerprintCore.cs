namespace GriauleFingerprintLibrary
{
    using GriauleFingerprintLibrary.DataTypes;
    using GriauleFingerprintLibrary.Events;
    using GriauleFingerprintLibrary.Exceptions;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class FingerprintCore
    {
        private bool bInitialized;
        private GCHandle gchFinger;
        private GCHandle gchImage;
        private GCHandle gchStatus;
        private GrFingerEventHandler grFingerEventHandler;
        private GrImageEventHandler grImageEventHandler;
        private GrStatusEventHandler grStatusEventHandler;

        public event FingerEventHandler onFinger;

        public event ImageEventHandler onImage;

        public event StatusEventHandler onStatus;

        public void CaptureFinalize()
        {
            try
            {
                int errorCode = 0;
                try
                {
                    errorCode = GrFingerprintProxy.GrCapFinalize();
                }
                catch (AccessViolationException exception)
                {
                    FingerprintException exception2 = new FingerprintException(-113, exception);
                    throw exception2;
                }
                FingerprintException.CheckError(errorCode);
            }
            finally
            {
                if (this.grStatusEventHandler != null)
                {
                    this.grStatusEventHandler = null;
                    if (this.gchStatus.IsAllocated)
                    {
                        this.gchStatus.Free();
                    }
                }
            }
        }

        public void CaptureInitialize()
        {
            if (this.grStatusEventHandler == null)
            {
                this.grStatusEventHandler = new GrStatusEventHandler(this.OnStatus);
                this.gchStatus = GCHandle.Alloc(this.grStatusEventHandler);
            }
            this.CaptureInitialize(this.grStatusEventHandler);
        }

        private void CaptureInitialize(GrStatusEventHandler grStatusEventHandler)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrCapInitialize(grStatusEventHandler);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
        }

        public void ConvertTemplate(FingerprintTemplate tpt, [Out] FingerprintTemplate newTpt, GrTemplateFormat format)
        {
            this.ConvertTemplate(tpt, newTpt, 0, format);
        }

        public void ConvertTemplate(FingerprintTemplate tpt, [Out] FingerprintTemplate newTpt, int context, GrTemplateFormat format)
        {
            IntPtr zero = IntPtr.Zero;
            IntPtr ptr2 = IntPtr.Zero;
            try
            {
                zero = Marshal.AllocHGlobal(tpt.Size);
                Marshal.Copy(tpt.Buffer, 0, zero, tpt.Size);
                ptr2 = Marshal.AllocHGlobal(0x2710);
                int newTptSize = 0x2710;
                this.ConvertTemplate(zero, ptr2, ref newTptSize, context, (int) format);
                ptr2 = Marshal.ReAllocHGlobal(ptr2, (IntPtr) newTptSize);
                newTpt = new FingerprintTemplate(ptr2, newTptSize);
            }
            catch (OutOfMemoryException exception)
            {
                throw new FingerprintException(-7, exception);
            }
            catch (NullReferenceException exception2)
            {
                throw new FingerprintException(-5, exception2);
            }
            catch (ArgumentException exception3)
            {
                throw new FingerprintException(-5, exception3);
            }
            finally
            {
                Marshal.FreeHGlobal(zero);
                Marshal.FreeHGlobal(ptr2);
            }
        }

        private void ConvertTemplate([In] IntPtr tpt, [Out] IntPtr newTpt, ref int newTptSize, int context, int format)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrConvertTemplate(tpt, newTpt, ref newTptSize, context, format);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
        }

        public void CreateContext(out int context)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrCreateContext(out context);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
        }

        public static FingerprintTemplate DecodeBase64(FingerprintTemplate encodedTemplate)
        {
            FingerprintTemplate template;
            IntPtr zero = IntPtr.Zero;
            IntPtr decodedBuffer = IntPtr.Zero;
            try
            {
                zero = Marshal.AllocHGlobal(encodedTemplate.Size);
                Marshal.Copy(encodedTemplate.Buffer, 0, zero, encodedTemplate.Size);
                int decodedSize = 0x2710;
                decodedBuffer = Marshal.AllocHGlobal(0x2710);
                DecodeBase64(zero, encodedTemplate.Size, ref decodedBuffer, ref decodedSize);
                template = new FingerprintTemplate(decodedBuffer, decodedSize);
            }
            catch (OutOfMemoryException exception)
            {
                throw new FingerprintException(-7, exception);
            }
            finally
            {
                Marshal.FreeHGlobal(zero);
                Marshal.FreeHGlobal(decodedBuffer);
            }
            return template;
        }

        public static int DecodeBase64(IntPtr encodedBuffer, int encodedSize, ref IntPtr decodedBuffer, ref int decodedSize)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.DecodeBase64(encodedBuffer, encodedSize, decodedBuffer, ref decodedSize);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
            return errorCode;
        }

        public void DestroyContext(int context)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrDestroyContext(context);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
        }

        public static FingerprintTemplate EncodeBase64(FingerprintTemplate referenceTemplate)
        {
            FingerprintTemplate template;
            IntPtr zero = IntPtr.Zero;
            IntPtr encodedBuffer = IntPtr.Zero;
            try
            {
                zero = Marshal.AllocHGlobal(referenceTemplate.Size);
                Marshal.Copy(referenceTemplate.Buffer, 0, zero, referenceTemplate.Size);
                int encodedSize = 0x2710;
                encodedBuffer = Marshal.AllocHGlobal(0x2710);
                EncodeBase64(zero, referenceTemplate.Size, ref encodedBuffer, ref encodedSize);
                template = new FingerprintTemplate(encodedBuffer, encodedSize);
            }
            catch (OutOfMemoryException exception)
            {
                throw new FingerprintException(-7, exception);
            }
            finally
            {
                Marshal.FreeHGlobal(zero);
                Marshal.FreeHGlobal(encodedBuffer);
            }
            return template;
        }

        public static int EncodeBase64(IntPtr buffer, int bufferSize, ref IntPtr encodedBuffer, ref int encodedSize)
        {
            int num = 0;
            try
            {
                num = GrFingerprintProxy.EncodeBase64(buffer, bufferSize, encodedBuffer, ref encodedSize);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            return num;
        }

        public GrEnrollState Enroll(FingerprintRawImage rawImage, ref FingerprintTemplate tpt, GrTemplateFormat tptFormat)
        {
            return this.Enroll(rawImage, ref tpt, tptFormat, 0);
        }

        public GrEnrollState Enroll(FingerprintRawImage rawImage, ref FingerprintTemplate tpt, GrTemplateFormat tptFormat, int context)
        {
            GrEnrollState state;
            IntPtr zero = IntPtr.Zero;
            try
            {
                int num = 0;
                int quality = -1;
                int cb = 0x2710 * Marshal.SizeOf(typeof(byte));
                zero = Marshal.AllocHGlobal(cb);
                IntPtr destination = Marshal.AllocCoTaskMem(rawImage.Width * rawImage.Height);
                Marshal.Copy(rawImage.RawImage, 0, destination, rawImage.Width * rawImage.Height);
                num = this.Enroll(destination, rawImage.Width, rawImage.Height, rawImage.Resolution, zero, ref cb, ref quality, (int) tptFormat, context);
                Marshal.FreeCoTaskMem(destination);
                zero = Marshal.ReAllocHGlobal(zero, (IntPtr) cb);
                tpt = new FingerprintTemplate(zero, cb, quality);
                state = (GrEnrollState) num;
            }
            catch (OutOfMemoryException exception)
            {
                throw new FingerprintException(-7, exception);
            }
            catch (AccessViolationException exception2)
            {
                throw new FingerprintException(-113, exception2);
            }
            finally
            {
                Marshal.FreeHGlobal(zero);
            }
            return state;
        }

        private int Enroll([In] IntPtr rawimage, int width, int height, int res, IntPtr tpt, ref int tptSize, ref int quality, int tptFormat, int context)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrEnroll(rawimage, width, height, res, tpt, ref tptSize, ref quality, tptFormat, context);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
            return errorCode;
        }

        public void Extract(FingerprintRawImage fingerPrintRawImage, ref FingerprintTemplate fingerTemplate)
        {
            this.Extract(fingerPrintRawImage, ref fingerTemplate, 0);
        }

        public void Extract(FingerprintRawImage fingerPrintRawImage, ref FingerprintTemplate fingerTemplate, int context)
        {
            try
            {
                IntPtr destination = Marshal.AllocCoTaskMem(fingerPrintRawImage.Width * fingerPrintRawImage.Height);
                Marshal.Copy(fingerPrintRawImage.RawImage, 0, destination, fingerPrintRawImage.Width * fingerPrintRawImage.Height);
                this.Extract(destination, fingerPrintRawImage.Width, fingerPrintRawImage.Height, fingerPrintRawImage.Resolution, ref fingerTemplate, context);
                Marshal.FreeCoTaskMem(destination);
            }
            catch (OutOfMemoryException exception)
            {
                FingerprintException exception2 = new FingerprintException(-7, exception);
                throw exception2;
            }
        }

        private void Extract([In] IntPtr rawimage, int width, int height, int res, ref FingerprintTemplate fingerTemplate, int context)
        {
            IntPtr zero = IntPtr.Zero;
            try
            {
                int cb = 0x2710 * Marshal.SizeOf(typeof(byte));
                zero = Marshal.AllocHGlobal(cb);
                int quality = FingerprintException.CheckError(GrFingerprintProxy.GrExtract(rawimage, width, height, res, zero, ref cb, context));
                zero = Marshal.ReAllocHGlobal(zero, (IntPtr) cb);
                fingerTemplate = new FingerprintTemplate(zero, cb, quality);
            }
            catch (OutOfMemoryException exception)
            {
                throw new FingerprintException(-7, exception);
            }
            catch (AccessViolationException exception2)
            {
                throw new FingerprintException(-113, exception2);
            }
            finally
            {
                Marshal.FreeHGlobal(zero);
            }
        }

        public void ExtractEx(FingerprintRawImage fingerPrintRawImage, ref FingerprintTemplate fingerTemplate, GrTemplateFormat tptFormat)
        {
            this.ExtractEx(fingerPrintRawImage, ref fingerTemplate, 0, tptFormat);
        }

        public void ExtractEx(FingerprintRawImage fingerPrintRawImage, ref FingerprintTemplate fingerTemplate, int context, GrTemplateFormat tptFormat)
        {
            try
            {
                IntPtr destination = Marshal.AllocCoTaskMem(fingerPrintRawImage.Width * fingerPrintRawImage.Height);
                Marshal.Copy(fingerPrintRawImage.RawImage, 0, destination, fingerPrintRawImage.Width * fingerPrintRawImage.Height);
                this.ExtractEx(destination, fingerPrintRawImage.Width, fingerPrintRawImage.Height, fingerPrintRawImage.Resolution, ref fingerTemplate, context, (int) tptFormat);
                Marshal.FreeCoTaskMem(destination);
            }
            catch (OutOfMemoryException exception)
            {
                FingerprintException exception2 = new FingerprintException(-7, exception);
                throw exception2;
            }
        }

        private void ExtractEx([In] IntPtr rawimage, int width, int height, int res, ref FingerprintTemplate fingerTemplate, int context, int tptFormat)
        {
            IntPtr zero = IntPtr.Zero;
            try
            {
                int cb = 0x2710 * Marshal.SizeOf(typeof(byte));
                zero = Marshal.AllocHGlobal(cb);
                int quality = FingerprintException.CheckError(GrFingerprintProxy.GrExtractEx(rawimage, width, height, res, zero, ref cb, context, tptFormat));
                zero = Marshal.ReAllocHGlobal(zero, (IntPtr) cb);
                fingerTemplate = new FingerprintTemplate(zero, cb, quality);
            }
            catch (OutOfMemoryException exception)
            {
                throw new FingerprintException(-7, exception);
            }
            finally
            {
                Marshal.FreeHGlobal(zero);
            }
        }

        ~FingerprintCore()
        {
            if (this.gchStatus.IsAllocated)
            {
                this.gchStatus.Free();
            }
            if (this.gchFinger.IsAllocated)
            {
                this.gchFinger.Free();
            }
            if (this.gchImage.IsAllocated)
            {
                this.gchImage.Free();
            }
            if (this.bInitialized)
            {
                this.Finalizer();
            }
        }

        public void Finalizer()
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrFinalize();
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
            this.bInitialized = false;
        }

        public void GetBiometricDisplay(FingerprintTemplate tptQuery, FingerprintRawImage rawImage, IntPtr hdc, ref IntPtr handle, int matchContext)
        {
            try
            {
                IntPtr destination = Marshal.AllocCoTaskMem(rawImage.Width * rawImage.Height);
                Marshal.Copy(rawImage.RawImage, 0, destination, rawImage.Width * rawImage.Height);
                this.GetBiometricDisplay(tptQuery, destination, rawImage.Width, rawImage.Height, rawImage.Resolution, hdc, ref handle, matchContext);
                Marshal.FreeCoTaskMem(destination);
            }
            catch (OutOfMemoryException exception)
            {
                FingerprintException exception2 = new FingerprintException(-7, exception);
                throw exception2;
            }
        }

        private void GetBiometricDisplay(FingerprintTemplate tptQuery, IntPtr rawImage, int width, int height, int res, IntPtr hdc, ref IntPtr handle, int matchContext)
        {
            IntPtr zero = IntPtr.Zero;
            try
            {
                zero = Marshal.AllocHGlobal(tptQuery.Size);
                Marshal.Copy(tptQuery.Buffer, 0, zero, tptQuery.Size);
                int errorCode = 0;
                try
                {
                    errorCode = GrFingerprintProxy.GrBiometricDisplay(zero, rawImage, width, height, res, hdc, ref handle, matchContext);
                }
                catch (AccessViolationException exception)
                {
                    FingerprintException exception2 = new FingerprintException(-113, exception);
                    throw exception2;
                }
                FingerprintException.CheckError(errorCode);
            }
            catch (OutOfMemoryException exception3)
            {
                throw new FingerprintException(-7, exception3);
            }
            finally
            {
                Marshal.FreeHGlobal(zero);
            }
        }

        public static IntPtr GetDC()
        {
            return GrFingerprintProxy.GetDC(IntPtr.Zero);
        }

        public void GetHandlerFromRawImage(FingerprintRawImage rawImage, IntPtr hdc, ref IntPtr image)
        {
            try
            {
                IntPtr destination = Marshal.AllocCoTaskMem(rawImage.Width * rawImage.Height);
                Marshal.Copy(rawImage.RawImage, 0, destination, rawImage.Width * rawImage.Height);
                this.GetHandlerFromRawImage(destination, rawImage.Width, rawImage.Height, hdc, ref image);
                Marshal.FreeCoTaskMem(destination);
            }
            catch (OutOfMemoryException exception)
            {
                FingerprintException exception2 = new FingerprintException(-7, exception);
                throw exception2;
            }
        }

        private void GetHandlerFromRawImage(IntPtr rawImage, int width, int height, IntPtr hdc, ref IntPtr image)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrCapRawImageToHandle(rawImage, width, height, hdc, ref image);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
        }

        public void GetIdentifyParameters(out int identifyThreshold, out int identifyRotationTolerance)
        {
            this.GetIdentifyParameters(out identifyThreshold, out identifyRotationTolerance, 0);
        }

        public void GetIdentifyParameters(out int identifyThreshold, out int identifyRotationTolerance, int context)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrGetIdentifyParameters(out identifyThreshold, out identifyRotationTolerance, context);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
        }

        public static GrLicense GetStrGrFingerVersion(ref byte majorVersion, ref byte minorVersion)
        {
            int num = 0;
            try
            {
                num = GrFingerprintProxy.GrGetGrFingerVersion(ref majorVersion, ref minorVersion);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            return (GrLicense) num;
        }

        public void GetVerifyParameters(out int verifyThreshold, out int verifyRotationTolerance)
        {
            this.GetVerifyParameters(out verifyThreshold, out verifyRotationTolerance, 0);
        }

        public void GetVerifyParameters(out int verifyThreshold, out int verifyRotationTolerance, int context)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrGetVerifyParameters(out verifyThreshold, out verifyRotationTolerance, context);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
        }

        public int Identify(FingerprintTemplate referenceTemplate, out int verifyScore)
        {
            return this.Identify(referenceTemplate, out verifyScore, 0);
        }

        public int Identify(FingerprintTemplate referenceTemplate, out int verifyScore, int context)
        {
            int num;
            IntPtr zero = IntPtr.Zero;
            try
            {
                zero = Marshal.AllocHGlobal(referenceTemplate.Size);
                Marshal.Copy(referenceTemplate.Buffer, 0, zero, referenceTemplate.Size);
                num = this.Identify(zero, out verifyScore, context);
            }
            catch (OutOfMemoryException exception)
            {
                throw new FingerprintException(-7, exception);
            }
            catch (NullReferenceException exception2)
            {
                throw new FingerprintException(-5, exception2);
            }
            catch (ArgumentException exception3)
            {
                throw new FingerprintException(-5, exception3);
            }
            finally
            {
                Marshal.FreeHGlobal(zero);
            }
            return num;
        }

        private int Identify(IntPtr referenceTemplate, out int verifyScore, int context)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrIdentify(referenceTemplate, out verifyScore, context);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
            return errorCode;
        }

        public void IdentifyPrepare(FingerprintTemplate queryTemplate)
        {
            this.IdentifyPrepare(queryTemplate, 0);
        }

        public void IdentifyPrepare(FingerprintTemplate queryTemplate, int context)
        {
            IntPtr zero = IntPtr.Zero;
            try
            {
                zero = Marshal.AllocHGlobal(queryTemplate.Size);
                Marshal.Copy(queryTemplate.Buffer, 0, zero, queryTemplate.Size);
                this.IdentifyPrepare(zero, context);
            }
            catch (OutOfMemoryException exception)
            {
                throw new FingerprintException(-7, exception);
            }
            catch (ArgumentException exception2)
            {
                throw new FingerprintException(-5, exception2);
            }
            finally
            {
                Marshal.FreeHGlobal(zero);
            }
        }

        private void IdentifyPrepare(IntPtr queryTemplate, int context)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrIdentifyPrepare(queryTemplate, context);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
        }

        public void Initialize()
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrInitialize();
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
            this.bInitialized = true;
        }

        public static void InstallLicense(string productKey)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrInstallLicense(productKey);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
        }

        public static bool IsBase64Encoding(IntPtr buffer, int bufferSize)
        {
            return GrFingerprintProxy.IsBase64Encoding(buffer, bufferSize);
        }

        public void LoadImageFromFile(string filename, int res)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrCapLoadImageFromFile(filename, res);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
        }

        private void OnFinger(string source, int e)
        {
            if (this.onFinger != null)
            {
                FingerEventType fingerEventType = (e == 10) ? FingerEventType.FINGER_UP : FingerEventType.FINGER_DOWN;
                FingerEventArgs fe = new FingerEventArgs(source, fingerEventType);
                this.onFinger(source, fe);
            }
        }

        private void OnImage(string source, int width, int height, IntPtr rawImage, int res)
        {
            if (this.onImage != null)
            {
                ImageEventArgs ie = new ImageEventArgs(source, rawImage, width, height, res);
                this.onImage(source, ie);
            }
        }

        private void OnStatus(string sender, int e)
        {
            if (this.onStatus != null)
            {
                StatusEventType statusEventType = (e == 0x15) ? StatusEventType.SENSOR_PLUG : StatusEventType.SENSOR_UNPLUG;
                StatusEventArgs se = new StatusEventArgs(sender, statusEventType);
                this.onStatus(sender, se);
            }
        }

        public static void ReleaseDC(IntPtr intPtr)
        {
            GrFingerprintProxy.ReleaseDC(IntPtr.Zero, intPtr);
        }

        public void SaveRawImageToFile(FingerprintRawImage rawImage, string fileName, GrCaptureImageFormat imageFormat)
        {
            try
            {
                IntPtr destination = Marshal.AllocCoTaskMem(rawImage.Width * rawImage.Height);
                Marshal.Copy(rawImage.RawImage, 0, destination, rawImage.Width * rawImage.Height);
                this.SaveRawImageToFile(destination, rawImage.Width, rawImage.Height, fileName, imageFormat);
                Marshal.FreeCoTaskMem(destination);
            }
            catch (OutOfMemoryException exception)
            {
                FingerprintException exception2 = new FingerprintException(-7, exception);
                throw exception2;
            }
        }

        private void SaveRawImageToFile(IntPtr rawimage, int width, int height, string fileName, GrCaptureImageFormat imageFormat)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrCapSaveRawImageToFile(rawimage, width, height, fileName, imageFormat);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-7, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
        }

        public void SetBiometricDisplayColors(int minutiaeColor, int minutiaeMatchedColor, int segmentColor, int segmentMatchedColor, int inclinationColor, int inclinationMatchedColor)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrSetBiometricDisplayColors(minutiaeColor, minutiaeMatchedColor, segmentColor, segmentMatchedColor, inclinationColor, inclinationMatchedColor);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
        }

        public void SetIdentifyParameters(int identifyThreshold, int identifyRotationTolerance)
        {
            this.SetIdentifyParameters(identifyThreshold, identifyRotationTolerance, 0);
        }

        public void SetIdentifyParameters(int identifyThreshold, int identifyRotationTolerance, int context)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrSetIdentifyParameters(identifyThreshold, identifyRotationTolerance, context);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
        }

        public static void SetLicenseFolder(string licenseFolder)
        {
            try
            {
                GrFingerprintProxy.GrSetLicenseFolder(licenseFolder);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
        }

        public void SetVerifyParameters(int verifyThreshold, int verifyRotationTolerance)
        {
            this.SetVerifyParameters(verifyThreshold, verifyRotationTolerance, 0);
        }

        public void SetVerifyParameters(int verifyThreshold, int verifyRotationTolerance, int context)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrSetVerifyParameters(verifyThreshold, verifyRotationTolerance, context);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
        }

        public void StartCapture(object sensor)
        {
            if (this.grFingerEventHandler == null)
            {
                this.grFingerEventHandler = new GrFingerEventHandler(this.OnFinger);
                this.gchFinger = GCHandle.Alloc(this.grFingerEventHandler);
            }
            if (this.grImageEventHandler == null)
            {
                this.grImageEventHandler = new GrImageEventHandler(this.OnImage);
                this.gchImage = GCHandle.Alloc(this.grImageEventHandler);
            }
            this.StartCapture(sensor.ToString(), this.grFingerEventHandler, this.grImageEventHandler);
        }

        private void StartCapture(string sensor, GrFingerEventHandler grFingerEventHandler, GrImageEventHandler grImageEventHandler)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrCapStartCapture(sensor, grFingerEventHandler, grImageEventHandler);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
        }

        public void StartEnroll()
        {
            this.StartEnroll(0);
        }

        public void StartEnroll(int context)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrStartEnroll(context).ToInt32();
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
        }

        public void StopCapture(object idSensor)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrCapStopCapture(idSensor.ToString());
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            finally
            {
                if (this.gchFinger.IsAllocated)
                {
                    this.gchFinger.Free();
                }
                if (this.gchImage.IsAllocated)
                {
                    this.gchImage.Free();
                }
            }
            FingerprintException.CheckError(errorCode);
        }

        public int Verify(FingerprintTemplate queryTemplate, FingerprintTemplate referenceTemplate, out int verifyScore)
        {
            return this.Verify(queryTemplate, referenceTemplate, out verifyScore, 0);
        }

        public int Verify(FingerprintTemplate queryTemplate, FingerprintTemplate referenceTemplate, out int verifyScore, int context)
        {
            int num;
            IntPtr zero = IntPtr.Zero;
            IntPtr destination = IntPtr.Zero;
            try
            {
                zero = Marshal.AllocHGlobal(queryTemplate.Size);
                Marshal.Copy(queryTemplate.Buffer, 0, zero, queryTemplate.Size);
                destination = Marshal.AllocHGlobal(referenceTemplate.Size);
                Marshal.Copy(referenceTemplate.Buffer, 0, destination, referenceTemplate.Size);
                num = this.Verify(zero, destination, out verifyScore, context);
            }
            catch (OutOfMemoryException exception)
            {
                throw new FingerprintException(-7, exception);
            }
            catch (NullReferenceException exception2)
            {
                throw new FingerprintException(-5, exception2);
            }
            catch (ArgumentException exception3)
            {
                throw new FingerprintException(-5, exception3);
            }
            finally
            {
                Marshal.FreeHGlobal(zero);
                Marshal.FreeHGlobal(destination);
            }
            return num;
        }

        private int Verify(IntPtr queryTemplate, IntPtr referenceTemplate, out int verifyScore, int context)
        {
            int errorCode = 0;
            try
            {
                errorCode = GrFingerprintProxy.GrVerify(queryTemplate, referenceTemplate, out verifyScore, context);
            }
            catch (AccessViolationException exception)
            {
                FingerprintException exception2 = new FingerprintException(-113, exception);
                throw exception2;
            }
            FingerprintException.CheckError(errorCode);
            return errorCode;
        }
    }
}

