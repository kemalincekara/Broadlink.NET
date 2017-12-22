using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Broadlink.NET.SharedData
{
    public class JsonIrCode
    {
        public static JsonIrCode[] Read(string fileName = nameof(JsonIrCode))
        {
            try
            {
                if (File.Exists(fileName + ".json"))
                    fileName = fileName + ".json";
                if (!File.Exists(fileName))
                    return new JsonIrCode[0];
                return File.ReadAllText(fileName, new System.Text.UTF8Encoding(false)).FromJson<JsonIrCode[]>();
            }
            catch (System.Exception)
            {
                return new JsonIrCode[0];
            }
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("buttonId")]
        public int ButtonId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("delay")]
        public int Delay { get; set; }

        [JsonIgnore()]
        public string CodeHex => string.Join("", CodeInt.SelectMany(item => (item & 0xff).ToString("X2")));

        [JsonProperty("code")]
        public int[] CodeInt { get; set; }
    }
}
