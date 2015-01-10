using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Funbit.Ets.Telemetry.Server.Helpers
{
    public static class AssemblyHelper
    {
        public static string Version
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = string.Format("{0}.{1}.{2}",
                    versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.ProductBuildPart);
                return version;
            }
        }
    }
}
