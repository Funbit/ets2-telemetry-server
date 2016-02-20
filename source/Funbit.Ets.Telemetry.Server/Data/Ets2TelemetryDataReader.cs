using System;
using Funbit.Ets.Telemetry.Server.Data.Reader;

namespace Funbit.Ets.Telemetry.Server.Data
{
    public class Ets2TelemetryDataReader : IDisposable
    {
        /// <summary>
        /// ETS2 telemetry plugin maps the data to this mapped file name.
        /// </summary>
        const string Ets2TelemetryMappedFileName = "Local\\Ets2TelemetryServer";

        readonly SharedProcessMemory<Ets2TelemetryStructure> _sharedMemory = 
            new SharedProcessMemory<Ets2TelemetryStructure>(Ets2TelemetryMappedFileName);

        readonly Ets2TelemetryData _data = new Ets2TelemetryData();

        readonly object _lock = new object();

        // ReSharper disable once InconsistentNaming
        static readonly Lazy<Ets2TelemetryDataReader> instance = new Lazy<Ets2TelemetryDataReader>(
            () => new Ets2TelemetryDataReader());
        public static Ets2TelemetryDataReader Instance => instance.Value;

        public bool IsConnected => _sharedMemory.IsConnected;

        public IEts2TelemetryData Read()
        {
            lock (_lock)
            {
                _sharedMemory.Data = default(Ets2TelemetryStructure);
                _sharedMemory.Read();
                _data.Update(_sharedMemory.Data);
                return _data;
            }
        }
        
        public void Dispose()
        {
            _sharedMemory?.Dispose();
        }
    }
}