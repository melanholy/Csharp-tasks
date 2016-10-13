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
            var angle = Manipulator.RobotMathematics.GetAngle(5, 3, 4);
            Assert.AreEqual(Math.PI / 2, angle);
            angle = Manipulator.RobotMathematics.GetAngle(3, 5, 4);
            Assert.AreEqual(Math.Acos(0.8), angle);
            angle = Manipulator.RobotMathematics.GetAngle(4, 3, 5);
            Assert.AreEqual(Math.Acos(0.6), angle);
            angle = Manipulator.RobotMathematics.GetAngle(4, 4, 4);
            Assert.AreEqual(Math.PI / 3, angle);
            angle = Manipulator.RobotMathematics.GetAngle(Math.Sin(Math.PI / 7) * 6, Math.Cos(Math.PI / 7) * 6, 6);
            Assert.AreEqual(Math.Round(Math.PI / 7, 10), Math.Round(angle, 10));
            angle = Manipulator.RobotMathematics.GetAngle(12, 6 ,12);
            Assert.AreEqual(Math.Round(Math.PI / 3, 10), Math.Round(angle, 10));
            var angles = Manipulator.RobotMathematics.MoveTo(170, 350, - Math.PI / 2);
            Assert.AreEqual(Math.Round(Math.PI / 2, 10), Math.Round(angles[0], 10));
            Assert.AreEqual(Math.Round(Math.PI / 2, 10), Math.Round(angles[1], 10));
            Assert.AreEqual(Math.Round(Math.PI / 2, 10), Math.Round(angles[2], 10));
        }
    }
}
