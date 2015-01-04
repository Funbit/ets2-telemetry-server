using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;

namespace Funbit.Ets.Telemetry.Server.Helpers
{
    public static class NetworkHelper
    {
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static IPAddress GetDefaultIpAddress(string networkInterfaceId = null)
        {
            Log.InfoFormat("Available network interfaces: {0}{1}",
                Environment.NewLine,
                string.Join(", " + Environment.NewLine,
                NetworkInterface.GetAllNetworkInterfaces().Select(a => string.Format("'{0}': '{1}' ({2})", 
                    a.Id, a.Name, a.OperationalStatus))));

            NetworkInterface card;
            if (string.IsNullOrWhiteSpace(networkInterfaceId))
            {
                card = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(a => a.OperationalStatus.ToString() == "Up");
                if (card == null)
                    throw new InvalidOperationException(
                        "System does not have any registered network interfaces that are connected to a network.");
            }
            else
            {
                card = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(a => a.OperationalStatus.ToString() == "Up" && a.Id == networkInterfaceId);
                if (card == null)
                    throw new InvalidOperationException(
                        string.Format("Network interface with Id '{0}' is not found.", networkInterfaceId));
            }
            
            var address = card.GetIPProperties().UnicastAddresses
                .FirstOrDefault(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            if (address == null)
                throw new InvalidOperationException(
                    "Could not determine default IPv4 address.");
            return address.Address;
        }
    }
}