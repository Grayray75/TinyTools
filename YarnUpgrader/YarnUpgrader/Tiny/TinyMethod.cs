using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YarnUpgrader.Tiny
{
    public class TinyMethod
    {
        public string OfficialName;
        public string IntermediaryName;
        public string Parameter;

        public TinyClass Parent;

        public TinyMethod()
        {
            this.OfficialName = String.Empty;
            this.IntermediaryName = String.Empty;
            this.Parameter = String.Empty;

            this.Parent = null;
        }

        public int ParameterCount
        {
            get { return TinyUtil.GetTinyParameterCount(this.Parameter); }
        }

        public override string ToString()
        {
            return $"{TinyType.METHOD} {this.OfficialName} {this.IntermediaryName}";
        }
    }
}
