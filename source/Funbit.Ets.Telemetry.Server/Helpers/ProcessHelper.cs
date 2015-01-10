using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Funbit.Ets.Telemetry.Server.Helpers
{
    public static class ProcessHelper
    {
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
                throw new Exception(string.Format("{0}: {1}", errorMessage, output));
            return output;
        }
    }
}