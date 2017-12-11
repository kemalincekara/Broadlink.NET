using Sockets.Plugin.Abstractions;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sockets.Plugin
{
    /// <summary>
    ///     Base class for .NET UDP socket wrapper.
    /// </summary>
    public abstract class MyUdpSocketBase
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        ///     Native socket field around which UdpSocketBase wraps.
        /// </summary>
        protected UdpClient _backingUdpClient;

        /// <summary>
        ///     Fired when a UDP datagram has been received.
        /// </summary>
        public event EventHandler<UdpReceiveResult> MessageReceived;

        public abstract Task StartListeningAsync(int port = 6742);

        public bool IsConnected()
        {
            try
            {
                return _backingUdpClient != null && !(_backingUdpClient.Client.Poll(10, SelectMode.SelectRead) && _backingUdpClient.Client.Available != 0);
            }
            catch (Exception) { return false; }
        }

        [System.Security.Permissions.HostProtection(ExternalThreading = true)]
        internal async void RunMessageReceiver(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var didReceive = false;
                var msg = new UdpReceiveResult();

                try
                {
                    // attempt to read next datagram
                    msg = await _backingUdpClient
                        .ReceiveAsync()
                        .WrapNativeSocketExceptions();

                    if (msg == default(UdpReceiveResult) && cancellationToken.IsCancellationRequested)
                        return;

                    didReceive = true;
                }
                catch
                {
                    // exception may occur because we stopped listening
                    // (i.e. cancelled the token) - if so exit loop
                    // otherwise throw.

                    if (cancellationToken.IsCancellationRequested)
                        return;
                }

                if (!didReceive)
                {
                    if (!IsConnected())
                        await StartListeningAsync();
                    //return; // cancelled, exit loop;
                }
                else
                    MessageReceived?.Invoke(this, msg);
            }
        }

        /// <summary>
        ///     Sends the specified data to the 'default' target of the underlying DatagramSocket.
        ///     There may be no 'default' target. depending on the state of the object.
        /// </summary>
        /// <param name="data">A byte array of data to be sent.</param>
        protected Task SendAsync(byte[] data)
        {
            return _backingUdpClient
                .SendAsync(data, data.Length)
                .WrapNativeSocketExceptions();
        }

        /// <summary>
        ///     Sends the specified data to the 'default' target of the underlying DatagramSocket.
        ///     There may be no 'default' target. depending on the state of the object.
        /// </summary>
        /// <param name="data">A byte array of data to be sent.</param>
        /// <param name="length">The number of bytes from <c>data</c> to send.</param>
        protected Task SendAsync(byte[] data, int length)
        {
            return _backingUdpClient
                .SendAsync(data, length)
                .WrapNativeSocketExceptions();
        }

        /// <summary>
        ///     Sends the specified data to the endpoint at the specified address/port pair.
        /// </summary>
        /// <param name="data">A byte array of data to send.</param>
        /// <param name="address">The remote address to which the data should be sent.</param>
        /// <param name="port">The remote port to which the data should be sent.</param>
        protected Task SendToAsync(byte[] data, string address, int port)
        {
            return _backingUdpClient
                .SendAsync(data, data.Length, address, port)
                .WrapNativeSocketExceptions();
        }

        /// <summary>
        ///     Sends the specified data to the endpoint at the specified address/port pair.
        /// </summary>
        /// <param name="data">A byte array of data to send.</param>
        /// <param name="length">The number of bytes from <c>data</c> to send.</param>
        /// <param name="address">The remote address to which the data should be sent.</param>
        /// <param name="port">The remote port to which the data should be sent.</param>
        protected Task SendToAsync(byte[] data, int length, string address, int port)
        {
            return _backingUdpClient
                .SendAsync(data, length, address, port)
                .WrapNativeSocketExceptions();
        }
        protected Task SendToAsync(byte[] data, IPEndPoint iPEndPoint)
        {
            return _backingUdpClient
                .SendAsync(data, data.Length, iPEndPoint)
                .WrapNativeSocketExceptions();
        }

        protected Task SendToAsync(byte[] data, int length, IPEndPoint iPEndPoint)
        {
            return _backingUdpClient
                .SendAsync(data, length, iPEndPoint)
                .WrapNativeSocketExceptions();
        }

        protected void ProtectAgainstICMPUnreachable(UdpClient udpClient)
        {

            // this should be called whenever the backing client is recreated.
            // it prevents a strange class of errors, some discussion at
            // http://stackoverflow.com/questions/7201862/an-existing-connection-was-forcibly-closed-by-the-remote-host

            uint IOC_IN = 0x80000000;
            uint IOC_VENDOR = 0x18000000;
            uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
            udpClient.Client.IOControl((int)SIO_UDP_CONNRESET, new[] { Convert.ToByte(false) }, null);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~MyUdpSocketBase()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_backingUdpClient != null)
                {
                    ((IDisposable)_backingUdpClient).Dispose();
                    _backingUdpClient = null;
                }
            }
        }

    }
}