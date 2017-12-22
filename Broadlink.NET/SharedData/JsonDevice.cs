using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace Broadlink.NET.SharedData
{
    public class JsonDevice
    {
        public static JsonDevice[] Read(string fileName = nameof(JsonDevice))
        {
            try
            {
                if (File.Exists(fileName + ".json"))
                    fileName = fileName + ".json";
                if (!File.Exists(fileName))
                    return null;
                return File.ReadAllText(fileName, new System.Text.UTF8Encoding(false)).FromJson<JsonDevice[]>();
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        private string _Mac = "";
        [JsonProperty("mac")]
        public string Mac
        {
            get => _Mac;
            set => _Mac = value.IndexOf(':') > 0 ? value.ToUpper() : Regex.Replace(value, "(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})", "$1:$2:$3:$4:$5:$6").ToUpper();
        }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("password")]
        public int Password { get; set; }

        [JsonProperty("lock")]
        public int Lock { get; set; }


        [JsonProperty("subDevice")]
        public int SubDevice { get; set; }

        [JsonProperty("terminalId")]
        public int TerminalId { get; set; }

        [JsonProperty("publicKey")]
        public int[] PublicKey { get; set; }
    }
}
