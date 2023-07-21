using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YarnUpgrader.Tiny
{
    public class TinyClass
    {
        public string OfficialName;
        public string IntermediaryName;
        public string IntermediaryPackage;
        public readonly List<TinyField> Fields;
        public readonly List<TinyMethod> Methodes;

        public TinyClass Parent;

        public TinyClass()
        {
            this.OfficialName = String.Empty;
            this.IntermediaryName = String.Empty;
            this.IntermediaryPackage = String.Empty;
            this.Fields = new List<TinyField>();
            this.Methodes = new List<TinyMethod>();

            this.Parent = null;
        }

        public string FullOffical
        {
            get
            {
                if (this.Parent is null)
                {
                    return this.OfficialName;
                }
                else
                {
                    return this.Parent.FullOffical + '$' + this.OfficialName;
                }
            }
        }

        public string FullIntermediary
        {
            get
            {
                if (this.Parent is null)
                {
                    return $"{this.IntermediaryPackage}/{this.IntermediaryName}";
                }
                else
                {
                    return this.Parent.FullIntermediary + '$' + this.IntermediaryName;
                }
            }
        }

        public override string ToString()
        {
            return $"{TinyType.CLASS} {this.FullOffical} {this.FullIntermediary}";
        }
    }
}
