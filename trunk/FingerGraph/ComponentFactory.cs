using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FingerGraph.Matcher;
using FingerGraphDB;
using FingerGraph.Scanner;

namespace FingerGraph {
    class ComponentFactory {
        public IMatcher CreateMatcher() { return null; }
        public IDatabase CreateDatabase() { return null; }
        public IScanner[] CreateSensors() { return null; }
    }
}
