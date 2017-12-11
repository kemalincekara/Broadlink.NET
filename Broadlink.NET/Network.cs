using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
namespace Broadlink.NET
{
    public class Network
    {
        public IPAddress BroadcastIPAddress { get; set; }
        public IPAddress LocalIPAddress { get; set; }
        public static IEnumerable<Network> GetNetworks()
        {
            foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
                if (adapter.Supports(NetworkInterfaceComponent.IPv4) && adapter.OperationalStatus == OperationalStatus.Up)
                    foreach (var unicast in adapter.GetIPProperties().UnicastAddresses)
                    {
                        Network model = null;
                        try
                        {
                            if (unicast.DuplicateAddressDetectionState == DuplicateAddressDetectionState.Preferred &&
                               unicast.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                var address = unicast.Address;
                                var mask = unicast.IPv4Mask;
                                var addressInt = BitConverter.ToInt32(address.GetAddressBytes(), 0);
                                if (mask == null) continue;
                                var maskInt = BitConverter.ToInt32(mask.GetAddressBytes(), 0);
                                var broadcastInt = addressInt | ~maskInt;
                                model = new Network
                                {
                                    LocalIPAddress = unicast.Address,
                                    BroadcastIPAddress = new IPAddress(BitConverter.GetBytes(broadcastInt))
                                };
                            }
                        }
                        catch (Exception)
                        {

                        }
                        if (model != null)
                            yield return model;
                    }
        }

        /// <summary>
        /// checks for used ports and retrieves the first free port
        /// https://gist.github.com/jrusbatch/4211535
        /// </summary>
        /// <returns>the free port or 0 if it did not find a free port</returns>
        public static List<int> GetAvailablePort(int startingPort, int count = 1)
        {
            IPEndPoint[] endPoints;
            List<int> portArray = new List<int>();

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            //getting active connections
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            portArray.AddRange(from n in connections
                               where n.LocalEndPoint.Port >= startingPort
                               select n.LocalEndPoint.Port);

            //getting active tcp listners - WCF service listening in tcp
            endPoints = properties.GetActiveTcpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            //getting active udp listeners
            endPoints = properties.GetActiveUdpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            portArray.Sort();

            List<int> model = new List<int>();
            for (int i = startingPort; i < UInt16.MaxValue; i++)
                if (!portArray.Contains(i))
                    if (count-- > 0)
                        model.Add(i);
                    else
                        break;
            return model;
        }
    }
}
