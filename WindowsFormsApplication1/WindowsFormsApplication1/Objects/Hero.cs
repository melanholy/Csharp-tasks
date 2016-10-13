using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace WindowsFormsApplication1.Objects
{
    public class Hero : IMovable
    {
        public Point Position { get; set; }
        public string Image => GetImage();
        public Game Game { get; }
        public int DrawingPriority => IsDead ? 4 : 2;
        public double Angle { get; private set; }
        public string Weapon { get; private set; }
        private readonly Stopwatch lastShot;
        public int Shots { get; private set; }
        public int Animated { get; private set; }
        static readonly Dictionary<string, string> normalPictures = new Dictionary<string, string>
        {
            { "", "Hero.png" }, { "Gun", "HeroWGun.png" }, { "Knife", "HeroWKnife.png" }
        };
        static readonly Dictionary<string, string> walk1Pictures = new Dictionary<string, string>
        {
            { "", "HeroWalk1.png" }, { "Gun", "HeroWGunWalk1.png" }, { "Knife", "HeroWKnifeWalk1.png" }
        };
        static readonly Dictionary<string, string> walk2Pictures = new Dictionary<string, string>
        {
            { "", "HeroWalk2.png" }, { "Gun", "HeroWGunWalk2.png" }, { "Knife", "HeroWKnifeWalk2.png" }
        };
        public bool IsDead;

        public Hero(int x, int y, Game game)
        {
            IsDead = false;
            Shots = 0;
            Animated = 0;
            lastShot = new Stopwatch();
            lastShot.Start();
            Weapon = "";
            Game = game;
            Angle = 0;
            Position = new Point(x, y);
        }

        private string GetImage()
        {
            if (IsDead) return "DeadHero.png";
            if (Animated > 0)
            {
                Animated--;
                return Animated > 7 ? "HeroSweep1.png" : "HeroSweep2.png";
            }
            if (Animated < 0 && (Animated < -20 || Animated > -10))
            {
                Animated++;
                return Animated < -20 ? walk1Pictures[Weapon] : walk2Pictures[Weapon];
            }
            if (Animated < 0) Animated++;
            return normalPictures[Weapon];
        }

        public void PickUp()
        {
            if (IsDead) return;
            IGameObject toAdd = null;
            IGameObject toDelete = null;
            foreach (var e in Game.InteractiveObjects.Where(e => Geometry.Distance(Position.X, Position.Y, e.Position.X, e.Position.Y) < 40))
            {
                if (Weapon != "")
                    toAdd = Weapon == "Gun" ? (IGameObject) new Gun(Position.X, Position.Y, Shots) : new Knife(Position.X, Position.Y);
                Weapon = e is Gun ? "Gun" : "Knife";
                if (e is Gun)
                {
                    var gun = e as Gun;
                    Shots = gun.Bullets;
                }
                toDelete = e;
                break;
            }
            if (toAdd != null) Game.InteractiveObjects.Add(toAdd);
            if (toDelete != null) Game.InteractiveObjects.Remove(toDelete);
        }

        public void Shoot()
        {
            if (Weapon== "" || IsDead) return;;
            if (Weapon == "Gun")
            {
                lastShot.Stop();
                if (lastShot.ElapsedMilliseconds < 500)
                {
                    lastShot.Start();
                    return;
                }
                lastShot.Restart();
                if (Shots == 0) return;
                Shots--;
                var rand = new Random();
                for (var i = 0; i < 4; i++)
                    Game.MovableObjects.Add(new Bullet((int) (Position.X + 15*Math.Cos(Angle + Math.PI/2)), Position.Y,
                        Angle + rand.NextDouble()/4));
                return;
            }
            lastShot.Stop();
            if (lastShot.ElapsedMilliseconds < 250)
            {
                lastShot.Start();
                return;
            }
            lastShot.Restart();
            Animated = 15;
        }

        public void Move()
        {
            if (IsDead) return;
            var deltaX = Game.DeltaX;
            var deltaY = Game.DeltaY;
            if (Animated == 0)
                Animated = -30;
            if (deltaY == 0 && deltaX == 0 && Animated < 0) Animated = 0;
            if (deltaX != 0 && deltaY != 0)
            {
                deltaY = (int)(deltaY / Math.Sqrt(2));
                deltaY += deltaY > 0 ? 1 : -1;
                deltaX = (int)(deltaX / Math.Sqrt(2));
                deltaX += deltaX > 0 ? 1 : -1;
            }
            Position = new Point(Position.X+deltaX, Position.Y+deltaY);
            var angle = Geometry.GetAngle(Game.Hero.Position, Game.Cursor.Position, Angle);
            if (double.IsNaN(angle)) angle = 0;
            Angle = angle;
        }
    }
}
