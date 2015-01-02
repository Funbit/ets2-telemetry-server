using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Funbit.Ets.Telemetry.Server.Helpers
{
    public static class Ets2ProcessHelper
    {
        static long _lastCheckTime;
        static bool _cachedRunningFlag;

        /// <summary>
        /// Checks whether ETS2 game process is running right now. The maximum check frequency is restricted to 1 second.
        /// </summary>
        /// <returns>True if ETS2 process is run, false otherwise.</returns>
        public static bool IsEts2Running
        {
            get
            {
                if (DateTime.Now - new DateTime(Interlocked.Read(ref _lastCheckTime)) > TimeSpan.FromSeconds(1))
                {
                    Interlocked.Exchange(ref _lastCheckTime, DateTime.Now.Ticks);
                    var processes = Process.GetProcesses();
                    foreach (Process process in processes)
                    {
                        try
                        {
                            _cachedRunningFlag = process.MainWindowTitle.StartsWith("Euro Truck Simulator 2") &&
                                                 process.ProcessName == "eurotrucks2";
                            if (_cachedRunningFlag)
                                return _cachedRunningFlag;
                        }
                        // ReSharper disable once EmptyGeneralCatchClause
                        catch
                        {
                        }
                    }
                }
                return _cachedRunningFlag;
            }
        }
    }
}