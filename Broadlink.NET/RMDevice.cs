using System;
using System.Threading.Tasks;

namespace Broadlink.NET
{
    /// <summary>
    /// Represents a remote control (IR/RF) device
    /// </summary>
    public class RMDevice : BroadlinkDevice
    {
        #region " Events "
        /// <summary>
        /// Get the temperature in degrees Celsius
        /// </summary>
        public event EventHandler<float> OnTemperature;

        /// <summary>
        /// Read the data for a remote control command.
        /// </summary>
        public event EventHandler<byte[]> OnRawData;
        public event EventHandler<byte[]> OnRawRFDataFirst;
        public event EventHandler<byte[]> OnRawRFDataSecond;
        public event EventHandler<byte[]> OnSentDataCallback;
        #endregion
        #region " Fields "
        private TimerMy timerCheckRFData;
        private TimerMy timerReadLearningData;
        #endregion
        #region " Constructor "
        public RMDevice()
        {
            OnPayload += RMDevice_OnPayload;
            timerCheckRFData = new TimerMy(5);
            timerCheckRFData.Elapsed += TimerCheckRFData_Elapsed;

            timerReadLearningData = new TimerMy(1);
            timerReadLearningData.Elapsed += TimerReadLearningData_Elapsed;
        }
        #endregion
        #region " Private Methods "
        private async void TimerReadLearningData_Elapsed(object sender, System.Timers.ElapsedEventArgs e) => await ReadLearningDataAsync();
        private async void TimerCheckRFData_Elapsed(object sender, System.Timers.ElapsedEventArgs e) => await CheckRFDataFirstAsync();
        private async void RMDevice_OnPayload(object sender, byte[] payload)
        {
            var param = payload[0];
            switch (param)
            {
                case 1: // OnTemperature
                    var temperatureData = payload.Slice(0x04, 0x05);
                    var temperature = temperatureData[0] + (float)temperatureData[1] / 10;
                    OnTemperature?.Invoke(this, temperature);
                    break;
                case 2:
                    OnSentDataCallback?.Invoke(this, payload.Slice(0x04));
                    break;
                case 4: //get from check_data
                    await ExitLearningModeAsync();
                    OnRawData?.Invoke(this, payload.Slice(0x04));
                    break;
                case 26: //get from check_data
                case 27: //get from check_data
                    var data = payload.Slice(0x04);
                    if (data[0] != 0x1) break;

                    if (payload[0] == 26)
                    {
                        //HelperMy.Bildirim(Color.Yellow, "Scan RF (found frequency - 1 of 2)");
                        //HelperMy.Bildirim(Color.Yellow, "[Keep holding that button!]");
                        OnRawRFDataFirst?.Invoke(this, data);
                        await CheckRFDataSecondAsync();
                    }
                    else
                    {
                        //HelperMy.Bildirim(Color.Yellow, "Scan RF (found frequency - 2 of 2)");
                        //HelperMy.Bildirim(Color.Yellow, "[Press the RF button multiple times with a pause between them]");
                        OnRawRFDataSecond?.Invoke(this, data);
                        timerCheckRFData.Stop();
                        timerReadLearningData.Start();
                    }
                    break;
            }
        }
        private async Task CheckRFDataFirstAsync() => await SendAsync(PacketGenerator.GenerateCheckRFDataFirstPacket(this));
        private async Task CheckRFDataSecondAsync() => await SendAsync(PacketGenerator.GenerateCheckRFDataSecondPacket(this));
        private async Task ExitRFLearningModeAsync() => await SendAsync(PacketGenerator.GenerateExitRFLearningModePacket(this));
        private async Task ExitIRLearningModeAsync() => await SendAsync(PacketGenerator.GenerateExitIRLearningModePacket(this));
        private async Task ReadLearningDataAsync() => await SendAsync(PacketGenerator.GenerateReadLearningModePacket(this));
        #endregion
        #region " Public Methods "

        /// <summary>
        /// Execute a remote control command
        /// </summary>
        /// <param name="data">Packet obtained using <see cref="OnRawData" /></param>
        public async Task SendRemoteCommandAsync(byte[] data) => await SendAsync(PacketGenerator.GenerateSendDataPacket(this, data));

        /// <summary>
        /// Get the temperature
        /// </summary>
        /// <returns>temperature in degrees Celsius. Event : OnTemperature</returns>
        public async Task GetTemperatureAsync() => await SendAsync(PacketGenerator.GenerateReadTemperaturePacket(this));

        /// <summary>
        /// Start the remote control command IR learning mode.
        /// </summary>
        /// <returns>Triggered event : <see cref="OnRawData" /></returns>
        public async Task EnterIRLearningModeAsync()
        {
            await SendAsync(PacketGenerator.GenerateStartIRLearningModePacket(this));
            timerCheckRFData.Stop();
            timerReadLearningData.Start();
        }

        /// <summary>
        /// Start the remote control command RF learning mode.
        /// </summary>
        /// <returns>Triggered event : <see cref="OnRawData" /></returns>
        public async Task EnterRFLearningModeAsync()
        {
            await SendAsync(PacketGenerator.GenerateEnterRFLearningModePacket(this));
            timerReadLearningData.Stop();
            timerCheckRFData.Start();
        }

        /// <summary>
        /// Exit IR and RF Learning  Mode
        /// </summary>
        /// <returns></returns>
        public async Task ExitLearningModeAsync()
        {
            timerCheckRFData.Stop();
            timerReadLearningData.Stop();
            await ExitRFLearningModeAsync();
            await ExitIRLearningModeAsync();
        }
        #endregion
    }
}
