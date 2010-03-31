namespace GriauleFingerprintLibrary.Exceptions
{
    using System;

    public class FingerprintException : ApplicationException
    {
        private int errorCode;

        public FingerprintException(int errorCode) : this(GetMessage(errorCode) + "(error " + errorCode.ToString() + ")")
        {
            this.errorCode = errorCode;
        }

        public FingerprintException(string message) : base(message)
        {
        }

        public FingerprintException(int errorCode, Exception innerException) : this(GetMessage(errorCode) + "(error " + errorCode.ToString() + ")", innerException)
        {
            this.errorCode = errorCode;
        }

        public FingerprintException(string message, Exception innerException) : base(message, innerException)
        {
        }

        internal static int CheckError(int errorCode)
        {
            if (errorCode < 0)
            {
                throw new FingerprintException(errorCode);
            }
            return errorCode;
        }

        internal static string GetMessage(int errorCode)
        {
            switch (errorCode)
            {
                case -312:
                    return "Product key not approved yet.";

                case -311:
                    return "Product key not linked with a Griaule Account.";

                case -310:
                    return "Unable to write the license file.";

                case -309:
                    return "Internal server error.";

                case -308:
                    return "Wrong product key.";

                case -307:
                    return "HTTP authentication failed.";

                case -306:
                    return "No hardware-bound license.";

                case -305:
                    return "Insufficient credit.";

                case -304:
                    return "Invalid product key.";

                case -303:
                    return "Bad request.";

                case -302:
                    return "Error internet connection.";

                case -301:
                    return "Unable to get your hardware key.";

                case -209:
                    return "Fingerprint reader error.";

                case -208:
                    return "Invalid file type.";

                case -207:
                    return "Invalid filename";

                case -206:
                    return "Invalid file extension.";

                case -205:
                    return "Capture wasn't started on the fingerprint reader.";

                case -204:
                    return "Invalid fingerprint reader ID.";

                case -203:
                    return "Capture has been canceled.";

                case -202:
                    return "Error while acquiring image.";

                case -201:
                    return "Error connecting to the fingerprint reader.";

                case -114:
                    return "Supplied template buffer is too small to hold the template.";

                case -113:
                    return "General, unexpected or unknown error.";

                case -112:
                    return "Context isn't valid.";

                case -111:
                    return "Context couldn't be created.";

                case -110:
                    return "Image resolution is out of the valid range.";

                case -109:
                    return "Image size is too big.";

                case -108:
                    return "Error extracting template.";

                case -107:
                    return "Function can't be called at this time.";

                case -9:
                    return "This trial license expired.";

                case -8:
                    return "An incorrect parameter was supplied.";

                case -7:
                    return "Memory allocation failure.";

                case -6:
                    return "Unexpected failure.";

                case -5:
                    return "A null parameter was supplied.";

                case -4:
                    return "No valid license found.";

                case -3:
                    return "GrFinger couldn't read the license file.";

                case -2:
                    return "GrFinger isn't initialized.";

                case -1:
                    return "Initialization failed.";
            }
            return "Unknow Error";
        }

        public int ErrorCode
        {
            get
            {
                return this.errorCode;
            }
        }
    }
}

