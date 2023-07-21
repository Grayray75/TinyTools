using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YarnUpgrader.Tiny
{
    public class TinyFile
    {
        public readonly List<TinyClass> Classes = new List<TinyClass>();

        public int ClassCount
        {
            get
            {
                return this.Classes.Count;
            }
        }

        public int FieldCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < this.Classes.Count; i++)
                {
                    count += this.Classes[i].Fields.Count;
                }
                return count;
            }
        }

        public int MethodCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < this.Classes.Count; i++)
                {
                    count += this.Classes[i].Methodes.Count;
                }
                return count;
            }
        }
    }
}
