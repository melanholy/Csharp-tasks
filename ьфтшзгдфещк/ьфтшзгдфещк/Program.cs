using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Manipulator
{
    public static class Program
    {
        static Form form;
        static double Wrist = Math.PI / 2;
        static double Elbow=Math.PI / 2;
        static double Shoulder= Math.PI / 2;
        static float x=50;
        static float y=400;
        static double angle = -Math.PI / 2;
        static float delta = 0.3f;
        static float delta1 = 0.5f;
        static double[] angles = new double[3];
        static string button = "";
        static string button1 = "";
        static string button2 = "";
        static Stopwatch a = new Stopwatch();
        static Stopwatch b = new Stopwatch();
        static float height;
        static float width;
        static float x3;
        static float y3;
        static float speed = 1;

        static void Paint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.Clear(Color.Beige);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            var pen = new Pen(Color.Black, 2);
            x = 50;
            y = 400;
            graphics.DrawLine(pen, 0, 400, form.ClientSize.Width, 400);
            var x1 = (float)(x + 150 * Math.Cos(Shoulder));
            var y1 = (float)(y - 150 * Math.Sin(Shoulder));
            graphics.DrawLine(pen, 50, 400, x1, y1);
            x = x1;
            y = y1;
            x1 = (float)(x + 120 * Math.Sin(3 * Math.PI / 2 - Shoulder - Elbow));
            y1 = (float)(y - 120 * Math.Cos(3 * Math.PI / 2 - Shoulder - Elbow));
            graphics.DrawLine(pen, x, y, x1, y1);
            x = x1;
            y = y1;
            x1 = (float)(x + 100 * Math.Cos(2 * Math.PI - Wrist - Shoulder - Elbow));
            y1 = (float)(y + 100 * Math.Sin(2 * Math.PI - Wrist - Shoulder - Elbow));
            graphics.DrawLine(pen, x, y, x1, y1);
            x = x1;
            y = y1;
            graphics.DrawString("Нажмите Е, чтобы вернуть робота в стартовое положение", new Font("Arial", 14), Brushes.Blue, 0, 0);
        }

        static void KeyDown(object sender, KeyEventArgs key)
        {
            button = key.KeyCode.ToString();
            if (key.KeyCode.ToString() == "ShiftKey") speed += 0.3f;
            if (key.KeyCode.ToString() == "ControlKey") speed -= 0.3f;
        }

        static void KeyUp(object sender1, KeyEventArgs key1)
        {
            button1 = button;
            button = "";
        }

        static void form_MouseClick(object sender, MouseEventArgs e)
        {
            x3 = e.X;
            y3 = e.Y;
            height = (e.Y - y) / 30.0f;
            width = (e.X - x) / 30.0f;
            button2 = "mouse";
        }

        public static void RefreshForm(object source, ElapsedEventArgs e)
        {
            form.Invalidate();
        }

        static void form_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                a.Start();
                button = "R";
            }
            else
            {
                b.Start();
                button = "F";
            }
        }

        private static void MouseMove(object sender, ElapsedEventArgs e)
        {
            if (button2 == "mouse" && Math.Abs(x - x3) > Math.Abs(width) && Math.Abs(y - y3) > Math.Abs(height))
            {
                x += width*delta1;
                y += height*delta1;
                angles = RobotMathematics.MoveTo(x, y, angle);
                if (Double.IsNaN(angles[0])) button2 = "";
                Recount();

                if (delta1 < 1) delta1 += 0.02f;
            }
            else if (button2 == "mouse") button2 = "";
        }

        public static void StopMoving(object source, ElapsedEventArgs e)
        {
            if (button1 != "")
                if (delta > 0.2f && y+delta<400)
                {
                    if (delta > 1.05) delta -= delta / 4;
                    if (button1 == "R") angle += delta/100;
                    if (button1 == "F") angle -= delta/100;
                    if (button1 == "A") x -= delta;
                    if (button1 == "D") x += delta;
                    if (button1 == "S") y += delta;
                    if (button1 == "W") y -= delta;
                    angles = RobotMathematics.MoveTo(x, y, angle);
                    if (button1 == "E")
                    {
                        angle = -Math.PI / 2;
                        angles = RobotMathematics.MoveTo(170, 350, angle);
                    }
                    Recount();
                    if (button1 != "R" && button1 != "F") delta -= 0.05f;
                    else delta -= 0.1f;
                }
                else
                {
                    delta = 0.3f;
                    button1 = "";
                }
        }

        public static void StartMoving(object source, ElapsedEventArgs e)
        {
            if (button != "" && !(y + delta > 400 && button == "S"))
            {
                button1 = "";
                if (button == "R") angle += delta / 100;
                if (button == "F") angle -= delta / 100;
                if (button == "A") x -= delta;
                if (button == "D") x += delta;
                if (button == "S") y += delta;
                if (button == "W") y -= delta;
                angles = RobotMathematics.MoveTo(x, y, angle);
                if (button == "E")
                {
                    angle = -Math.PI / 2;
                    angles = RobotMathematics.MoveTo(170, 350, angle);
                }
                if (delta<speed) delta += 0.05f;
                Recount();
                if (a.IsRunning && a.ElapsedMilliseconds > 200)
                {
                    button = "";
                    button1 = "R";
                    a.Reset();
                }
                if (b.IsRunning && b.ElapsedMilliseconds > 200)
                {
                    button = "";
                    button1 = "F";
                    b.Reset();
                }
            }
        }

        static void Recount()
        {
            if (angles[0] != 0 && angles[1] != 0 && angles[2] != 0 && !Double.IsNaN(angles[0]) && angles[2]<Math.PI-Math.PI/360)
            {
                Shoulder = angles[0];
                Elbow = angles[1];
                Wrist = angles[2];
            }
        }

        [STAThread]
        static void Main()
        {
            System.Timers.Timer myTimer = new System.Timers.Timer();
            System.Timers.Timer myTimer1 = new System.Timers.Timer();
            myTimer1.Elapsed += new ElapsedEventHandler(RefreshForm);
            myTimer1.Interval = 10;
            myTimer1.Start();
            myTimer.Interval = 20;
            myTimer.Start();
            myTimer.Elapsed += new ElapsedEventHandler(StartMoving);
            myTimer.Elapsed += new ElapsedEventHandler(StopMoving);
            myTimer.Elapsed += new ElapsedEventHandler(MouseMove);
            form = new AntiFlickerForm();
            form.Paint += Paint;
            form.KeyDown += KeyDown;
            form.KeyUp += KeyUp;
            form.MouseClick += form_MouseClick;
            form.MouseWheel += form_MouseWheel;
            form.ClientSize = new Size(1024, 768);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.Text = "Manipulator";
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            Application.Run(form);
        }
    }
}
