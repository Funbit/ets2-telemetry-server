using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;

namespace Funbit.Ets.Telemetry.Server.Helpers
{
    public static class NetworkHelper
    {
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static NetworkInterfaceInfo[] GetAllActiveNetworkInterfaces()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();

            Log.InfoFormat("Found following network interfaces: {0}{1}", Environment.NewLine,
                string.Join(", " + Environment.NewLine,
                    interfaces.Select(a => $"'{a.Id}': '{a.Name}' ({a.OperationalStatus})")));

            var foundInterfaces = interfaces.Where(
                    a => a.OperationalStatus.ToString() == "Up" &&
                    a.GetIPProperties()
                     .UnicastAddresses.Any(ua => ua.Address.AddressFamily == AddressFamily.InterNetwork))
                     .Select(i => new NetworkInterfaceInfo
                     {
                         Id = i.Id,
                         Name = i.Name,
                         Ip = i.GetIPProperties().UnicastAddresses
                            .First(ua => ua.Address.AddressFamily == AddressFamily.InterNetwork).Address.ToString()
                     }).ToArray();

            if (!foundInterfaces.Any())
                throw new Exception(
                    "System does not have any registered network interfaces that are connected to a network.");

            return foundInterfaces;
        }
    }

    public class NetworkInterfaceInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}