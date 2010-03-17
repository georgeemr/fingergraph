using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FingerGraph {
    internal class FingerprintCard {
        string FirstName { get; set; }
        string LastName { get; set; }
        byte[] Template { get; set; }
    }
}
