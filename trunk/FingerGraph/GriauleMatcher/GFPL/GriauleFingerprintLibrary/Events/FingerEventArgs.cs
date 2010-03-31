namespace GriauleFingerprintLibrary.Events
{
    using System;

    public class FingerEventArgs : EventArgs
    {
        private FingerEventType figerEventType;
        private string source;

        public FingerEventArgs(string source, FingerEventType fingerEventType)
        {
            this.source = source;
            this.figerEventType = fingerEventType;
        }

        public FingerEventType EventType
        {
            get
            {
                return this.figerEventType;
            }
        }

        public string Source
        {
            get
            {
                return this.source;
            }
        }
    }
}

