using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyFixer
{
    public static class Extensions
    {
        public static int CountChar(this string str, char c)
        {
            return str.Split(c).Length - 1;
        }

        public static int CountPattern(this string str, string pattern)
        {
            return str.Split(pattern).Length - 1;
        }
    }
}
