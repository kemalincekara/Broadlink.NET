using Newtonsoft.Json;
using System.IO;

namespace Broadlink.NET.SharedData
{
    public class JsonSubIr
    {
        public static JsonSubIr[] Read(string fileName = nameof(JsonSubIr))
        {
            try
            {
                if (File.Exists(fileName + ".json"))
                    fileName = fileName + ".json";
                if (!File.Exists(fileName))
                    return new JsonSubIr[0];
                return File.ReadAllText(fileName, new System.Text.UTF8Encoding(false)).FromJson<JsonSubIr[]>();
            }
            catch (System.Exception)
            {
                return new JsonSubIr[0];
            }
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("deviceId")]
        public int DeviceId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        public override string ToString()
        {
            switch (Type)
            {
                default:
                    return Name;
                case 0:
                    return "Cloud AC";
                case 1:
                    return "Audio";
                case 2:
                    return "Set Top Box";
                case 3:
                    return "TV";
                case 4:
                    return "TC";
                case 5:
                    return "User-defined";
                case 6:
                    return "Gesture Control ??";
                case 7:
                    return "User-Defined Aircon ??";
            }
        }

        public static implicit operator string(JsonSubIr jsonSubIr) => jsonSubIr.ToString();
    }
}
