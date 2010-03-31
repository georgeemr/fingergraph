namespace GriauleFingerprintLibrary
{
    using System;
    using System.ComponentModel;

    public enum GrCaptureImageFormat
    {
        [Description("Windows Bitmap (BMP) image format.")]
        GRCAP_IMAGE_FORMAT_BMP = 0x1f5,
        [Description("Raw image format.")]
        GRCAP_IMAGE_FORMAT_RAW = 0x1f7,
        [Description("Tagged Image File Format (TIFF) image format.")]
        GRCAP_IMAGE_FORMAT_TIFF = 0x1f6
    }
}

