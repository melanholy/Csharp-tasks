using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Recognizer
{
    static class Program
    {
        const int ResizeRate = 2;

        static Bitmap Convert(int W, int H, Func<int, int, Color> func)
        {
            var bmp = new Bitmap(ResizeRate * W, ResizeRate * H);
            using (var g = Graphics.FromImage(bmp))
            {
                for (int x = 0; x < W; x++)
                    for (int y = 0; y < H; y++)
                        g.FillRectangle(new SolidBrush(func(x, y)),
                            ResizeRate * x,
                            ResizeRate * y,
                            ResizeRate,
                            ResizeRate
                            );
            }
            return bmp;
        }

        static Bitmap Convert(byte[, ,] array)
        {
            return Convert(array.GetLength(0), array.GetLength(1), (x, y) => Color.FromArgb(array[x, y, 0], array[x, y, 1], array[x, y, 2]));
        }

        static Bitmap Convert(double[,] array)
        {
            return Convert(array.GetLength(0), array.GetLength(1), (x, y) =>
            {
                var gray = (int)(255 * array[x, y]);
                gray = Math.Min(gray, 255);
                gray = Math.Max(gray, 0);
                return Color.FromArgb(gray, gray, gray);
            });

        }



        static PictureBox CreateBox(Bitmap bmp)
        {
            var box = new PictureBox();
            box.Size = bmp.Size;
            box.Dock = DockStyle.Fill;
            box.Image = bmp;
            return box;
        }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            var bmp = (Bitmap)Bitmap.FromFile("eurobot.bmp");
            var array = new byte[bmp.Width, bmp.Height, 3];
            var rnd = new Random();
            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    array[x, y, 0] = pixel.R;
                    array[x, y, 1] = pixel.G;
                    array[x, y, 2] = pixel.B;
                }

            var form = new Form();
            form.ClientSize = new Size(3 * ResizeRate * bmp.Width, 2 * ResizeRate * bmp.Height);


            var panel = new TableLayoutPanel();
            panel.RowCount = 2;
            panel.ColumnCount = 3;
            panel.Dock = DockStyle.Fill;
            form.Controls.Add(panel);

            panel.Controls.Add(CreateBox(Convert(array)), 0, 0);
            Tasks.ClearNoise(array);
            panel.Controls.Add(CreateBox(Convert(array)), 1, 0);
            var grayscale = Tasks.Grayscale(array);
            panel.Controls.Add(CreateBox(Convert(grayscale)), 2, 0);
            var sobell = Tasks.SobelFiltering(grayscale);
            panel.Controls.Add(CreateBox(Convert(sobell)), 0, 1);
            Tasks.ThresholdFiltering(sobell);
            panel.Controls.Add(CreateBox(Convert(sobell)), 1, 1);
            var bitmap = Convert(sobell);
            using (var g = Graphics.FromImage(bitmap))
            {
                var lines = Tasks.HoughAlgorithm(sobell);
                var pen = new Pen(Color.Red, 2);
                foreach (var e in lines)
                    g.DrawLine(pen, e.X0 * ResizeRate, e.Y0 * ResizeRate, e.X1 * ResizeRate, e.Y1 * ResizeRate);
            }
            panel.Controls.Add(CreateBox(bitmap), 2, 1);
            Application.Run(form);


        }
    }
}
