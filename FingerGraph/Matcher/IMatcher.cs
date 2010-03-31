using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using FingerGraphDB;

namespace FingerGraph.Matcher {
    interface IMatcher {
        FingerprintCard Identify(Image fingerprint, FingerprintCard[] cards);
        bool Verify(Image fingerprint, FingerprintCard card);
    }
}