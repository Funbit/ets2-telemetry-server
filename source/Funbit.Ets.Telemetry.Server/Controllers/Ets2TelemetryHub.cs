using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Funbit.Ets.Telemetry.Server.Controllers
{
    public class Ets2TelemetryHub : Hub
    {
        static readonly ConcurrentDictionary<string, bool> ConnectedIds = new ConcurrentDictionary<string, bool>();

        public static bool HasConnections => ConnectedIds.Count > 0;
        
        static void ThrottleRequests()
        {
            // add some delay to throttle websocket requests to avoid CPU overhead
            Thread.Sleep(1);
        }

        public void RequestData()
        {
            if (HasConnections)
            {
                Clients.Caller.updateData(Ets2TelemetryController.GetEts2TelemetryJson());
                ThrottleRequests();
            }
        }

        public override Task OnConnected()
        {
            ConnectedIds.TryAdd(Context.ConnectionId, true);
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            ConnectedIds.TryAdd(Context.ConnectionId, true);
            Clients.Caller.updateData(Ets2TelemetryController.GetEts2TelemetryJson());
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            bool dummy;
            ConnectedIds.TryRemove(Context.ConnectionId, out dummy);
            return base.OnDisconnected(stopCalled);
        }
    }
}