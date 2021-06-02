using System;
using System.Collections.Generic;
using System.Linq;

namespace UESourceAnalyzer.PropertyCheck
{
    internal class PropertyChecker
    {
        public FileType TargetFileType => FileType.CppHeader;

        public bool IsMatch(in string line)
        {
            if (line.Contains("#") || !line.Contains(";") || !line.Contains("*"))
            {
                return false;
            }

            if (line.Contains("virtual") || line.Contains("override") || line.Contains(");") || line.Contains("];")
                || line.Contains("UFUNCTION") || line.Contains("}") || line.Contains("const;"))
            {
                return false;
            }                    
            
            // TArrayとTMapはチェック
            if (line.Contains("TArray<A") || line.Contains("TArray<U") || line.Contains("TMap<"))
            {
                return true;
            }

            var words = line.Split(new[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            if (!words
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Any(x => x.Length >= 3 && (x[0] == 'A' || x[0] == 'U') && char.IsUpper(x[1]) && x.Last() == '*'))
            {
                return false;
            }

            return true;
        }

        public bool IsValid(in int lineNumber, in IReadOnlyList<string> allLines)
        {
            if (lineNumber < 2)
            {
                return true;
            }

            var previous = allLines[lineNumber - 1];
            var current = allLines[lineNumber];
            
            if (current.Contains("UPROPERTY"))
            {
                return true;
            }

            if (previous.Contains("UFUNCTION"))
            {
                return true;
            }

            if (previous.Contains("UPROPERTY"))
            {
                return true;
            }

            if (previous.Trim().StartsWith("//") && lineNumber > 3)
            {
                return allLines[lineNumber - 2].Contains("UPROPERTY");
            }

            return false;
        }
    }
}
