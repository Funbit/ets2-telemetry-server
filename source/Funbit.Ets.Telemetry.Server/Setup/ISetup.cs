namespace Funbit.Ets.Telemetry.Server.Setup
{
    public interface ISetup
    {
        SetupStatus Status { get; }
        SetupStatus Install();
        SetupStatus Uninstall();
    }
}