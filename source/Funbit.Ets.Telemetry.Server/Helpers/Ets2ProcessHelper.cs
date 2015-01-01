using System;
using System.Diagnostics;
using System.Linq;

namespace Funbit.Ets.Telemetry.Server.Helpers
{
    public static class Ets2ProcessHelper
    {
        public static bool IsEts2Running()
        {
            var processes = Process.GetProcesses();
            foreach (Process process in processes) 
            {
                try
                {
                    if (process.MainWindowTitle == "Euro Truck Simulator 2" && 
                        process.ProcessName == "eurotrucks2")
                        return true;
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch 
                {
                }
            }
            return false;
        }
    }
}