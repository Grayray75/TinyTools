using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YarnUpgrader.Tiny
{
    public class TinyField
    {
        public string OfficialName;
        public string IntermediaryName;

        public TinyClass Parent;

        public TinyField()
        {
            this.OfficialName = String.Empty;
            this.IntermediaryName = String.Empty;

            this.Parent = null;
        }

        public override string ToString()
        {
            return $"{TinyType.FIELD} {this.OfficialName} {this.IntermediaryName}";
        }
    }
}
