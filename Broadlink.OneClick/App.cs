using Broadlink.NET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broadlink.OneClick
{
    public class App
    {
        #region Fields
        private int Timeout = 5;
        private Client DiscoverClient;
        private TimerMy timerDiscover;
        private TaskCompletionSource<bool> IsComplete;
        private string[] Args;
        #endregion
        public static string App_Path => string.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath) ? AppDomain.CurrentDomain.BaseDirectory : AppDomain.CurrentDomain.RelativeSearchPath;
        public App(string[] args)
        {
            Args = args;
            IsComplete = new TaskCompletionSource<bool>();
            timerDiscover = new TimerMy(1500, TimerMy.Froms.FromMilliseconds);
            timerDiscover.Elapsed += async (sender, e) =>
            {
                if (Timeout-- < 0)
                    IsComplete.TrySetResult(false);
                else
                    await Discover();
            };
            if (DiscoverClient == null)
            {
                DiscoverClient = new Client();
                DiscoverClient.DeviceHandler += DiscoverClient_DeviceHandler;
                timerDiscover.Start();
            }
        }
        public async Task Discover()
        {
            if (DiscoverClient != null)
            {
                await DiscoverClient.DiscoverAsync();
                await IsComplete.Task;
            }
        }

        private async void DiscoverClient_DeviceHandler(object sender, BroadlinkDevice device)
        {
            if (device is RMDevice rm)
            {
                timerDiscover.Stop();
                DiscoverClient.Dispose();
                DiscoverClient = null;
                rm.OnDeviceReady += async (s, e) =>
                {
                    var filePath = Path.Combine(App_Path, "Komutlar.json");
                    if (File.Exists(filePath))
                    {
                        var commands = File.ReadAllText(filePath, new UTF8Encoding(false)).FromJson<List<Command>>();
                        if (commands != null)
                        {
                            foreach (var i in Args)
                            {
                                var cmd = commands.FirstOrDefault(j => j.ID == i);
                                if (cmd != null)
                                {
                                    await (rm as RMDevice).SendRemoteCommandAsync(cmd.Code.HexToBytes());
                                    await Task.Delay(100);
                                }
                            }
                        }
                    }
                    rm.Dispose();
                    IsComplete.TrySetResult(true);
                };
                await rm.AuthorizeAsync();
            }
        }
    }
}
