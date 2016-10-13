using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
    public class JpeggerConstructor : Form
    {
        const int ElementSize = 32;
        Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        static List<CreatureAnimation> animations = new List<CreatureAnimation>();
        static Game game;

        public JpeggerConstructor(int height, int width)
        {
            game = new Game(height, width);
            this.KeyDown += KeyD;
            this.KeyUp += KeyU;
            ClientSize = new Size(ElementSize * game.MapWidth, ElementSize * game.MapHeight + ElementSize);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Text = ".jpegger constructor";
            DoubleBuffered = true;
            var imagesDirectory = new DirectoryInfo("Images");
            foreach (var e in imagesDirectory.GetFiles("*"))
                bitmaps[e.Name] = (Bitmap)Bitmap.FromFile(e.FullName);
            var timer = new Timer();
            timer.Interval = 1;
            timer.Tick += TimerTick;
            timer.Start();
        }

        public static void KeyD(object sender, KeyEventArgs key)
        {
            var x = 0;
            var y = 0;
            var flag = false;
            for (var i = 0; i < game.MapHeight; i++)
                for (var j = 0; j < game.MapWidth; j++)
                {
                    if (game.Map[i, j] is DiggerCursor)
                    {
                        x = j;
                        y = i;
                        flag = true;
                        break;
                    }
                    if (flag) break;
                }
            if (key.KeyCode.ToString() == "Q")
                game.Memory[y, x] = new Terrain();
            if (key.KeyCode.ToString() == "T")
                game.Memory[y, x] = new StaticGold();
            if (key.KeyCode.ToString() == "E")
                game.Memory[y, x] = new StaticSack();
            if (key.KeyCode.ToString() == "R")
                game.Memory[y, x] = new StaticMonster();
            if (key.KeyCode.ToString() == "S") game.DeltaY = 1;
            if (key.KeyCode.ToString() == "W") game.DeltaY = -1;
            if (key.KeyCode.ToString() == "A") game.DeltaX = -1;
            if (key.KeyCode.ToString() == "D") game.DeltaX = 1;
        }

        public static void KeyU(object sender, KeyEventArgs key)
        {
            game.DeltaX = 0;
            game.DeltaY = 0;
        }

        void Act()
        {
            animations.Clear();
            for (int x = 0; x < game.MapWidth; x++)
                for (int y = 0; y < game.MapHeight; y++)
                {
                    var creature = game.Map[x, y];
                    var creature1 = game.Memory[x, y];
                    if (creature != null)
                    {
                        var command = creature.Act(x, y, game);
                        animations.Add(new CreatureAnimation
                        {
                            Command = command,
                            Creature = creature,
                            Location = new Point(x * ElementSize, y * ElementSize)
                        });
                    }
                    if (creature1 != null)
                    {
                        var command1 = creature1.Act(x, y, game);
                        animations.Add(new CreatureAnimation
                        {
                            Command = command1,
                            Creature = creature1,
                            Location = new Point(x * ElementSize, y * ElementSize)
                        });
                    }
                }
            animations = animations.Where(x => x.Creature != null).OrderByDescending(z => z.Creature.GetDrawingPriority).ToList();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(0, ElementSize);
            e.Graphics.FillRectangle(Brushes.Black, 0, 0, ElementSize * game.MapWidth, ElementSize * game.MapHeight);
            foreach (var a in animations)
                e.Graphics.DrawImage(bitmaps[a.Creature.GetImageFileName], a.Location);
            e.Graphics.ResetTransform();
            e.Graphics.DrawString("Q E R T", new Font("Arial", 16), Brushes.Green, 0, 0);
        }

        int tickCount = 0;

        void TimerTick(object sender, EventArgs args)
        {
            if (tickCount == 0) Act();
            foreach (var e in animations)
                e.Location = new Point(e.Location.X + 4 * e.Command.DeltaX, e.Location.Y + 4 * e.Command.DeltaY);
            if (tickCount == 7)
            {
                for (int x = 0; x < game.MapWidth; x++) for (int y = 0; y < game.MapHeight; y++) game.Map[x, y] = null;
                foreach (var e in animations)
                {
                    var x = e.Location.X / 32;
                    var y = e.Location.Y / 32;
                    if (e.Creature is DiggerCursor) game.Map[x, y] = e.Creature;
                }
            }
            tickCount++;
            if (tickCount == 8) tickCount = 0;
            Invalidate();
        }
    }
}
