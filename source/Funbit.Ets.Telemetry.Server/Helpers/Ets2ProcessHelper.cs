using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Funbit.Ets.Telemetry.Server.Helpers
{
    public static class Ets2ProcessHelper
    {
        static long _ets2LastCheckTime;
        private static long _atsLastCheckTime;
        static bool _ets2CachedRunningFlag;
        private static bool _atsCachedRunningFlag;

        /// <summary>
        /// Checks whether ETS2 game process is running right now. The maximum check frequency is restricted to 1 second.
        /// </summary>
        /// <returns>True if ETS2 process is running, false otherwise.</returns>
        public static bool IsEts2Running
        {
            get
            {
                if (DateTime.Now - new DateTime(Interlocked.Read(ref _ets2LastCheckTime)) > TimeSpan.FromSeconds(1))
                {
                    Interlocked.Exchange(ref _ets2LastCheckTime, DateTime.Now.Ticks);
                    var processes = Process.GetProcesses();
                    foreach (Process process in processes)
                    {
                        try
                        {
                            bool running = process.MainWindowTitle.StartsWith("Euro Truck Simulator 2") &&
                                           process.ProcessName == "eurotrucks2";

                            if (running)
                            {
                                _ets2CachedRunningFlag = true;
                                return _ets2CachedRunningFlag;
                            }
                        }
                        // ReSharper disable once EmptyGeneralCatchClause
                        catch
                        {
                        }
                    }
                    _ets2CachedRunningFlag = false;
                }
                return _ets2CachedRunningFlag;
            }
        }

        /// <summary>
        /// Checks whether ATS is running right now. The maximum check frequency is restricted to 1 second.
        /// </summary>
        /// <returns>True if ATS process is running, false otherwise.</returns>
        public static bool IsAtsRunning
        {
            get
            {
                if (DateTime.Now - new DateTime(Interlocked.Read(ref _atsLastCheckTime)) > TimeSpan.FromSeconds(1))
                {
                    Interlocked.Exchange(ref _atsLastCheckTime, DateTime.Now.Ticks);
                    var processes = Process.GetProcesses();
                    foreach (Process process in processes)
                    {
                        try
                        {
                            bool running = process.MainWindowTitle.StartsWith("American Truck Simulator") &&
                                           process.ProcessName == "amtrucks";

                            if (running)
                            {
                                _atsCachedRunningFlag = true;
                                return _atsCachedRunningFlag;
                            }
                        }
                        // ReSharper disable once EmptyGeneralCatchClause
                        catch
                        {
                        }
                    }
                    _atsCachedRunningFlag = false;
                }
                return _atsCachedRunningFlag;
            }
        }
    }
}