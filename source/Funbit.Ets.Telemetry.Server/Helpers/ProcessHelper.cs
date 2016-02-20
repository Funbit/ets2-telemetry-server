using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Funbit.Ets.Telemetry.Server.Helpers
{
    public static class ProcessHelper
    {
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void OpenUrl(string url)
        {
            var info = new ProcessStartInfo
            {
                FileName = GetAssociatedExecutablePath(".htm"),
                Arguments = url,
                UseShellExecute = true
            };
            Process.Start(info);
        }

        public static int RunAndWait(
            out string output, out string error,
            string exeFileName, string arguments,
            int timeoutInSeconds = 10)
        {
            var info = new ProcessStartInfo(exeFileName, arguments)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            var outputBuilder = new StringBuilder();
            var errorBuiler = new StringBuilder();

            var exited = false;

            Log.InfoFormat("Running command line: \r\n{0} {1}", exeFileName, arguments);

            using (var outputWaitHandle = new AutoResetEvent(false))
            using (var errorWaitHandle = new AutoResetEvent(false))
            using (var process = Process.Start(info))
            {
                if (process == null || process.Handle == IntPtr.Zero)
                    throw new Exception("Failed to start process: " + exeFileName);

                // ReSharper disable AccessToDisposedClosure
                process.OutputDataReceived += (sender, a) =>
                {
                    if (a.Data == null)
                        outputWaitHandle.Set();
                    else
                        outputBuilder.AppendLine(a.Data);
                };
                process.ErrorDataReceived += (sender, a) =>
                {
                    if (a.Data == null)
                        errorWaitHandle.Set();
                    else
                        errorBuiler.AppendLine(a.Data);
                };

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                var cancellationSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutInSeconds));
                cancellationSource.Token.Register(
                    () =>
                    {
                        // ReSharper disable once AccessToModifiedClosure
                        if (!exited)
                            process.Kill();
                    });

                // ReSharper restore AccessToDisposedClosure
                process.WaitForExit();
                exited = true;
                outputWaitHandle.WaitOne();
                errorWaitHandle.WaitOne();

                output = outputBuilder.ToString();
                error = errorBuiler.ToString();

                return process.ExitCode;
            }
        }

        public static string RunNetShell(string arguments, string errorMessage)
        {
            const string netShellExe = @"netsh";
            string output, error;

            int code = RunAndWait(out output, out error, netShellExe, arguments);
            if (code != 0)
                throw new Exception($"{errorMessage}: {output}");
            return output;
        }

        [DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
        static extern uint AssocQueryString(AssocF flags, AssocStr str, string pszAssoc, string pszExtra, [Out] StringBuilder pszOut, ref uint pcchOut);

        public static string GetAssociatedExecutablePath(string extension)
        {
            uint pcchOut = 0;
            AssocQueryString(AssocF.Verify, AssocStr.Executable, extension, null, null, ref pcchOut);
            StringBuilder pszOut = new StringBuilder((int)pcchOut);
            AssocQueryString(AssocF.Verify, AssocStr.Executable, extension, null, pszOut, ref pcchOut);
            return pszOut.ToString();
        }

        [Flags]
        public enum AssocF
        {
            InitNoRemapClsid = 0x1,
            InitByExeName = 0x2,
            OpenByExeName = 0x2,
            InitDefaultToStar = 0x4,
            InitDefaultToFolder = 0x8,
            NoUserSettings = 0x10,
            NoTruncate = 0x20,
            Verify = 0x40,
            RemapRunDll = 0x80,
            NoFixUps = 0x100,
            IgnoreBaseClass = 0x200
        }
        
        public enum AssocStr
        {
            Command = 1,
            Executable,
            FriendlyDocName,
            FriendlyAppName,
            NoOpen,
            ShellNewValue,
            DdeCommand,
            DdeIfExec,
            DdeApplication,
            DdeTopic
        }
    }
}