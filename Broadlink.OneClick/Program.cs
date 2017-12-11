using System;

namespace Broadlink.OneClick
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0) return;
            new App(args).Discover().Wait();
        }

    }
}
