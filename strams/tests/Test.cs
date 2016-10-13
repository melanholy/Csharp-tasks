using System.Drawing;
using System.Drawing.Imaging;
using NUnit.Framework;
using Rhino.Mocks;
using streams;

namespace tests
{
    [TestFixture]
    public class FuncsTests
    {
        [Test]
        public void TestTimer()
        {
            var timer = new MyTimer();

            using (timer.Start())
                Assert.True(timer.IsRunning);

            Assert.False(timer.IsRunning);

            using (timer.Continue())
                Assert.True(timer.IsRunning);

            Assert.False(timer.IsRunning);
        }

        [Test]
        public void TestEditor()
        {
            var bmp = new Bitmap(8, 6, PixelFormat.Format24bppRgb);

            bmp.SetPixel(4, 4, Color.FromArgb(240, 248, 255));

            using (var be = new BitmapEditor(bmp))
            {
                be.SetPixel(5, 5, 4, 5, 2);
                Assert.AreEqual(be.GetPixel(4, 4), Color.FromArgb(240, 248, 255));
                Assert.AreEqual(be.GetPixel(5, 5), Color.FromArgb(4, 5, 2));
            }

            Assert.AreEqual(bmp.GetPixel(4, 4), Color.FromArgb(240, 248, 255));
            Assert.AreEqual(bmp.GetPixel(5, 5), Color.FromArgb(4, 5, 2));
        }
    }
}