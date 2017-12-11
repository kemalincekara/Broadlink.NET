using System;
using System.Drawing;

namespace Broadlink.NET
{
    public static class HelperMy
    {
        public static event EventHandler<(Color Color, string Message, object[] FormatStringArgs)> NotificationEvent;
        public static void Notification(Color color, string message, params object[] FormatStringArgs)
        {
            if (NotificationEvent == null)
                Console.WriteLine(message, FormatStringArgs);
            else
                NotificationEvent(null, (color, message, FormatStringArgs));
        }


        #region Check Internet

        /// <summary>
        ///     Checks for available Connection | more reliable then pinging Google
        /// </summary>
        /// <param name="description"></param>
        /// <param name="reservedValue"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable => InternetGetConnectedState(out int description, 0);

        #endregion
    }
}
