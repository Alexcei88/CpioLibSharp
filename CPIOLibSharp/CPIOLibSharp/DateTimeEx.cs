using System;

namespace CPIOLibSharp
{
    internal static class DateTimeEx
    {
        public static DateTime ToUnixTime(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}