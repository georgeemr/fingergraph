namespace GriauleFingerprintLibrary
{
    using System;
    using System.ComponentModel;

    public enum GrLicense
    {
        [Description("GrFinger 4.1 FREE license agreement.")]
        GRFINGER_FREE = 3,
        [Description("Fingerprint IDENTIFICATION SDK license agreement.")]
        GRFINGER_FULL = 1,
        [Description("Fingerprint VERIFICATION SDK license agreement.")]
        GRFINGER_LIGHT = 2
    }
}

