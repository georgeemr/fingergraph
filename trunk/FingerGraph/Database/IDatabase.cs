using System.Drawing;

namespace FingerGraph.Database
{
    interface IDatabase
    {
        void AddFingerprint(FingerprintCard card, Image newprint);
        FingerprintCard[] GetFingerprints();        
    }
}