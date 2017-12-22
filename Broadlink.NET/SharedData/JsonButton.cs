using Newtonsoft.Json;
using System.IO;

namespace Broadlink.NET.SharedData
{
    public class JsonButton
    {
        public static JsonButton[] Read(string fileName = nameof(JsonButton))
        {
            try
            {
                if (File.Exists(fileName + ".json"))
                    fileName = fileName + ".json";
                if (!File.Exists(fileName))
                    return null;
                return File.ReadAllText(fileName, new System.Text.UTF8Encoding(false)).FromJson<JsonButton[]>();
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("subIRId")]
        public int SubIrId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("background")]
        public string Background { get; set; }

        public override string ToString()
        {
            if (!Name.IsNullOrEmptyTrim())
                return Name;
            switch (Index)
            {
                default:
                    return Name;
                case 0:
                    return "On/Off";
                case 6:
                    return "Mute";
                case 7:
                    return "Switch";
                case 8:
                    return "Back";
                case 9:
                    return "Up";
                case 10:
                    return "Left";
                case 11:
                    return "Down";
                case 12:
                    return "Right";
                case 13:
                    return "OK";
                case 14:
                    return "Vol+";
                case 15:
                    return "Vol-";
                case 16:
                    return "Channel+";
                case 17:
                    return "Channel-";
                case 18:
                    return "Exit";
                case 19:
                    return "Menu";
                case 100:
                case 101:
                case 102:
                case 103:
                case 104:
                case 105:
                case 106:
                case 107:
                case 108:
                case 109:
                    return (Index - 100).ToString();
                case 110:
                    return "*";
                case 111:
                    return "#";
            }
        }

        public static implicit operator string(JsonButton jsonButton) => jsonButton.ToString();
    }
}
