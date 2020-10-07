using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Flipsider.Core
{
    public static class Logger
    {
        // TODO log files
        public static void Warn(object message)
        {
            Debug.WriteLine(message);
        }

        public static void Fatal(object message)
        {
            Warn("Fatal error. " + message);
            Environment.Exit(1);
        }

        public static void Error(object message)
        {
            Warn(message);
            // TODO error handle
        }
    }
}
