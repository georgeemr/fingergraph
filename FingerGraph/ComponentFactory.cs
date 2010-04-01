using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FingerGraph.Database;
using FingerGraph.Matcher;
using FingerGraph.Scanner;
using FingerGraph.GriauleMatcher;
using FutronicDrv;

namespace FingerGraph {
    class ComponentFactory {
        public IMatcher CreateMatcher() { return new GriauleMatcher.GriauleMatcher(); }
        public IDatabase CreateDatabase() { return new FingerDatabase(); }
        public IScanner CreateScanner() { return new Device(); }
    }
}
