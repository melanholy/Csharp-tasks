using System;
using System.IO;
using System.Linq;

namespace Tests
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) Console.WriteLine("Please input correct arguments");
            else
            {
                if (!File.Exists(args[0])) Console.WriteLine("There is no " + args[0]);
                else
                {
                    var text = File.ReadAllLines(args[0]);
                    var f = int.Parse(args[1]);
                    foreach (var line in text)
                    {
                        var space = FindSpaces(line, f);
                        Console.WriteLine(space);
                    }
                }
            }
        }

        public static string FindSpaces(string line, int f)
        {
            var spaces = new string[line.Length];
            bool spaceInQuotes = false, spaceInApostrophes = false, screen = false;
            var spacesCount = 0;
            var memory = "";
            for (var i = 0; i < line.Length; i++)
            {
                var indicator = false;
                case1:
                if (i!= line.Length-1 && (spaceInQuotes || spaceInApostrophes) && line[i] == '\\' && (line[i + 1] == '\'' || line[i + 1] == '"' || line[i + 1] == '\\'))
                {
                    spaces[spacesCount] += line[i + 1];
                    i+=2;
                    goto case1;
                }
                if ((spaceInQuotes || spaceInApostrophes) && (line[i] != '\'' && line[i] != '"')) memory += line[i];
                else if (line[i] != '\'' && line[i] != '"' && line[i]!=' ') spaces[spacesCount] += line[i];
                if (line[i] == '\'' && !spaceInQuotes && !screen)
                {
                    if (spaceInApostrophes == true) indicator = true;
                    spacesCount++;
                    spaceInApostrophes = !spaceInApostrophes;
                }
                if (line[i] == '"' && !spaceInApostrophes && !screen)
                {
                    if (spaceInQuotes == true) indicator = true;
                    spacesCount++;
                    spaceInQuotes = !spaceInQuotes;
                }
                if ((!spaceInQuotes || !spaceInApostrophes) && memory.Length != 0)
                {
                    spaces[spacesCount] += memory;
                    memory = "";
                }
                if (line[i] == ' ' && !spaceInQuotes && !spaceInApostrophes || indicator == true) spacesCount++;
            }
            spaces = spaces.Where(x => x != null && x.TrimStart() != "").ToArray();
            if (f - 1 < spaces.Length) return spaces[f - 1];
            else return "null";
        }
    }
}
