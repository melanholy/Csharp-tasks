using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
    public class Game
    {
        public int DeltaX { get; set; }
        public int DeltaY { get; set; }
        public ICreature[,] Memory { get; set; }
        public ICreature[,] Map { get; set; }
        public readonly int MapWidth;
        public readonly int MapHeight;
        public int Score { get; private set; }
        public readonly int MaxScore;

        public void IncreaseScore()
        {
            Score++;
        }

        public Game(string level)
        {
            var ls = File.ReadAllLines(level);
            MapWidth = ls[0].Length;
            MapHeight = ls.Length;
            Map = new ICreature[ls[0].Length, ls.Length];
            for (var i = 0; i < MapWidth; i++)
                for (var j = 0; j < MapHeight; j++)
                {
                    if (ls[j][i]=='#') Map[i, j] = new Terrain();
                    if (ls[j][i] == 'D') Map[i, j] = new Digger();
                    if (ls[j][i] == 'G')
                    {
                        Map[i, j] = new Gold();
                        MaxScore++;
                    }
                    if (ls[j][i] == '$')
                    {
                        Map[i, j] = new Sack();
                        MaxScore++;
                    }
                    if (ls[j][i] == 'Z') Map[i, j] = new Monster();
                    if (ls[j][i] == 'X') Map[i, j] = new Monster1();
                }
        }

        public Game(int height, int width)
        {
            MapWidth = width;
            MapHeight = height;
            Map = new ICreature[height, width];
            Memory = new ICreature[height, width];
            Map[0, 0] = new DiggerCursor();
        }
    }
}
