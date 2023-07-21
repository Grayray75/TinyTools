using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YarnUpgrader.Tiny
{
    public class TinyParser
    {
        private TinyFile _tinyFile = null;

        public TinyFile Parse(string filePath)
        {
            _tinyFile = new TinyFile();
            string[] fileLines = File.ReadAllLines(filePath);

            for (int i = 0; i < fileLines.Length; i++)
            {
                string line = fileLines[i].Trim();
                string[] splity = line.Split('\t');

                if (splity[0] == "CLASS")
                {
                    TinyClass tc = this.ParseClass(splity);

                    _tinyFile.Classes.Add(tc);
                    //Console.WriteLine(tc.ToString());
                }
                else if (splity[0] == "FIELD")
                {
                    TinyField tf = this.ParseField(splity);
                    tf.Parent.Fields.Add(tf);
                }
                else if (splity[0] == "METHOD")
                {
                    TinyMethod tm = this.ParseMethod(splity);
                    tm.Parent.Methodes.Add(tm);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Found {0} classes", _tinyFile.ClassCount);
            Console.WriteLine("Found {0} fields", _tinyFile.FieldCount);
            Console.WriteLine("Found {0} methodes", _tinyFile.MethodCount);

            return _tinyFile;
        }

        private TinyClass ParseClass(string[] columns)
        {
            TinyClass tc = new TinyClass();
            var firstSplit = columns[1].Split('$');
            if (firstSplit.Length > 1)
            {
                string pName = String.Join('$', firstSplit[..^1]);
                tc.Parent = _tinyFile.Classes.Find(tc => tc.FullOffical == pName);
            }
            tc.OfficialName = firstSplit.Last();

            var secondSplit = columns[2].Split(new[] { '/', '$' });
            tc.IntermediaryName = secondSplit.Last();
            tc.IntermediaryPackage = secondSplit.GetPackagePath();

            return tc;
        }

        private TinyField ParseField(string[] columns)
        {
            TinyField tf = new TinyField();
            tf.Parent = _tinyFile.Classes.Find(tc => tc.FullOffical == columns[1]);
            tf.OfficialName = columns[columns.Length - 2];
            tf.IntermediaryName = columns[columns.Length - 1];

            return tf;
        }

        private TinyMethod ParseMethod(string[] columns)
        {
            TinyMethod tm = new TinyMethod();
            tm.Parent = _tinyFile.Classes.Find(tc => tc.FullOffical == columns[1]);
            string a = tm.Parent.FullOffical;
            tm.OfficialName = columns[columns.Length - 2];
            tm.IntermediaryName = columns[columns.Length - 1];
            tm.Parameter = columns[2];

            return tm;
        }
    }
}
