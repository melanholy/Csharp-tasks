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
            var testArray = new string[] { "gather", "hero", "ust-kuzminsk" };
            Assert.AreEqual("gather", autocomplete.ClassForTests.FindByPrefix("ga", testArray));
            Assert.AreEqual(null, autocomplete.ClassForTests.FindByPrefix("ta", testArray));
            Assert.AreEqual("ust-kuzminsk", autocomplete.ClassForTests.FindByPrefix("us", testArray));
            Assert.AreEqual("the", autocomplete.ClassForTests.FindByPrefix("the", new string[] { "the" }));

            testArray = new string[] { "army", "elephant", "elevator", "electricity", "elective", "emotion", "ust-kuzminsk" };
            Assert.AreEqual(4, autocomplete.ClassForTests.FindCount("el", testArray));
            Assert.AreEqual(5, autocomplete.ClassForTests.FindCount("e", testArray));
            Assert.AreEqual(1, autocomplete.ClassForTests.FindCount("arm", testArray));
            Assert.AreEqual(0, autocomplete.ClassForTests.FindCount("matcore", testArray));

            Assert.AreEqual(null, autocomplete.ClassForTests.FindByPrefix("matcore", 3, testArray));
            Assert.AreEqual("army ", autocomplete.ClassForTests.FindByPrefix("a", 3, testArray));
            Assert.AreEqual("elephant elevator electricity ", autocomplete.ClassForTests.FindByPrefix("e", 3, testArray));
        }
    }
}
