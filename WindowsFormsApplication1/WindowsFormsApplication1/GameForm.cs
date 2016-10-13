using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public sealed partial class GameForm : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps;
        private Game game;
        private readonly Bitmap floor;
        private readonly PrivateFontCollection font;
        private bool paused;
        private readonly Button cont;
        private readonly Button exit;
        private bool stopped;
        private bool completed;
        private readonly Stopwatch sw = new Stopwatch();

        public GameForm(Game game)
        {
            completed = false;
            paused = false;
            stopped = false;
            bitmaps = new Dictionary<string, Bitmap>();
            font = new PrivateFontCollection();
            font.AddFontFile("Shoguns Clan.ttf");
            font.AddFontFile("WIDEAWAKE.TTF");
            this.game = game;
            KeyDown += KeyD;
            KeyUp += KeyU;
            ClientSize = new Size(800, 600);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Text = @"Inside Job";
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            MouseMove += OnMouseMove;
            DoubleBuffered = true;
            MouseClick += OnMouseClick;
            var imagesDirectory = new DirectoryInfo("Images");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
            {
                var img = Image.FromFile(e.FullName);
                bitmaps[e.Name] = new Bitmap(img, img.Width / 2, img.Height / 2);
            }
            floor = new Bitmap(game.Graph.GetLength(0) * game.ObjectSize, game.Graph.GetLength(1) * game.ObjectSize);
            var graphics = Graphics.FromImage(floor);
            foreach (var obj in game.FloorSprites)
                graphics.DrawImage(bitmaps[obj.Image], obj.Position);
            var timer = new Timer {Interval = 1000/75};
            timer.Tick += TimerTick;
            timer.Start();
            game.Cursor.CursorMove += SetCursorPos;
            Cursor.Hide();
            sw.Start();
            cont = new Button
            {
                Location = new Point(ClientSize.Width/2 - 125, ClientSize.Height/2 - 75),
                Size = new Size(250, 50),
                Font = new Font(font.Families[0], 28),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.DarkRed,
                Visible = false,
                BackColor = Color.GreenYellow
            };
            cont.Click += Pause;
            exit = new Button
            {
                Location = new Point(ClientSize.Width / 2 - 125, ClientSize.Height/2 - 15),
                Size = new Size(250, 50),
                Font = new Font(font.Families[0], 32),
                Text = @"Exit",
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.DarkRed,
                Visible = false,
                BackColor = Color.Orange
            };
            exit.Click += (x, y) => Close();
            Controls.Add(cont);
            Controls.Add(exit);
        }

        private void ChangeLevel(List<string> map, List<List<string>> levels)
        {
            game = new Game(map, levels);
            paused = false;
            stopped = false;
            completed = false;
            cont.Hide();
            exit.Hide();
            Cursor.Hide();
            Focus();
            cont.Click -= Restart;
            cont.Click -= NextLevel;
            cont.Click += Pause;
        }

        private void Restart(object sender, EventArgs e)
        {
            ChangeLevel(game.Map, game.Levels);
        }

        private void Pause()
        {
            paused = !paused;
            if (game.IsLevelComplete)
            {
                completed = true;
                cont.Click -= Pause;
                cont.Click -= Restart;
                cont.Click += NextLevel;
            }
            else if (game.IsOver)
            {
                stopped = true;
                cont.Click -= Pause;
                cont.Click -= NextLevel;
                cont.Click += Restart;
            }
            if (paused)
            {
                cont.Show();
                exit.Show();
                Cursor.Show();
            }
            else
            {
                cont.Hide();
                exit.Hide();
                Cursor.Hide();
                Focus();
            }
        }

        private void NextLevel(object sender, EventArgs e)
        {
            ChangeLevel(game.Levels[0], game.Levels.GetRange(1, game.Levels.Count - 1));
        }

        private void Pause(object sender, EventArgs e)
        {
            Pause();
        }

        private void SetCursorPos(int dx, int dy)
        {
            Cursor.Position = new Point(Cursor.Position.X+dx, Cursor.Position.Y+dy);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (paused) return;
            if (e.Button == MouseButtons.Right) game.Hero.PickUp();
            else if (e.Button == MouseButtons.Left) game.Hero.Shoot();
        }

        private int de;
        private int du;

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Cursor.Clip = Bounds;
            if (paused) return;
            game.Cursor.Position = new Point(e.X + du, e.Y + de);
            if (Cursor.Position.Y >= Bottom - 20)
            {
                Cursor.Position = new Point(Cursor.Position.X, Top + 60);
                de += 560;
                sw.Restart();
            }
            else if (Cursor.Position.Y <= Top + 40)
            {
                Cursor.Position = new Point(Cursor.Position.X, Bottom - 40);
                de += -560;
                sw.Restart();
            }
            if (Cursor.Position.X >= Right - 20)
            {
                Cursor.Position = new Point(Left + 40, Cursor.Position.Y);
                du += 760;
                sw.Restart();
            }
            else if (Cursor.Position.X <= Left + 20)
            {
                Cursor.Position = new Point(Right - 40, Cursor.Position.Y);
                du += -760;
                sw.Restart();
            }
        }

        private void KeyD(object sender, KeyEventArgs key)
        {
            const int heroSpeed = 5;
            if (key.KeyCode.ToString() == "Escape") Pause();
            if (paused) return;
            if (key.KeyCode.ToString() == "S") game.DeltaY = heroSpeed;
            else if (key.KeyCode.ToString() == "W") game.DeltaY = -heroSpeed;
            else if (key.KeyCode.ToString() == "A") game.DeltaX = -heroSpeed;
            else if (key.KeyCode.ToString() == "D") game.DeltaX = heroSpeed;
        }

        private void KeyU(object sender, KeyEventArgs key)
        {
            if (key.KeyCode.ToString() == "S") game.DeltaY = 0;
            if (key.KeyCode.ToString() == "W") game.DeltaY = 0;
            if (key.KeyCode.ToString() == "A") game.DeltaX = 0;
            if (key.KeyCode.ToString() == "D") game.DeltaX = 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform((400 - game.Hero.Position.X), (300 - game.Hero.Position.Y));
            e.Graphics.DrawImage(floor, new Point(0, 0));
            foreach (var obj in game.BloodStains)
                e.Graphics.DrawImage(bitmaps[obj.Image], obj.Position);
            foreach (var obj in game.InteractiveObjects)
                e.Graphics.DrawImage(bitmaps[obj.Image], obj.Position);
            game.MovableObjects.Sort((x, y) => y.DrawingPriority.CompareTo(x.DrawingPriority));
            e.Graphics.TranslateTransform(-(400 - game.Hero.Position.X), -(300 - game.Hero.Position.Y));
            foreach (var obj in game.MovableObjects)
            {
                var a = obj.Position.X + 15;
                var b = obj.Position.Y + 9;
                e.Graphics.TranslateTransform(a, b);
                e.Graphics.TranslateTransform((400 - game.Hero.Position.X), (300 - game.Hero.Position.Y));
                e.Graphics.RotateTransform((float) obj.Angle*180/(float) Math.PI + 90);
                e.Graphics.TranslateTransform(-a, -b);
                e.Graphics.DrawImage(bitmaps[obj.Image], obj.Position);
                e.Graphics.ResetTransform();
            }
            e.Graphics.TranslateTransform((400 - game.Hero.Position.X), (300 - game.Hero.Position.Y));
            foreach (var obj in game.Walls)
                e.Graphics.DrawImage(bitmaps[obj.Image], obj.Position);
            e.Graphics.TranslateTransform(-(400 - game.Hero.Position.X), -(300 - game.Hero.Position.Y));
            if (game.Hero.Weapon == "Gun")
            {
                e.Graphics.FillRectangle(Brushes.Black, 0, ClientSize.Height - 70, 200, 50);
                e.Graphics.DrawString(string.Format("{0}/4 shots", game.Hero.Shots), new Font(font.Families[1], 30), Brushes.BlueViolet, 10, ClientSize.Height - 78);
            }
            if (!paused) return;
            e.Graphics.DrawImage(bitmaps["Menu.png"], new Point(ClientSize.Width/2 - 150, ClientSize.Height/2 - 175));
            cont.Text = completed ? "Next level" : stopped ? "Try agai n" : "Continue";
            e.Graphics.DrawString(completed ? "You won" : stopped ? "You lost" : "Paused", new Font(font.Families[1], 45), Brushes.BlueViolet,
                completed ? new Point(ClientSize.Width/2 - 110, ClientSize.Height/2 - 170) : 
                    !stopped ? new Point(ClientSize.Width/2 - 90, ClientSize.Height/2 - 170) :
                        new Point(ClientSize.Width / 2 - 110, ClientSize.Height / 2 - 170));
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if ((game.IsOver || game.IsLevelComplete) && !paused) Pause();
            if (stopped || !paused) game.Update();
            Invalidate();
        }
    }
}