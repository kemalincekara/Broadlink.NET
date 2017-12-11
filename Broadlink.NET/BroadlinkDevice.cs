using Sockets.Plugin;
using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Broadlink.NET
{
    /// <summary>
    /// Generic Broadlink device
    /// </summary>
    public class BroadlinkDevice : IDisposable
    {
        #region Fields & Events
        private MyUdpSocketReceiver ClientSocket;
        public event EventHandler OnDeviceReady;
        protected event EventHandler<byte[]> OnPayload;
        #endregion
        #region Properties
        public bool IsEventsReady { get; set; }

        /// <summary>
        /// <see cref="IPEndPoint"/> of this Broadlink device
        /// </summary>
        public IPEndPoint EndPoint { get; set; }

        /// <summary>
        /// <see cref="short"/> mapping to a Broadlink device type
        /// </summary>
        public short DeviceType { get; set; }

        /// <summary>
        /// MAC address of this device
        /// </summary>
        public byte[] MacAddress { get; set; }

        public string MacAddressStr => Regex.Replace(MacAddress.Slice(0, 5).Reverse().ToArray().ByteToHex(), "(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})", "$1:$2:$3:$4:$5:$6");

        /// <summary>
        /// Boolean indicating whether <see cref="AuthorizeAsync()"/> completed successfully
        /// </summary>
        /// <remarks>This should be true before other calls can be made</remarks>
        public bool IsAuthorized { get { return DeviceId != null && EncryptionKey != null; } }

        /// <summary>
        /// The Broadlink protocol uses a counter for the amount of packets that are sent
        /// </summary>
        public short PacketCount { get; private set; } = 1;

        /// <summary>
        /// This is sent with every call; for internal usage
        /// </summary>
        /// <remarks>Obtained by the <see cref="AuthorizeAsync()" /> method</remarks>
        public byte[] DeviceId { get; private set; }

        /// <summary>
        /// This is sent with every call; for internal usage
        /// </summary>
        /// <remarks>Obtained by the <see cref="AuthorizeAsync()" /> method</remarks>
        public byte[] EncryptionKey { get; private set; }
        #endregion
        #region Methods

        /// <summary>
        /// Obtain the device id and encryption key needed for other calls
        /// </summary>
        /// <remarks>Run this method before making any other calls</remarks>
        public async Task AuthorizeAsync()
        {
            if (ClientSocket == null)
                await Start_ClientListenAsync();

            byte[] authorizationPacket = PacketGenerator.GenerateAuthorizationPacket(this);
            await SendAsync(authorizationPacket);
        }
        public async Task KomutGonderAsync(byte[] veri) => await ClientSocket?.SendToAsync(veri, veri.Length, EndPoint);
        public async Task SendAsync(byte[] packet)
        {
            await KomutGonderAsync(packet);
            PacketCount++;
        }
        private async Task Start_ClientListenAsync()
        {
            try
            {
                bool internetVarmi = true;
                if (!internetVarmi)
                    HelperMy.Notification(Color.Red, "İnternet bağlantısı yok!");
                do
                {
                    internetVarmi = true;
                    if (internetVarmi)
                    {
                        ClientSocket = new MyUdpSocketReceiver();
                        ClientSocket.MessageReceived += new EventHandler<UdpReceiveResult>(ClientSocket_MessageReceived);
                        await ClientSocket.StartListeningAsync();
                    }
                    else
                        await Task.Delay(500);
                } while (!internetVarmi);
            }
            catch (Exception)
            {
                HelperMy.Notification(Color.Red, "Soket Bağlantısı Kurulamadı!");
            }
        }
        private void ClientSocket_MessageReceived(object sender, UdpReceiveResult e)
        {
            var encryptedResponse = e.Buffer;

            //var errorCode = BitConverter.ToInt16(encryptedResponse, 0x22);
            var errorCode = encryptedResponse[0x22] | (encryptedResponse[0x23] << 8);
            if (errorCode != 0)
            {
                //HelperMy.Bildirim(Color.Red, $"Error {errorCode} response");
                return;
            }

            var encryptedPayload = encryptedResponse.Slice(0x38);

            var payload = encryptedPayload.Decrypt(EncryptionKey);

            var command = encryptedResponse[0x26];

            if (command == 0xe9)
            {
                DeviceId = new byte[4];
                EncryptionKey = new byte[16];
                Array.Copy(payload, 0x00, DeviceId, 0, DeviceId.Length);
                Array.Copy(payload, 0x04, EncryptionKey, 0, EncryptionKey.Length);
                OnDeviceReady?.Invoke(this, null);
            }
            else if (command == 0xee || command == 0xef)
                OnPayload?.Invoke(this, payload);
        }
        public override string ToString()
        {
            switch (DeviceType)
            {
                case 0x2712: return "RM2";
                case 0x2737: return "RM Mini";
                case 0x273d: return "RM Pro Phicomm";
                case 0x2783: return "RM2 Home Plus";
                case 0x277c: return "RM2 Home Plus GDT";
                case 0x272a: return "RM2 Pro Plus";
                case 0x2787: return "RM2 Pro Plus2";
                case 0x278b: return "RM2 Pro Plus BL";
                case 0x278f: return "RM Mini Shate";
                default: return $"Bilinmeyen [{EndPoint.Address}]";
            }
        }

        public void Dispose()
        {
            if (ClientSocket != null)
            {
                ClientSocket.StopListeningAsync().Wait();
                ClientSocket.Dispose();
                ClientSocket = null;
            }
        }

        /// <summary>
        /// Setup a new Broadlink device via AP Mode. Review the README to see how to enter AP Mode.
        /// Only tested with Broadlink RM Pro 2017 RM03
        /// </summary>
        /// <param name="SSID">Modem SSID</param>
        /// <param name="Password">Modem Password</param>
        /// <param name="SecurityMode">Security mode options are (0 - none, 1 = WEP, 2 = WPA1, 3 = WPA2, 4 = WPA1/2)</param>
        public static async Task SetupAsync(string SSID, string Password, int SecurityMode)
        {
            var payload = new byte[0x88];
            payload[0x26] = 0x14;// This seems to always be set to 14;

            //Add the SSID to the payload
            int ssid_start = 68, ssid_length = 0;
            foreach (var letter in SSID)
            {
                payload[(ssid_start + ssid_length)] = (byte)letter;
                ssid_length++;
            }

            //Add the WiFi password to the payload
            int pass_start = 100, pass_length = 0;
            foreach (var letter in Password)
            {
                payload[(pass_start + pass_length)] = (byte)letter;
                pass_length++;
            }

            payload[0x84] = Convert.ToByte(ssid_length); // Character length of SSID
            payload[0x85] = Convert.ToByte(pass_length); // Character length of password
            payload[0x86] = Convert.ToByte(SecurityMode); // Type of encryption (00 - none, 01 = WEP, 02 = WPA1, 03 = WPA2, 04 = WPA1/2)

            var checksum = 0xbeaf;
            for (int i = 0; i < payload.Length; i++)
            {
                checksum += payload[i];
                checksum = checksum & 0xffff;
            }

            payload[0x20] = Convert.ToByte(checksum & 0xff); //Checksum 1 position
            payload[0x21] = Convert.ToByte(checksum >> 8); //Checksum 2 position

            using (var sock = new UdpClient(AddressFamily.InterNetwork))
            {
                sock.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                sock.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                foreach (var item in Network.GetNetworks())
                    await sock.SendAsync(payload, payload.Length, new IPEndPoint(item.BroadcastIPAddress, 80));
            }
            HelperMy.Notification(Color.Lime, "SSID ayarlandı.");
        }
        #endregion
    }
}
