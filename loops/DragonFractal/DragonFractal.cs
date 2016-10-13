using System;
using System.Drawing; 
using System.Windows.Forms;

namespace Fractals
{
	class DragonFractal
	{
		const int Size = 800;

		static void Main()
		{
			var image = CreateDragonImage(1000000);
			ShowImageInWindow(image);
		}

		private static void ShowImageInWindow(Bitmap image)
		{
			var form = new Form
			{
				Text = "НЕВЕРОЯТНО ЭПИЧНОЕ НАЗВАНИЕ НЕ ОЧЕНЬ-ТО ЭПИЧНОГО ОКНА",
				ClientSize = new Size(Size, Size),
			};
			form.Controls.Add(new PictureBox {Image = image, Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.CenterImage});
			form.ShowDialog();
		}


        static Bitmap CreateDragonImage(int iterationsCount)
        {
            var image = new Bitmap(Size, Size);
            var g = Graphics.FromImage(image);
            g.FillRectangle(Brushes.DarkSlateBlue, 0, 0, image.Width, image.Height);
            Random rand = new Random();
            double x = 0, y = 0;

            int a = rand.Next(255), b = rand.Next(255), c = rand.Next(255);
            for (int i = 0; i < iterationsCount; i++)
            {
                double x1 = x;
                if (rand.Next(2) == 1)
                {
                    x = (x * Math.Cos(Math.PI / 4) - y * Math.Sin(Math.PI / 4)) / Math.Sqrt(2);
                    y = (x1 * Math.Sin(Math.PI / 4) + y * Math.Cos(Math.PI / 4)) / Math.Sqrt(2);
                    //x = (x * Math.Cos(-Math.PI / 4) - y * Math.Sin(-Math.PI / 4)) / Math.Sqrt(2);
                    //y = (x1 * Math.Sin(-Math.PI / 4) + y * Math.Cos(-Math.PI / 4)) / Math.Sqrt(2);
                }
                else
                {
                    x = (x * Math.Cos(0.75 * Math.PI) - y * Math.Sin(0.75 * Math.PI)) / Math.Sqrt(2) + 1;
                    y = (x1 * Math.Sin(0.75 * Math.PI) + y * Math.Cos(0.75 * Math.PI)) / Math.Sqrt(2);
                    //x = (x * Math.Cos(-Math.PI / 4) - y * Math.Sin(-Math.PI / 4)) / Math.Sqrt(2)+1;
                    //y = (x1 * Math.Sin(-Math.PI / 4) + y * Math.Cos(-Math.PI / 4)) / Math.Sqrt(2);
                }
                SetPixel(image, x-0.79, y-0.5, (int)-Math.Abs((x*y)*143000), 100, 1.4);
                //SetPixel(image, x - 0.75, y + 0.25, (int)-Math.Abs((x * y) * 24000), 100, 2.0);
                /*int o = rand.Next(100);
                if (o == 0)
                {
                    x = 0;
                    y *= 0.16;
                }
                if (o>0 && o <= 85)
                {
                    x1 = x;
                    x = 0.85 * x + 0.04 * y;
                    y = -0.04 * x1 + 0.85 * y + 1.6;
                }
                if (o>85 && o <= 92)
                {
                    x1 = x;
                    x = 0.2 * x - 0.26 * y;
                    y = 0.23 * x1 + 0.22 * y + 1.6;
                }
                if (o>92 && o <= 99)
                {
                    x1 = x;
                    x = -0.15 * x + 0.28 * y;
                    y = 0.26 * x1 + 0.24 * y + 0.44;
                }
                SetPixel(image, x-4.3, y - 9, (int)-Math.Abs((x * y) * 2400), 170, 9.0);*/
            }
            return image;
        }

        static void SetPixel(Bitmap image, double x, double y, int a, int MarginSize, double scal)
        {
            var xx = Scale(x, image.Width, MarginSize, scal);
            var yy = Scale(y, image.Height, MarginSize, scal);
            if (xx >= 0 && xx < image.Width && yy >= 0 && yy < image.Height)
                image.SetPixel(xx, yy, Color.FromArgb(a));
        }

		static int Scale(double x, double maxX, int MarginSize, double scal)
		{
			return (int)Math.Round(maxX / scal + (maxX / scal - MarginSize) * x);
		}
	}
}
