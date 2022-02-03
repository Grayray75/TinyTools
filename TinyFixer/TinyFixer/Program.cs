using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TinyFixer
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Directory.CreateDirectory("in");
            Directory.CreateDirectory("out");
            string[] files = Directory.GetFiles("./in", "*.tiny");

            foreach (var file in files)
            {
                FixFile(file);
            }

            Console.WriteLine();
            Console.WriteLine("Finished");
            Console.ReadLine();
        }

        private static void FixFile(string filePath)
        {
            Dictionary<string, string> classMap = new Dictionary<string, string>();

            string[] fileContent = File.ReadAllLines(filePath);

            for (int i = 0; i < fileContent.Length; i++)
            {
                string line = fileContent[i].Trim();
                string[] lineparts = line.Split('\t');

                if (lineparts[0] == "CLASS")
                {
                    if (lineparts.Length >= 3)
                    {
                        int a = lineparts[1].CountChar('$');
                        int b = lineparts[2].CountChar('$');
                        if (a >= 1 && b >= 0 && a > b)
                        {
                            Console.WriteLine("Found a broken line, class: '{0}'", lineparts[1]);
                            string parent = lineparts[1][..lineparts[1].LastIndexOf('$')];
                            string parentClassPath = classMap[parent];

                            string[] innerClassSplit = lineparts[2].Split(new[] { '/', '$' });

                            fileContent[i] = $"{lineparts[0]}\t{lineparts[1]}\t{parentClassPath}${innerClassSplit.Last()}";
                            Console.WriteLine("New line: '{0}'", fileContent[i]);
                            Console.WriteLine();

                            // split again to save
                            lineparts = fileContent[i].Split('\t');
                            classMap.Add(lineparts[1], lineparts[2]);
                        }
                        else if (a >= 1 && b >= 0 && a <= b)
                        {
                            Console.WriteLine("Not a valid/fixable line: {0}", line);
                        }
                        else
                        {
                            classMap.Add(lineparts[1], lineparts[2]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Not a valid class line: {0}", line);
                        Console.ReadLine();
                    }
                }
            }

            File.WriteAllLines(Path.Combine("out", Path.GetFileName(filePath)), fileContent);
        }
    }
}
