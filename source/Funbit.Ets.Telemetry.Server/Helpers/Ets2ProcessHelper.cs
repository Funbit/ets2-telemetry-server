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
        /// The current simulator that is running. If both are running for some reason, this will return whichever simulator it checks first.
        /// </summary>
        /// <returns>"Euro Truck Simulator 2" if ETS2 is running, or "American Truck Simulator" is running.</returns>
        public static string RunningGame { get; set; }

        /// <summary>
        /// Checks whether ETS2 game process is running right now. The maximum check frequency is restricted to 1 second.
        /// </summary>
        /// <returns>True if ETS2 process is run, false otherwise.</returns>
        public static bool IsEts2OrAtsRunning
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
                            bool running = (process.MainWindowTitle.StartsWith("Euro Truck Simulator 2") &&
                                           process.ProcessName == "eurotrucks2")
                                           || (process.MainWindowTitle.StartsWith("American Truck Simulator") &&
                                           process.ProcessName == "amtrucks");

                            if (running)
                            {
                                RunningGame = process.MainWindowTitle;
                                _cachedRunningFlag = true;
                                return _cachedRunningFlag;
                            }
                        }
                        // ReSharper disable once EmptyGeneralCatchClause
                        catch
                        {
                        }
                    }
                    _cachedRunningFlag = false;
                }
                return _cachedRunningFlag;
            }
        }
    }
}