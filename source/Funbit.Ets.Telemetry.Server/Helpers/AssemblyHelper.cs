using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Funbit.Ets.Telemetry.Server.Helpers
{
    public static class AssemblyHelper
    {
        public static string Version
        {
            get
            {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(
                    Process.GetCurrentProcess().MainModule.FileName);
                string version = string.Format("{0}.{1}.{2}",
                    versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.ProductBuildPart);
                return version;
            }
        }
    }
}
