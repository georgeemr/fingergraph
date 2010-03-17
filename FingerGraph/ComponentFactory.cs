using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FingerGraph.Matcher;
using FingerGraph.Database;
using FingerGraph.Sensor;

namespace FingerGraph {
    class ComponentFactory {
        public IMatcher CreateMatcher() { return null; }
        public IDatabase CreateDatabase() { return null; }
        public ISensor[] CreateSensors() { return null; }
    }
}
