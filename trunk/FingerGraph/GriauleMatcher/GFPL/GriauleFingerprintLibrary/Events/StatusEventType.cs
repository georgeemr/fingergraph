namespace GriauleFingerprintLibrary.Events
{
    using System;
    using System.ComponentModel;

    [Description("Enum of Status Events types.")]
    public enum StatusEventType : short
    {
        [Description("Event rised when the sensor is plugged in.")]
        SENSOR_PLUG = 0x15,
        [Description("Event rised when the sensor is plugged out.")]
        SENSOR_UNPLUG = 20
    }
}

