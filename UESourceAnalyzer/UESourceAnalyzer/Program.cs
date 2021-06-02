using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UESourceAnalyzer.PropertyCheck;

namespace UESourceAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                return;
            }

            var propertyCheck = new PropertyChecker();

            var outputs = new List<string>();

            var options = new EnumerationOptions();
            options.RecurseSubdirectories = true;
            foreach (var file in Directory.GetFiles(args[0], "*.h", options))
            {
                var lines = File.ReadAllLines(file);
                for (var i = 0; i < lines.Length; ++i)
                {
                    if (propertyCheck.IsMatch(lines[i]))
                    {
                        if (!propertyCheck.IsValid(i, lines))
                        {
                            outputs.Add("########################################");
                            outputs.Add($"{file}:{i}");
                            outputs.Add("------------------------------");
                            for (var j = Math.Max(0, i - 2); j < Math.Min(lines.Length, i + 2); ++j)
                            {
                                if (j == i)
                                {
                                    outputs.Add($"{lines[j]} <<<<<<<<<<<<");
                                }
                                else
                                {
                                    outputs.Add(lines[j]);
                                }
                            }
                            outputs.Add("");
                        }
                    }
                }
            }

            File.WriteAllLines(Path.Combine(args[0], "uproperty.txt"), outputs.ToArray(), Encoding.UTF8);
        }
    }
}
