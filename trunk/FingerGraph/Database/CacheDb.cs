using System;

namespace FingerGraph.Database
{
    class CacheDb
    {

        private FingerprintCard[] CachedDatabase;

        private int capacity;
        private int loadfactor;


        public FingerprintCard[] getCacheDb()
        {
            if (CachedDatabase == null) return null;
            FingerprintCard[] returnarray = new FingerprintCard[loadfactor];
            Array.Copy(CachedDatabase, returnarray, loadfactor);
            return returnarray;
        }

        public void UpdateCache(FingerprintCard[] fprintarr)
        {
            loadfactor = fprintarr.Length;
            capacity = loadfactor + 30;
            CachedDatabase = new FingerprintCard[capacity];
            Array.Copy(fprintarr, CachedDatabase, loadfactor);
        }



        public void UpdateCard(FingerprintCard fcard)
        {
            if (CachedDatabase == null) return; // caching is redundant
            bool found = false;
            for (int i = 0; i < loadfactor; ++i)
            {
                if (CachedDatabase[i].guid == fcard.guid)
                {
                    CachedDatabase[i] = fcard;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                CachedDatabase[loadfactor] = fcard;
                ++loadfactor;
            }
            if (loadfactor == capacity)
            {
                increaseCache();
            }

        }

        public void increaseCache()
        {
            capacity += 50;
            FingerprintCard[] newarray = new FingerprintCard[capacity];
            Array.Copy(CachedDatabase, newarray, CachedDatabase.Length);
            CachedDatabase = newarray;
        }
    }
}
