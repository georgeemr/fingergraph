using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace FingerGraphDB
{

    class FingerDatabase : IDatabase
    {
        private CacheDb CachedDB = new CacheDb();

        public static byte[] SerializeImage(Image image)   
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, image.RawFormat);
            return ms.GetBuffer();
        }

        private static Image DeserializeImage(byte[] arr)  
        {
            MemoryStream ms = new MemoryStream(arr);
            Image im = Image.FromStream(ms);
            return im;
        }


        public void AddFingerprint(FingerprintCard card, Image newprint)
        {
            using (var newconnect = new DbConnection())
            {

                if (!newconnect.Connect()) throw new CannotConnectToDatabaseException();

                string query;
                if (card.FingerprintList.Count == 0) //card is empty => register new card
                {
                    query = "insert into usertbl values(\"" + card.guid + "\",\""
                    + card.FirstName + "\",\"" + card.LastName + "\");";
                    newconnect.SendQueryAndClose(query);
                }

                query = "insert into fprinttbl values(?File,\""
                              + card.guid + "\");";

                byte[] rawimg = SerializeImage(newprint);
                MySqlParameter pFile = new MySqlParameter("?File", rawimg);
                newconnect.SendQueryAndClose(query, pFile);
                card.FingerprintList.Add(newprint);
            }

            CachedDB.UpdateCard(card);
        }


        private List<Image> GetFingerprintList(Guid guid)
        {
            List<Image> resultlist = new List<Image>();
            string query = "select fprint from fprinttbl where userid=\"" + guid + "\";";
            DbConnection verynewconnect = new DbConnection();
            verynewconnect.Connect();
            MySqlDataReader reader = verynewconnect.SendQuery(query);

            while (reader.Read())
            {
                byte[] barr = (byte[]) reader.GetValue(0);
                resultlist.Add(DeserializeImage(barr));
            }
            reader.Close();
            verynewconnect.Close();
            return resultlist;

        }


        public FingerprintCard[] GetFingerprints()
        {

            FingerprintCard[] resultarray = CachedDB.getCacheDb(); //getCache
            if (resultarray != null) return resultarray;

            using (var newconnect = new DbConnection())
            {

                if (!newconnect.Connect()) throw new CannotConnectToDatabaseException();

                string query = "select * from usertbl;";
                MySqlDataReader reader = newconnect.SendQuery(query);
                List<FingerprintCard> fingerprintlist = new List<FingerprintCard>();

                while (reader.Read())
                {
                    FingerprintCard newcard = new FingerprintCard(reader.GetValue(1).ToString(), reader.GetValue(2).ToString());
                    string guidstr = (string)reader.GetValue(0);
                    newcard.guid = new Guid(guidstr);
                    newcard.FingerprintList = GetFingerprintList(newcard.guid);
                    fingerprintlist.Add(newcard);
                }
                reader.Close();
                resultarray = fingerprintlist.ToArray();
            }
            CachedDB.UpdateCache(resultarray);
            return resultarray;
        }
    }
}
