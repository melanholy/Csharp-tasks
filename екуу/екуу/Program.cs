using System;
using System.Collections.Generic;

namespace екуу
{
    public class TreeNode<T> where T : IComparable
    {
        public TreeNode<T> Left;
        public TreeNode<T> Right;
        public T Value;

        public int SubtreesCount
        {
            get
            {
                if (Left == null && Right == null) return 1;
                if (Left == null && Right != null) return Right.SubtreesCount + 1;
                if (Left != null && Right == null) return Left.SubtreesCount + 1;
                return Right.SubtreesCount + Left.SubtreesCount + 1;
            }
        }

        public TreeNode(T value)
        {
            Value = value;
        }
    }

    public class Tree<T> : IEnumerable<T> where T : IComparable
    {
        public TreeNode<T> Root;

        public void Add(T key)
        {
            if (Root == null) Root = new TreeNode<T>(key);
            else
            {
                var target = Root;
                while (true)
                    if (target.Value.CompareTo(key) > 0)
                        if (target.Left == null)
                        {
                            target.Left = new TreeNode<T>(key);
                            break;
                        }
                        else target = target.Left;
                    else if (target.Right == null)
                    {
                        target.Right = new TreeNode<T>(key);
                        break;
                    }
                    else target = target.Right;
            }
        }

        public bool Contains(T key)
        {
            var target = Root;
            while (target != null)
                if (target.Value.CompareTo(key) == 0) return true;
                else target = target.Value.CompareTo(key) > 0 ? target.Left : target.Right;
            return false;
        }

        public T this[int i]
        {
            get
            {
                if (i > Root.SubtreesCount || i < 0) throw new IndexOutOfRangeException();
                var target = Root;
                while (true)
                {
                    if (target.Left != null && target.Left.SubtreesCount == i || target.Left == null && i == 0) return target.Value;
                    if (target.Left != null && target.Left.SubtreesCount > i) target = target.Left;
                    else
                    {
                        i -= target.Left == null ? 1 : target.Left.SubtreesCount + 1;
                        target = target.Right;
                    }
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var stack = new Stack<TreeNode<T>>();
            var visited = new HashSet<TreeNode<T>>();
            var target = Root;
            stack.Push(target);
            while (true)
            {
                while (target.Left != null && !visited.Contains(target.Left))
                {
                    stack.Push(target.Left);
                    target = target.Left;
                }
                var a = stack.Pop();
                visited.Add(a);
                yield return a.Value;
                if (a.Right != null)
                {
                    target = a.Right;
                    stack.Push(target);
                }
                else if (stack.Count != 0) target = stack.Peek();
                else yield break;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class Program
    {
        static void Main()
        {
            var tree = new Tree<double> {5, 6, 5.5, 7, 3, 1, -1, 0, -5, -3, -4, -2};
            foreach(var e in tree)
                Console.WriteLine(e);
            Console.WriteLine();
            for (var i = 0; i < 10; i++)
                Console.WriteLine(tree[i]);
            Console.ReadKey();
        }
    }
}
