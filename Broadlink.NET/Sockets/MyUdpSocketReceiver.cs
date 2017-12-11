using Sockets.Plugin.Abstractions;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PclSocketException = Sockets.Plugin.Abstractions.SocketException;
using PlatformSocketException = System.Net.Sockets.SocketException;

namespace Sockets.Plugin
{
    /// <summary>
    ///     Listens on a port for UDP traffic and can send UDP data to arbitrary endpoints.
    /// </summary>
    public class MyUdpSocketReceiver : MyUdpSocketBase
    {
        private CancellationTokenSource _messageCanceller;

        /// <summary>
        ///     Binds the <code>UdpSocketServer</code> to the specified port on all endpoints and listens for UDP traffic.
        /// </summary>
        /// <param name="port">The port to listen on. If '0', selection is delegated to the operating system.</param>        
        /// <param name="listenOn">The <code>CommsInterface</code> to listen on. If unspecified, all interfaces will be bound.</param>
        /// <returns></returns>
        public override Task StartListeningAsync(int port = 0)
        {
            return Task
                .Run(() =>
                {
                    var _localEndPoint = new IPEndPoint(IPAddress.Any, port);

                    _messageCanceller = new CancellationTokenSource();

                    _backingUdpClient = new UdpClient(AddressFamily.InterNetwork)//(port, AddressFamily.InterNetwork)
                    {
                        EnableBroadcast = true,
                        MulticastLoopback = true,
                    };

                    _backingUdpClient.Client.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.NoDelay, 1);
                    _backingUdpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);

                    // The following three lines allow multiple clients on the same PC
                    _backingUdpClient.ExclusiveAddressUse = false;
                    _backingUdpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    _backingUdpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                    _backingUdpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, 1);
                    //_backingUdpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);
                    _backingUdpClient.ExclusiveAddressUse = false;
                    _backingUdpClient.Client.Bind(_localEndPoint);
                    _backingUdpClient.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 0);


                    ProtectAgainstICMPUnreachable(_backingUdpClient);

                    RunMessageReceiver(_messageCanceller.Token);
                })
                .WrapNativeSocketExceptions();
        }

        /// <summary>
        ///     Unbinds a bound <code>UdpSocketServer</code>. Should not be called if the <code>UdpSocketServer</code> has not yet
        ///     been unbound.
        /// </summary>
        public Task StopListeningAsync()
        {
            return Task.Run(() =>
            {
                _messageCanceller.Cancel();
                _backingUdpClient.Close();
            });
        }

        public new Task SendToAsync(byte[] data, IPEndPoint iPEndPoint) => SendToAsync(data, data.Length, iPEndPoint);
        public new async Task SendToAsync(byte[] data, int length, IPEndPoint iPEndPoint)
        {
            if (_backingUdpClient == null)
            {
                // haven't bound to a port, so _backingUdpClient has not been created
                // (must be created with the binding port as a parameter, so is 
                // instantiated on call to StartListeningAsync(). If we are here, user
                // is sending before having 'bound' to a port, so just create a temporary
                // backing client to send this data. 

                try
                {
                    _backingUdpClient = new UdpClient { EnableBroadcast = true };
                    ProtectAgainstICMPUnreachable(_backingUdpClient);
                }
                catch (PlatformSocketException ex)
                {
                    throw new PclSocketException(ex);
                }

                using (_backingUdpClient)
                {
                    await base.SendToAsync(data, length, iPEndPoint);
                }

                // clear _backingUdpClient because it has been disposed and is unusable. 
                _backingUdpClient = null;
            }
            else
            {
                await base.SendToAsync(data, length, iPEndPoint);
            }
        }

        /// <summary>
        ///     Sends the specified data to the endpoint at the specified address/port pair.
        /// </summary>
        /// <param name="data">A byte array of data to send.</param>
        /// <param name="address">The remote address to which the data should be sent.</param>
        /// <param name="port">The remote port to which the data should be sent.</param>
        public new Task SendToAsync(byte[] data, string address, int port)
        {
            return SendToAsync(data, data.Length, address, port);
        }

        /// <summary>
        ///     Sends the specified data to the endpoint at the specified address/port pair.
        /// </summary>
        /// <param name="data">A byte array of data to send.</param>
        /// <param name="length">The number of bytes from <c>data</c> to send.</param>
        /// <param name="address">The remote address to which the data should be sent.</param>
        /// <param name="port">The remote port to which the data should be sent.</param>
        public new async Task SendToAsync(byte[] data, int length, string address, int port)
        {
            if (_backingUdpClient == null)
            {
                // haven't bound to a port, so _backingUdpClient has not been created
                // (must be created with the binding port as a parameter, so is 
                // instantiated on call to StartListeningAsync(). If we are here, user
                // is sending before having 'bound' to a port, so just create a temporary
                // backing client to send this data. 

                try
                {
                    _backingUdpClient = new UdpClient { EnableBroadcast = true };
                    ProtectAgainstICMPUnreachable(_backingUdpClient);
                }
                catch (PlatformSocketException ex)
                {
                    throw new PclSocketException(ex);
                }

                using (_backingUdpClient)
                {
                    await base.SendToAsync(data, length, address, port);
                }

                // clear _backingUdpClient because it has been disposed and is unusable. 
                _backingUdpClient = null;
            }
            else
            {
                await base.SendToAsync(data, length, address, port);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (_messageCanceller != null && !_messageCanceller.IsCancellationRequested)
                _messageCanceller.Cancel();

            base.Dispose();
        }
    }
}