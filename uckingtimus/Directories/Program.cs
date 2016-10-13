using System;
using System.Collections.Generic;
using System.Text;

namespace Directories
{
    class DirectoryTree
    {
        public readonly SortedDictionary<Directory, Directory> Directories =
            new SortedDictionary<Directory, Directory>();

        public void Print()
        {
            foreach (var dir in Directories.Keys)
            {
                for (var i = 0; i < dir.Depth; i++) Console.Write(' ');
                Console.WriteLine(dir.Name);
                if (dir.Childs == null) continue;
                dir.Childs.Print();
            }
        }

        public Directory Add(string name, int depth)
        {
            var dir = new Directory(name, depth);
            if (Directories.ContainsKey(dir)) return Directories[dir];
            Directories.Add(dir, dir);
            return dir;
        }
    }

    class Directory : IComparable
    {
        public readonly string Name;
        public readonly DirectoryTree Childs;
        public readonly int Depth;

        public Directory(string name, int depth)
        {
            Childs = new DirectoryTree();
            Name = name;
            Depth = depth;
        }

        public override bool Equals(object obj)
        {
            var dir = obj as Directory;
            return Name == dir.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            var dir = obj as Directory;
            return String.Compare(Name, dir.Name, StringComparison.Ordinal);
        }
    }

    class Program
    {
        static void Main()
        {
            var tree = new DirectoryTree();
            var dirsCount = int.Parse(Console.ReadLine());
            for (var i = 1; i < dirsCount + 1; i++)
            {
                var dirs = Console.ReadLine().Split('\\');
                var parent = tree.Add(dirs[0], 0);
                for (var j = 1; j < dirs.Length; j++)
                    parent = parent.Childs.Add(dirs[j], j);
            }
            tree.Print();
            Console.ReadKey();
        }
    }
}