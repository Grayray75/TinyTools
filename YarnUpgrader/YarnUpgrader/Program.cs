using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using YarnUpgrader.Tiny;

namespace YarnUpgrader
{
    public class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            if (args.Length < 0)
            {
                args = new[] { "1.8.9.tiny-old.txt", "1.8.9.tiny-new.txt", "./mappings-old", "./mappings-new" };
            }
#endif

            if (args.Length < 4 || File.Exists(args[0]) || File.Exists(args[1]) || Directory.Exists(args[2]))
            {
                Console.WriteLine("Invalid args, usage: <old_tiny> <new_tiny> <old_mapping_dir> <new_mapping_dir>");
            }
            Console.WriteLine();

            Stopwatch watch = new Stopwatch();

            Console.WriteLine("Reading old tiny file...");
            TinyFile oldTiny = new TinyParser().Parse(args[0]);
            Console.WriteLine();

            Console.WriteLine("Reading new tiny file...");
            TinyFile newTiny = new TinyParser().Parse(args[1]);
            Console.WriteLine();

            string oldMapsDir = Path.GetFullPath(args[2]);
            string newMapsDir = Path.GetFullPath(args[3]);
            Directory.CreateDirectory(newMapsDir);

            Console.WriteLine("Generating changes maps...");
            Dictionary<string, string> classChanges = new Dictionary<string, string>();
            Dictionary<string, string> innerClassChanges = new Dictionary<string, string>();
            Dictionary<string, string> fieldChanges = new Dictionary<string, string>();
            Dictionary<string, string> methodChanges = new Dictionary<string, string>();

            for (int i = 0; i < oldTiny.Classes.Count; i++)
            {
                TinyClass oldClass = oldTiny.Classes[i];
                TinyClass newClass = newTiny.Classes.Find(tc => tc.FullOffical == oldClass.FullOffical);
                classChanges.Add(oldClass.FullIntermediary, newClass.FullIntermediary);

                if (oldClass.IntermediaryName.Contains("class"))
                {
                    innerClassChanges.Add(oldClass.IntermediaryName, newClass.IntermediaryName);
                }

                for (int k = 0; k < oldClass.Fields.Count; k++)
                {
                    TinyField oldField = oldClass.Fields[k];
                    if (oldField.OfficialName == oldField.IntermediaryName) continue; // Filter Java or named fields

                    TinyField newField = newClass.Fields.Find(tf => tf.OfficialName == oldField.OfficialName);
                    fieldChanges.Add(oldField.IntermediaryName, newField.IntermediaryName);
                }

                for (int m = 0; m < oldClass.Methodes.Count; m++)
                {
                    TinyMethod oldMethod = oldClass.Methodes[m];
                    if (oldMethod.OfficialName == oldMethod.IntermediaryName) continue; // Filter Java or named methodes

                    TinyMethod newMethod = newClass.Methodes.Find(tm => tm.OfficialName == oldMethod.OfficialName && tm.Parameter == oldMethod.Parameter);

                    if (methodChanges.ContainsKey(oldMethod.IntermediaryName))
                    {
                        // EDIT 1 year later: I have no idea what this means...
                        Console.WriteLine("Something wrong? " + oldMethod);
                    }
                    else
                    {
                        methodChanges.Add(oldMethod.IntermediaryName, newMethod.IntermediaryName);
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("Mapped {0} class changes", classChanges.Count);
            Console.WriteLine("Mapped {0} field changes", fieldChanges.Count);
            Console.WriteLine("Mapped {0} method changes", methodChanges.Count);
            Console.WriteLine();

            Console.WriteLine("Writting change map files...");
            Directory.CreateDirectory("./out");
            List<string> classChangesFile = new List<string>();
            List<string> fieldChangesFile = new List<string>();
            List<string> methodChangesFile = new List<string>();
            foreach (var (key, val) in classChanges)
            {
                classChangesFile.Add($"{key}\t=>\t{val}");
            }
            foreach (var (key, val) in fieldChanges)
            {
                fieldChangesFile.Add($"{key}\t=>\t{val}");
            }
            foreach (var (key, val) in methodChanges)
            {
                methodChangesFile.Add($"{key}\t=>\t{val}");
            }
            File.WriteAllLines(Path.Combine("./out", "classChanges.txt"), classChangesFile);
            File.WriteAllLines(Path.Combine("./out", "fieldChanges.txt"), fieldChangesFile);
            File.WriteAllLines(Path.Combine("./out", "methodChanges.txt"), methodChangesFile);
            Console.WriteLine();

            Console.WriteLine("Reading old mappings...");
            watch.Restart();
            Console.WriteLine();

            string[] mapFiles = Directory.GetFiles(oldMapsDir, "*.mapping", SearchOption.AllDirectories);
            int counter = 0;

            foreach (var filePath in mapFiles)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Writing file {++counter} of {mapFiles.Length}");

                string[] fileContent = File.ReadAllLines(filePath);
                for (int i = 0; i < fileContent.Length; i++)
                {
                    var lineSplit = fileContent[i].Split(new[] { ' ', ';', 'L' });

                    for (int k = 0; k < lineSplit.Length; k++)
                    {
                        if (classChanges.ContainsKey(lineSplit[k]))
                        {
                            fileContent[i] = fileContent[i].Replace(lineSplit[k], classChanges[lineSplit[k]]);
                        }
                        else if (innerClassChanges.ContainsKey(lineSplit[k]))
                        {
                            fileContent[i] = fileContent[i].Replace(lineSplit[k], innerClassChanges[lineSplit[k]]);
                        }
                        else if (fieldChanges.ContainsKey(lineSplit[k]))
                        {
                            fileContent[i] = fileContent[i].Replace(lineSplit[k], fieldChanges[lineSplit[k]]);
                        }
                        else if (methodChanges.ContainsKey(lineSplit[k]))
                        {
                            fileContent[i] = fileContent[i].Replace(lineSplit[k], methodChanges[lineSplit[k]]);
                        }
                    }
                }

                string relPath = Path.GetRelativePath(oldMapsDir, filePath);
                string newPath = Path.Combine(newMapsDir, relPath);
                Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                //File.WriteAllLines(newPath, fileContent);
                File.WriteAllText(newPath, String.Join("\n", fileContent) + "\n"); // Use only LF endings
            }

            watch.Stop();
            Console.WriteLine();
            Console.WriteLine($"Took: {watch.ElapsedMilliseconds}ms");
            Console.WriteLine();
            Console.WriteLine("Finished!");

            Console.ReadLine();
        }
    }
}
