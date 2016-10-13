using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeometryLibrary;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var seg = Geometry.CreateSegment(new MyPoint { X = 0, Y = 50 }, new MyPoint { X = 50, Y = 50 });
            var answer = Geometry.IsPointInSegment(seg, new MyPoint { X = 25, Y = 50 });
            Assert.AreEqual(true, answer);
            answer = Geometry.IsPointInSegment(seg, new MyPoint { X = 70, Y = 50 });
            Assert.AreEqual(false, answer);
            answer = Geometry.IsPointInSegment(seg, new MyPoint { X = 70, Y = 60 });
            Assert.AreEqual(false, answer);
            answer = Geometry.IsPointInSegment(seg, new MyPoint { X = 25, Y = 60 });
            Assert.AreEqual(false, answer);
            answer = Geometry.IsPointInSegment(seg, new MyPoint { X = 50, Y = 50 });
            Assert.AreEqual(true, answer);
            answer = Geometry.IsPointInSegment(seg, new MyPoint { X = -1, Y = 50 });
            Assert.AreEqual(false, answer);
            answer = Geometry.IsPointInSegment(seg, new MyPoint { X = 0, Y = 50 });
            Assert.AreEqual(true, answer);

            seg = Geometry.CreateSegment(new MyPoint { X = 0, Y = 0 }, new MyPoint { X = 5, Y = 5 });
            answer = Geometry.IsPointInSegment(seg, new MyPoint { X = 3, Y = 3 });
            Assert.AreEqual(true, answer);
            answer = Geometry.IsPointInSegment(seg, new MyPoint { X = 4, Y = 3 });
            Assert.AreEqual(false, answer);
            answer = Geometry.IsPointInSegment(seg, new MyPoint { X = 3, Y = 5 });
            Assert.AreEqual(false, answer);
            answer = Geometry.IsPointInSegment(seg, new MyPoint { X = -1, Y = 3 });
            Assert.AreEqual(false, answer);
            answer = Geometry.IsPointInSegment(seg, new MyPoint { X = -1, Y = -1 });
            Assert.AreEqual(false, answer);
            answer = Geometry.IsPointInSegment(seg, new MyPoint { X = 6, Y = 6 });
            Assert.AreEqual(false, answer);
            answer = Geometry.IsPointInSegment(seg, new MyPoint { X = 6, Y = -1 });
            Assert.AreEqual(false, answer);
            answer = Geometry.IsPointInSegment(seg, new MyPoint { X = -1, Y = 6 });
            Assert.AreEqual(false, answer);
            answer = Geometry.IsPointInSegment(seg, new MyPoint { X = 0, Y = 6 });
            Assert.AreEqual(false, answer);

            answer = Geometry.IsPointInSegment(new Segment { StartX = 0, StartY = 0, EndX = 0, EndY = 5 }, new MyPoint { X = 0, Y = -1 });
            Assert.AreEqual(false, answer);

            var rect = Geometry.CreateRectangle(new MyPoint { X = 0, Y = 0 }, new MyPoint { X = 0, Y = 30 }, new MyPoint { X = 50, Y = 30 });
            Assert.AreEqual(50, rect.Width);
            Assert.AreEqual(30, rect.Height);
            Assert.AreEqual(0, rect.Anchor.X);
            Assert.AreEqual(0, rect.Anchor.Y);
            rect = Geometry.CreateRectangle(new MyPoint { X = 0, Y = 0 }, new MyPoint { X = 30, Y = 0 }, new MyPoint { X = 30, Y = -60 });
            Assert.AreEqual(30, rect.Width);
            Assert.AreEqual(60, rect.Height);
            Assert.AreEqual(0, rect.Anchor.X);
            Assert.AreEqual(-60, rect.Anchor.Y);
            rect = Geometry.CreateRectangle(new MyPoint { X = 50, Y = 0 }, new MyPoint { X = 50, Y = 20 }, new MyPoint { X = 0, Y = 0 });
            Assert.AreEqual(50, rect.Width);
            Assert.AreEqual(20, rect.Height);
            Assert.AreEqual(0, rect.Anchor.X);
            Assert.AreEqual(0, rect.Anchor.Y);
            rect = Geometry.CreateRectangle(new MyPoint { X = 50, Y = 0 }, new MyPoint { X = 50, Y = 70 }, new MyPoint { X = 0, Y = 0 });
            Assert.AreEqual(50, rect.Width);
            Assert.AreEqual(70, rect.Height);
            Assert.AreEqual(0, rect.Anchor.X);
            Assert.AreEqual(0, rect.Anchor.Y);
            rect = Geometry.CreateRectangle(new MyPoint { X = 48, Y = 5 }, new MyPoint { X = 0, Y = 0 }, new MyPoint { X = 48, Y = 0 });
            Assert.AreEqual(48, rect.Width);
            Assert.AreEqual(5, rect.Height);
            Assert.AreEqual(0, rect.Anchor.X);
            Assert.AreEqual(0, rect.Anchor.Y);
            rect = Geometry.CreateRectangle(new MyPoint { X = -20, Y = 84 }, new MyPoint { X = 40, Y = 84 }, new MyPoint { X = 40, Y = 32 });
            Assert.AreEqual(60, rect.Width);
            Assert.AreEqual(52, rect.Height);
            Assert.AreEqual(-20, rect.Anchor.X);
            Assert.AreEqual(32, rect.Anchor.Y);

            rect = rect.Rotate((float)Math.PI / 2, new MyPoint { X = -20, Y = 32 });
            Assert.AreEqual(-20, rect.Anchor.X);
            Assert.AreEqual(32, rect.Anchor.Y);
            rect = new MyRectangle { Anchor = new MyPoint { X = -1, Y = -2 }, Height = 1, Width = 1, Slope = 0 };
            rect = rect.Rotate((float)Math.PI / 2, new MyPoint { X = 0, Y = 0 });
            Assert.AreEqual(2, rect.Anchor.X);
            Assert.AreEqual(-1, rect.Anchor.Y, 1e-6);
            rect = rect.Rotate((float)Math.PI / 2, new MyPoint { X = 0, Y = 0 });
            Assert.AreEqual(1, rect.Anchor.X, 1e-6);
            Assert.AreEqual(2, rect.Anchor.Y, 1e-6);
            rect = new MyRectangle { Anchor = new MyPoint { X = -1, Y = -1 }, Height = (float)Math.Sqrt(2), Width = (float)Math.Sqrt(2), Slope = (float)-Math.PI / 4 };
            rect = rect.Rotate((float)Math.PI / 4, new MyPoint { X = 0, Y = 0 });
            Assert.AreEqual(0, rect.Anchor.X, 1e-6);
            Assert.AreEqual((float)-Math.Sqrt(2), rect.Anchor.Y, 1e-6);
            rect = rect.Rotate((float)(2 * Math.PI - Math.PI / 4), new MyPoint { X = 0, Y = 0 });
            Assert.AreEqual(-1, rect.Anchor.X, 1e-6);
            Assert.AreEqual(-1, rect.Anchor.Y, 1e-6);
            rect = new MyRectangle { Anchor = new MyPoint { X = 0, Y = 0 }, Height = 2, Width = 2, Slope = 0 };
            rect = rect.Rotate((float)Math.PI / 2, new MyPoint { X = 1, Y = 1 });
            Assert.AreEqual(2, rect.Anchor.X, 1e-6);
            Assert.AreEqual(0, rect.Anchor.Y, 1e-6);
        }
    }
}
