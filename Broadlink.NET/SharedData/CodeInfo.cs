using System.Collections.Generic;
using System.Linq;
namespace Broadlink.NET.SharedData
{
    public class CodeInfo
    {
        #region JsonDevice
        public string Mac { get; set; }
        #endregion
        #region JsonSubIr
        public string RemoteName { get; set; }
        public int RemoteType { get; set; }
        #endregion
        #region JsonButton
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Index { get; set; }
        #endregion
        #region JsonIrCode
        public string Code { get; set; }
        public int Delay { get; set; }
        public int Order { get; set; }
        #endregion

        public CodeInfo(JsonDevice jsonDevice, JsonSubIr jsonSubIr, JsonButton jsonButton, JsonIrCode jsonIrCode)
        {
            if (!IsValid(jsonSubIr, jsonButton, jsonIrCode)) return;

            Mac = jsonDevice.Mac;

            RemoteName = jsonSubIr.Name;
            RemoteType = jsonSubIr.Type;

            Id = jsonButton.Id;
            Name = jsonButton;
            Type = jsonButton.Type;
            Index = jsonButton.Index;

            Delay = jsonIrCode.Delay;
            Order = jsonIrCode.Order;
            Code = jsonIrCode.CodeHex;
        }
        public static CodeInfo[] GetSharedData(
            string fileJsonDevice = nameof(JsonDevice), string fileJsonButton = nameof(JsonButton),
            string fileJsonIrCode = nameof(JsonIrCode), string fileJsonSubIr = nameof(JsonSubIr))
        {
            var jsonDevices = JsonDevice.Read();
            var jsonButtons = JsonButton.Read();
            var jsonIrCodes = JsonIrCode.Read();
            var jsonSubIrs = JsonSubIr.Read();
            if (jsonDevices == null || jsonDevices.Length == 0 ||
                jsonButtons == null || jsonButtons.Length == 0 ||
                jsonIrCodes == null || jsonIrCodes.Length == 0 ||
                jsonSubIrs == null || jsonSubIrs.Length == 0)
                return null;

            var model = new List<CodeInfo>();
            foreach (var subIr in jsonSubIrs)
                if (jsonDevices.FirstOrDefault(item => item.Id == subIr.DeviceId) is JsonDevice device)
                    foreach (var button in jsonButtons)
                        foreach (var irCode in jsonIrCodes)
                            if (IsValid(subIr, button, irCode) && !irCode.CodeHex.IsNullOrEmptyTrim() && irCode.CodeHex.Length > 0)
                                model.Add(new CodeInfo(device, subIr, button, irCode));
            return model.OrderBy(item => item.ToString()).ToArray();
        }
        public override string ToString() => $"{RemoteName} \u2022 {Name}";
        public static bool IsValid(JsonSubIr jsonSubIr, JsonButton jsonButton, JsonIrCode jsonIrCode) => jsonSubIr.Id != 0 && jsonButton.Id != 0 && jsonIrCode.ButtonId != 0 && jsonSubIr.Id == jsonButton.SubIrId && jsonIrCode.ButtonId == jsonButton.Id && jsonIrCode.CodeHex != null && jsonIrCode.CodeHex.Length > 0;
    }
}
