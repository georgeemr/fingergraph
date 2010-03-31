namespace GriauleFingerprintLibrary
{
    using System;
    using System.ComponentModel;

    public enum GrEnrollState
    {
        [Description("Good enrollment.")]
        GR_ENROLL_GOOD = 2,
        [Description("Maximum limit of consolidated templates was reached.")]
        GR_ENROLL_MAX_LIMIT_REACHED = 4,
        [Description("Enrollment process not ready.")]
        GR_ENROLL_NOT_READY = 0,
        [Description("Sufficient enrollment.")]
        GR_ENROLL_SUFFICIENT = 1,
        [Description("Very good enrollment.")]
        GR_ENROLL_VERY_GOOD = 3
    }
}

