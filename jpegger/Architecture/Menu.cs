using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
    public class JpeggerMenu : Form
    {
        public int Type { get; private set; }

        public JpeggerMenu()
        {
            this.MouseClick += MouseC;
            this.Paint += Paint5;
            ClientSize = new Size(800, 600);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Text = ".jpegger menu";
            DoubleBuffered = true;
            var imagesDirectory = new DirectoryInfo("Images");
        }

        private void MouseC(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString() == "Left" && e.X < 500 && e.X > 300)
            {
                if (e.Y < 320 && e.Y > 220)
                {
                    Type = 1;
                    this.Close();
                }
                else if (e.Y < 470 && e.Y > 370)
                {
                    Type = 2;
                    this.Close();
                }
            }
        }

        static void Paint5(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.Clear(Color.Black);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            var pen = new Pen(Color.White, 4);
            graphics.DrawRectangle(pen, 300, 220, 200, 100);
            graphics.DrawRectangle(pen, 300, 370, 200, 100);
            graphics.DrawString("Начать игру", new Font("Arial", 20), Brushes.White, 320, 253);
            graphics.DrawString("Создать", new Font("Arial", 20), Brushes.White, 340, 384);
            graphics.DrawString("уровень", new Font("Arial", 20), Brushes.White, 342, 420);
            graphics.DrawString(".jpegger", new Font("Comic Sans MS", 80), Brushes.Yellow, 160, 10);
        }
    }
}
