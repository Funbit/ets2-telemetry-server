using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace Funbit.Ets.Telemetry.Server.Helpers
{
    public static class NetworkHelper
    {
        static NetworkInterface GetFirstNetworkInterface()
        {
            var card = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
            if (card == null)
                throw new InvalidOperationException("System does not have any registered network interfaces.");
            return card;
        }

        public static IPAddress GetDefaultGatewayIpAddress()
        {
            var card = GetFirstNetworkInterface();
            var address = card.GetIPProperties().GatewayAddresses
                .FirstOrDefault(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            if (address == null)
                throw new InvalidOperationException("Could not determine default gateway, network is not properly configured.");
            return address.Address;
        }

        public static IPAddress GetDefaultIpAddress()
        {
            var card = GetFirstNetworkInterface();
            var address = card.GetIPProperties().UnicastAddresses
                .FirstOrDefault(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            if (address == null)
                throw new InvalidOperationException("Could not determine default IPv4 address.");
            return address.Address;
        }
    }
}