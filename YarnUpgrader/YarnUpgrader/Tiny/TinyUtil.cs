using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YarnUpgrader.Tiny
{
    public static class TinyUtil
    {
        public static int GetTinyParameterCount(string str)
        {
            int count = 0;

            int a = str.IndexOf('(');
            int b = str.IndexOf(')');
            string para = str.Substring(a + 1, b - a - 1);

            bool isL = false;
            for (int i = 0; i < para.Length; i++)
            {
                if (para[i] == 'L')
                {
                    isL = true;
                }
                else if (para[i] == ';')
                {
                    isL = false;
                }
                else
                {
                    if (!isL)
                    {
                        if (para[i].Contains('I', 'F', 'D', 'J', 'Z', 'B', 'C', 'S'))
                        {
                            count++;
                        }
                        else if (!para[i].Contains('['))
                        {
                            // DEBUG
                            throw new Exception("Unknown paramter type!");
                        }
                    }
                }
            }

            return count;
        }
    }
}
