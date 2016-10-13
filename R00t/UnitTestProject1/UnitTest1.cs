using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var x = R00t.Program.FindRoot(new string[] { "-1", "1", "2", "6", "4"});
            Assert.AreEqual(0, 2+6*x+4*x*x);
            x = R00t.Program.FindRoot(new string[] { "-2", "-1", "2", "6", "4" });
            Assert.AreEqual(0, 2 + 6 * x + 4 * x * x);
            x = R00t.Program.FindRoot(new string[] { "0", "3", "1", "-1", "-5", "0", "1" });
            Assert.AreEqual(0, Math.Round(1-x-5*x*x+x*x*x*x, 10));
            x = R00t.Program.FindRoot(new string[] { "-3", "3", "12", "0,7", "0,3", "0,9", "0,3", "-3", "1"});
            Assert.AreEqual(0, Math.Round(12 + 0.7 * x + 0.3 * x * x + 0.9 * x * x * x + 0.3 * x * x * x * x - 3 * x * x * x * x * x + x * x * x * x * x * x));
            x = R00t.Program.FindRoot(new string[] { "1", "2", "12", "0,7", "0,3", "0,9", "0,3", "-3", "1" });
            Assert.AreEqual(0, Math.Round(12 + 0.7 * x + 0.3 * x * x + 0.9 * x * x * x + 0.3 * x * x * x * x - 3 * x * x * x * x * x + x * x * x * x * x * x));
        }
    }
}
