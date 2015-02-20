using System.Windows.Forms;

namespace Funbit.Ets.Telemetry.Server.Setup
{
    public interface ISetup
    {
        SetupStatus Status { get; }
        SetupStatus Install(IWin32Window owner);
        SetupStatus Uninstall(IWin32Window owner);
    }
}