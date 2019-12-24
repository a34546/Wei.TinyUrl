using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wei.TinyUrl.Data
{
    public static class Utils
    {
        static readonly Random random = new Random();
        static readonly char[] charsArray = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
        static readonly int charsArrayLength = charsArray.Length;

        public static string GenerateCode(int length)
        {
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = charsArray[random.Next(charsArrayLength)];
            }

            return new string(result);
        }

        public static bool HasValue<T>(this IEnumerable<T> source)
        {
            if (source != null && source.Any()) return true;
            return false;
        }
    }
}
