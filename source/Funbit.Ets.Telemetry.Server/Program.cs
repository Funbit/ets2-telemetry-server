using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Funbit.Ets.Telemetry.Server
{
    static class Program
    {
        [DllImport("kernel32.dll", EntryPoint = "CreateMutexA")]
        private static extern int CreateMutex(Int32 lpMutexAttributes, int bInitialOwner, string lpName);
        [DllImport("kernel32.dll")]
        private static extern int GetLastError();
        private const int ErrorAlreadyExists = 183;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // check if another instance is running
            CreateMutex(0, -1, "Ets2Telemetry_8F63CCBE353DE22BD1A86308AD675001");
            bool bAnotherInstanceRunning = GetLastError() == ErrorAlreadyExists;
            if (bAnotherInstanceRunning) return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
