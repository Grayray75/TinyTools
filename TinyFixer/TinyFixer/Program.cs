using System;
using System.Collections.Generic;
using System.IO;

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
                var splitly = line.Split('\t');

                if (splitly[0] == "CLASS")
                {
                    if (splitly.Length >= 3)
                    {
                        int a = splitly[1].CountChar('$');
                        int b = splitly[2].CountChar('$');
                        if (a >= 1 && b >= 0 && a > b)
                        {
                            Console.WriteLine("Found a broken line, class: '{0}'", splitly[1]);
                            string parent = splitly[1][..splitly[1].LastIndexOf('$')];
                            string pid = classMap[parent];

                            var ffarr = splitly[2].Split(new[] { '/', '$' });

                            fileContent[i] = $"{splitly[0]}\t{splitly[1]}\t{pid}${ffarr[ffarr.Length - 1]}";
                            Console.WriteLine("New line: '{0}'", fileContent[i]);
                            Console.WriteLine();

                            // split again to save lol
                            splitly = fileContent[i].Split('\t');
                            classMap.Add(splitly[1], splitly[2]);
                        }
                        else if (a >= 1 && b >= 0 && a <= b)
                        {
                            Console.WriteLine("Not a valid class line: {0}", line);
                        }
                        else
                        {
                            classMap.Add(splitly[1], splitly[2]);
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
