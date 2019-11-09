using System;

namespace Rider.RemoteSimulator.Win.Core
{
    public static class StringExtensions
    {
        public static int ToInt(this string numero, int defVal = 0)
        {
            if (!Int32.TryParse(numero, out var num))
            {
                num = defVal;
            }
            return num;
        }
    }
}
