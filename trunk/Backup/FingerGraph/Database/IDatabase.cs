using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FingerGraph.Database {
    internal interface IDatabase {
        void AddFingerprint(FingerprintCard card);
        FingerprintCard[] GetFingerprints();
    }
}
