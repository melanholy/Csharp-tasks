using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
	public class JpeggerGame : Form
    {
        const int ElementSize = 32;
        Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        static List<CreatureAnimation> animations = new List<CreatureAnimation>();
        static Game game = new Game("terrain1.txt");

        public JpeggerGame()
        {
            this.KeyDown += KeyD;
            this.KeyUp += KeyU;
            ClientSize = new Size(ElementSize * game.MapWidth, ElementSize * game.MapHeight + ElementSize);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Text = ".jpegger";
            DoubleBuffered = true;
            var imagesDirectory = new DirectoryInfo("Images");
            foreach(var e in imagesDirectory.GetFiles("*"))
                bitmaps[e.Name]=(Bitmap)Bitmap.FromFile(e.FullName);
            var timer = new Timer();
            timer.Interval = 1;
            timer.Tick += TimerTick;
            timer.Start();
        }

        public static void KeyU(object sender, KeyEventArgs key)
        {
            game.DeltaY = 0;
            game.DeltaX = 0;
        }

        public static void KeyD(object sender, KeyEventArgs key)
        {
            if (key.KeyCode.ToString() == "S") game.DeltaY = 1;
            if (key.KeyCode.ToString() == "W") game.DeltaY = -1;
            if (key.KeyCode.ToString() == "A") game.DeltaX = -1;
            if (key.KeyCode.ToString() == "D") game.DeltaX = 1;
        }

        void Act()
        {
            animations.Clear();
            for (int x = 0; x < game.MapWidth; x++)
                for (int y = 0; y < game.MapHeight; y++)
                {
                    var creature = game.Map[x, y];
                    if (creature == null) continue;
                    var command = creature.Act(x,y, game);
                    animations.Add(new CreatureAnimation
                    {
                        Command = command,
                        Creature = creature,
                        Location = new Point(x * ElementSize, y * ElementSize)
                    });
                }
            animations = animations.OrderByDescending(z => z.Creature.GetDrawingPriority).ToList();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(0,ElementSize);
            e.Graphics.FillRectangle(Brushes.Black,0,0,ElementSize*game.MapWidth,ElementSize*game.MapHeight);
            foreach (var a in animations)
            {
                var bitmap = "";
                if (a.Creature is Digger) 
                    bitmap = Digger.GetImageFileName1(game);
                else bitmap = a.Creature.GetImageFileName;
                e.Graphics.DrawImage(bitmaps[bitmap], a.Location);
            }
            e.Graphics.ResetTransform();
            if (game.Score != game.MaxScore) e.Graphics.DrawString(game.Score.ToString(), new Font("Arial", 16), Brushes.Orange, 0, 0);
            if (game.Score == game.MaxScore)
                e.Graphics.DrawString("YOU WON", new Font("Arial", 16), Brushes.Green, 0, 0);
        }

        int tickCount = 0;

        void TimerTick(object sender, EventArgs args)
        {
            if (tickCount == 0) Act();
            foreach (var e in animations)
                e.Location = new Point(e.Location.X + 4*e.Command.DeltaX, e.Location.Y + 4*e.Command.DeltaY);
            if (tickCount==7)
            {
                for (int x=0;x<game.MapWidth;x++) for (int y=0;y<game.MapHeight;y++) game.Map[x,y]=null;
                foreach(var e in animations)
                {
                    var x=e.Location.X/32;
                    var y=e.Location.Y/32;
                    var nextCreature = e.Command.TransformTo == null ? e.Creature : e.Command.TransformTo;
                    if (game.Map[x, y] == null) game.Map[x, y] = nextCreature;
                    else
                    {
                        bool newDead = nextCreature.DeadInConflict(game.Map[x, y], game);
                        bool oldDead = game.Map[x, y].DeadInConflict(nextCreature, game);
                        if (newDead && oldDead) game.Map[x, y] = null;
                        else if (!newDead && oldDead) game.Map[x, y] = nextCreature;
                    }
                }
            }
            tickCount++;
            if (tickCount == 8) tickCount = 0;
            Invalidate();
        }
    }
}
