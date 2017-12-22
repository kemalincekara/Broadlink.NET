using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Broadlink.NET
{
    public static class Extensions
    {
        public static bool IsNumeric(this string text) => System.Text.RegularExpressions.Regex.IsMatch(text, "^\\d+$");
        public static bool IsNullOrEmptyTrim(this string value) => string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim());
        public static string StringFormat(this string format, params object[] args) => string.Format(format, args);
        public static string ToJson(this object value, bool camelCase = false, string dateTimeFormatConverter = null) => JsonConvert.SerializeObject(value, GetJsonSettings(camelCase, dateTimeFormatConverter));
        public static T FromJson<T>(this string value, bool camelCase = false, string dateTimeFormatConverter = null) => JsonConvert.DeserializeObject<T>(value, GetJsonSettings(camelCase, dateTimeFormatConverter));
        public static JsonSerializerSettings GetJsonSettings(bool camelCase, string dateTimeFormatConverter = null)
        {
            var settings = new JsonSerializerSettings
            {
                Culture = CultureInfo.CurrentCulture,
                Error = (sender, args) =>
                {
                    args.ErrorContext.Handled = true;
                },
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };
            if (!dateTimeFormatConverter.IsNullOrEmptyTrim())
                settings.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = dateTimeFormatConverter });
            if (camelCase)
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return settings;
        }
        public static string FriendlyUrl(this string text)
        {
            if (text.IsNullOrEmptyTrim()) return string.Empty;
            text = Regex.Replace(text, @"\s+", "-");
            text = Regex.Replace(text, @"\-{2,}", "-");

            text = text.ToLower();
            text = Regex.Replace(text, @"&\w+;", "");
            text = Regex.Replace(text, @"[^a-z0-9\-\s]", "");
            text = text.Replace(' ', '-');
            text = Regex.Replace(text, @"-{2,}", "-");
            text = text.TrimStart(new[] { '-' });
            if (text.Length > 80)
                text = text.Substring(0, 79);
            text = text.TrimEnd(new[] { '-' });
            return text;
        }
    }
}
