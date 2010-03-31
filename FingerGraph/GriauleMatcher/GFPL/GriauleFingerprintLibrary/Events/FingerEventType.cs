namespace GriauleFingerprintLibrary.Events
{
    using System;
    using System.ComponentModel;

    [Description("Enum of Finger Events types.")]
    public enum FingerEventType : short
    {
        [Description("A finger was placed over the fingerprint reader.")]
        FINGER_DOWN = 11,
        [Description("A finger was removed from the fingerprint reader")]
        FINGER_UP = 10
    }
}

