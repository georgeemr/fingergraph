using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FingerGraph.Sensor {
    internal delegate void ImageReceivedHandler(object sender, Image img);

    internal interface ISensor {
        event ImageReceivedHandler ImageReceived;
    }
}
