using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace Funbit.Ets.Telemetry.Server.Data.Reader
{
    class SharedProcessMemory<T> : IDisposable
    {
        readonly string _mapName;

        public T Data { get; set; }

        public bool IsConnected
        {
            get
            {
                InitializeViewAccessor(); 
                return _memoryMappedAccessor != null;
            }
        }

        MemoryMappedFile _memoryMappedFile;
        MemoryMappedViewAccessor _memoryMappedAccessor;

        public SharedProcessMemory(string mapName)
        {
            _mapName = mapName;
            Data = default(T);
        }

        void InitializeViewAccessor()
        {
            if (_memoryMappedAccessor == null)
            {
                try
                {
                    _memoryMappedFile = MemoryMappedFile.OpenExisting(_mapName, MemoryMappedFileRights.ReadWrite);
                    _memoryMappedAccessor = _memoryMappedFile.CreateViewAccessor(0, Marshal.SizeOf(typeof(T)), MemoryMappedFileAccess.Read);
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                }
            }
        }

        public void Read()
        {
            InitializeViewAccessor();

            if (_memoryMappedAccessor == null)
                return;

            byte []rawData = new byte[Marshal.SizeOf(typeof (T))];

            _memoryMappedAccessor.ReadArray(0, rawData, 0, rawData.Length);
            
            T createdObject;

            IntPtr reservedMemPtr = IntPtr.Zero;
            try
            {
                reservedMemPtr = Marshal.AllocHGlobal(rawData.Length);
                Marshal.Copy(rawData, 0, reservedMemPtr, rawData.Length);
                createdObject = (T)Marshal.PtrToStructure(reservedMemPtr, typeof(T));
            }
            finally
            {
                if (reservedMemPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(reservedMemPtr);
            }

            Data = createdObject;
        }

        public void Dispose()
        {
            if (_memoryMappedAccessor != null)
            {
                _memoryMappedAccessor.Dispose();
                _memoryMappedFile.Dispose();
            }
        }
    }
}
