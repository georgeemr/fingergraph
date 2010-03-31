using System.Drawing;

namespace FingerGraphDB
{
    interface IDatabase
    {
        void AddFingerprint(FingerprintCard card, Image newprint);
        FingerprintCard[] GetFingerprints();        
    }
}