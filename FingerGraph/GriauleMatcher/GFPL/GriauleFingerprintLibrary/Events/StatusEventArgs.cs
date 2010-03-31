namespace GriauleFingerprintLibrary.Events
{
    using System;

    public class StatusEventArgs : EventArgs
    {
        private string source;
        private GriauleFingerprintLibrary.Events.StatusEventType statusEventType;

        public StatusEventArgs(string source, GriauleFingerprintLibrary.Events.StatusEventType statusEventType)
        {
            this.source = source;
            this.statusEventType = statusEventType;
        }

        public string Source
        {
            get
            {
                return this.source;
            }
        }

        public GriauleFingerprintLibrary.Events.StatusEventType StatusEventType
        {
            get
            {
                return this.statusEventType;
            }
        }
    }
}

