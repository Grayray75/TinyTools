using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YarnUpgrader
{
    public static class Extensions
    {
        public static string GetPackagePath(this string[] arr)
        {
            string path = String.Empty;

            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].StartsWith("class_")) { break; }

                path += $"{arr[i]}/";
            }

            return path.Trim('/');
        }

        public static bool Contains(this char c, params char[] chars)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == c)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsDigitsOnly(this string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public static int CountChar(this string str, char c)
        {
            return str.Split(c).Length - 1;
        }
        public static int CountChars(this string str, params char[] chars)
        {
            return str.Split(chars).Length - 1;
        }
        public static int CountPattern(this string str, string pattern)
        {
            return str.Split(pattern).Length - 1;
        }
    }
}
