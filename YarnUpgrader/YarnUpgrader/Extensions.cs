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
    }
}
