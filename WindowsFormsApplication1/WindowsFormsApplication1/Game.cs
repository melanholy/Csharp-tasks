using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WindowsFormsApplication1.Objects;

namespace WindowsFormsApplication1
{
    public class Game
    {
        public List<IMovable> MovableObjects;
        public List<IGameObject> Walls;
        public List<Floor> FloorSprites;
        public List<IGameObject> InteractiveObjects;
        public List<BloodStain> BloodStains; 
        public Hero Hero;
        public Cursor Cursor;
        public int DeltaX;
        public int DeltaY;
        public char[,] Graph;
        public bool IsOver => Hero.IsDead;
        public List<string> Map;
        public int ObjectSize => 34;
        public List<List<string>> Levels;

        public bool IsLevelComplete
        {
            get
            {
                return MovableObjects.OfType<Enemy>().All(x => x.IsDead);
            }
        }

        public Game(List<string> map, List<List<string>> levels)
        {
            Levels = levels;
            var width = int.Parse(map[0].Split(' ')[0]);
            var height = int.Parse(map[0].Split(' ')[1]);
            Map = map;
            Graph = new char[width, height + 2];
            MovableObjects = new List<IMovable>();
            Walls = new List<IGameObject>();
            FloorSprites = new List<Floor>();
            InteractiveObjects = new List<IGameObject>();
            BloodStains = new List<BloodStain>();
            DeltaX = 0;
            DeltaY = 0;
            var objSize = ObjectSize;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                    switch (map[y + 1][x])
                    {
                        case 'H':
                            var hero = new Hero(x*objSize, y*objSize, this);
                            MovableObjects.Add(hero);
                            Hero = hero;
                            var cursor = new Cursor(x*objSize, y*objSize, this);
                            MovableObjects.Add(cursor);
                            Cursor = cursor;
                            break;
                        case 'G':
                            var gun = new Gun(x*objSize, y*objSize, 4);
                            InteractiveObjects.Add(gun);
                            break;
                        case '-':
                            var hWall = new HWall(x*objSize, y*objSize);
                            Graph[x, y] = 'w';
                            Walls.Add(hWall);
                            break;
                        case 'E':
                            var enemy = new Enemy(x*objSize, y*objSize, this);
                            Graph[x, y] = 'e';
                            MovableObjects.Add(enemy);
                            break;
                        case '|':
                            var vWall = new VWall(x*objSize, y*objSize);
                            Graph[x, y] = 'w';
                            Walls.Add(vWall);
                            break;
                        case 'K':
                            var knife = new Knife(x*objSize, y*objSize);
                            InteractiveObjects.Add(knife);
                            break;
                    }
                for (var y = height + 1; y < 2*height; y++)
                    FloorSprites.Add(new Floor(x * objSize, (y - height) * objSize, string.Format("Floor{0}.png", map[y+1][x])));
            }
        }

        public void Update()
        {
            var rand = new Random();
            var toRemove = new List<IMovable>();
            foreach (var e in MovableObjects)
            {
                var canMove = true;
                if (e is Hero)
                {
                    if (Walls.Any(v => Geometry.Distance(new Point(e.Position.X + DeltaX, e.Position.Y + DeltaY), v.Position) < 20 &&
                                 (DeltaY != 0 && v is HWall || DeltaX != 0 && v is VWall)))
                        canMove = false;
                }
                if (e is Enemy)
                {
                    var enemy = e as Enemy;
                    if (Hero.Animated > 12 && Geometry.Distance(Hero.Position, e.Position) < 50 &&
                        Geometry.GetAngle(Hero.Position, e.Position, Hero.Angle + Math.PI) < Math.PI/2)
                    {
                        if (!enemy.IsDead) enemy.Kill();
                        BloodStains.Add(new BloodStain(enemy.Position.X - 10 + rand.Next(-5, 5),
                            enemy.Position.Y - 10 + rand.Next(-5, 5)));
                    }
                    canMove = !enemy.IsDead;
                    if (enemy.Animated > 12 && Geometry.Distance(Hero.Position, e.Position) < 50 &&
                        Geometry.GetAngle(e.Position, Hero.Position, e.Angle + Math.PI) < Math.PI/2)
                    {
                        Hero.IsDead = true;
                        BloodStains.Add(new BloodStain(Hero.Position.X - 30 + rand.Next(-5, 5),
                            Hero.Position.Y - 30 + rand.Next(-5, 5)));
                    }
                }
                if (e is Bullet)
                {
                    foreach (var enemy in MovableObjects.Where(x => x is Enemy && Geometry.Distance(e.Position, x.Position) < 20).Select(v => v as Enemy))
                    {
                        toRemove.Add(e);
                        if (!enemy.IsDead) enemy.Kill();
                        BloodStains.Add(new BloodStain(e.Position.X - 10 + rand.Next(-5, 5),
                            e.Position.Y - 10 + rand.Next(-5, 5)));
                        break;
                    }
                    if (Walls.Any(x => Geometry.Distance(e.Position, x.Position) < 20))
                        toRemove.Add(e);
                }
                if (canMove) e.Move();
            }
            foreach (var e in toRemove)
                MovableObjects.Remove(e);
        }
    }
}
