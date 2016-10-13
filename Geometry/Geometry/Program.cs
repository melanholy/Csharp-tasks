using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeometryLibrary;

namespace Geometry
{
    static class Program
    {
        static MyRectangle original;
        static MyRectangle processed;
        static MyPoint rotationCenter;
        static float angle;

        static void Paint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.Clear(System.Drawing.Color.Beige);
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Red);
            pen.Width = 2;
            pen.DashPattern = new float[] { 4, 3, 4, 3 };
            var coords = original.GetCoords();
            graphics.DrawLine(pen, original.Anchor.X, original.Anchor.Y, coords[0], coords[1]);
            graphics.DrawLine(pen, original.Anchor.X, original.Anchor.Y, coords[2], coords[3]);
            graphics.DrawLine(pen, coords[0], coords[1], coords[4], coords[5]);
            graphics.DrawLine(pen, coords[2], coords[3], coords[4], coords[5]);
            pen = new System.Drawing.Pen(System.Drawing.Color.Black, 2);
            graphics.DrawPie(pen, 350, 400, 4, 4, 0, 360);
            pen = new System.Drawing.Pen(System.Drawing.Color.Green, 2);
            coords = processed.GetCoords();
            graphics.DrawLine(pen, processed.Anchor.X, processed.Anchor.Y, coords[0], coords[1]);
            graphics.DrawLine(pen, processed.Anchor.X, processed.Anchor.Y, coords[2], coords[3]);
            graphics.DrawLine(pen, coords[0], coords[1], coords[4], coords[5]);
            graphics.DrawLine(pen, coords[2], coords[3], coords[4], coords[5]);
        }

        [STAThread]
        static void Main()
        {
            var form = new Form();
            original = new MyRectangle { Anchor = new MyPoint { X=50, Y=35 } , Height = 200, Width = 300, Slope = (float)Math.PI / 18 * 0};
            angle = (float)(Math.PI / 2);
            rotationCenter = new MyPoint { X = 350, Y = 400 };
            processed = original.Rotate(angle, rotationCenter);
            form.Paint += Paint;
            form.ClientSize = new System.Drawing.Size(1024, 768);
            Application.Run(form);
        }
    }
}
