using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using екуу;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAddAndContains()
        {
            var tree = new Tree<int> {5, 3, 6, 5, 7, 1, 4, -1, 0};
            Assert.AreEqual(true, tree.Contains(0));
            Assert.AreEqual(true, tree.Contains(3));
            Assert.AreEqual(true, tree.Contains(7));
            Assert.AreEqual(true, tree.Contains(-1));
            Assert.AreEqual(true, tree.Contains(4));
            Assert.AreEqual(true, tree.Contains(6));
            Assert.AreEqual(false, tree.Contains(2));
            Assert.AreEqual(false, tree.Contains(-100));
            Assert.AreEqual(false, tree.Contains(6546546));
            Assert.AreEqual(false, tree.Contains(634543));
        }

        [TestMethod]
        public void TestSpeedHundredThousand()
        {
            var tree = new Tree<int>();
            var rnd = new Random();
            for (var i = 0; i < 100000; i++)
                tree.Add(rnd.Next(int.MinValue, int.MaxValue));
        }

        [TestMethod]
        public void TestSpeedTwoHundredThousand()
        {
            var tree = new Tree<int>();
            var rnd = new Random();
            for (var i = 0; i < 200000; i++)
                tree.Add(rnd.Next(int.MinValue, int.MaxValue));
        }

        [TestMethod]
        public void TestSpeedFourHundredThousand()
        {
            var tree = new Tree<int>();
            var rnd = new Random();
            for (var i = 0; i < 400000; i++)
                tree.Add(rnd.Next(int.MinValue, int.MaxValue));
        }

        [TestMethod]
        public void TestIEnumerable()
        {
            var tree = new Tree<int> { 5, 6, 7, 3, 1, -1, 0, -5, -3, -4, -2 };
            CollectionAssert.AreEqual(tree.ToList(), new List<int> { -5, -4, -3, -2, -1, 0, 1, 3, 5, 6, 7 });
            var strtree = new Tree<string> { "s", "c", "x", "a", "f", "t", "z", "b", "d" };
            CollectionAssert.AreEqual(strtree.ToList(), new List<string> {"a", "b", "c", "d", "f", "s", "t", "x", "z"});
        }

        [TestMethod]
        public void TestIndexes()
        {
            var tree = new Tree<int> { 5, 6, 7, 3, 1, -1, 0, -5, -3, -4, -2 };
            var list = new List<int>();
            for (var i = 0; i < 11; i++)
                list.Add(tree[i]);
            CollectionAssert.AreEqual(tree.ToList(), list);
            var strtree = new Tree<string> { "s", "c", "x", "a", "f", "t", "z", "b", "d" };
            var strlist = new List<string>();
            for (var i = 0; i < 9; i++)
                strlist.Add(strtree[i]);
            CollectionAssert.AreEqual(strtree.ToList(), strlist);
        }
    }
}
