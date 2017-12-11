namespace Broadlink.NET
{
    public class Command
    {
        public string ID { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// IR veya RF hex kodu
        /// </summary>
        public string Code { get; set; }

        public override string ToString() => Name;
    }
}
