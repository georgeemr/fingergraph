using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FingerGraph
{
    public class FingerprintCard
    {
        public FingerprintCard(string firstname, string lastname)
        {
            this.FirstName = firstname;
            this.LastName = lastname;
            this.FingerprintList = new List<Image>();
            this.guid = Guid.NewGuid();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Image> FingerprintList { get; set; }
        public Guid guid { get; set; }
    }
}
