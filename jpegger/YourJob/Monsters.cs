using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    public class Monster : ICreature
    {
        static Random rand = new Random();
        static string lastMove = "";
        public string GetImageFileName { get { return "Monster.png"; } }
        public int GetDrawingPriority { get { return 1; } }

        public CreatureCommand Act(int x, int y, Game game)
        {
            var coordOfDigger = MonsterAI.FindDigger(game);
            var x2 = coordOfDigger[0];
            var y2 = coordOfDigger[1];
            var props = MonsterAI.FollowDigger(lastMove, rand, x, y, x2, y2, game);
            var deltaX = int.Parse(props[0]);
            var deltaY = int.Parse(props[1]);
            lastMove = props[2];
            if (deltaX != 0 || deltaY != 0) return new CreatureCommand { DeltaX = deltaX, DeltaY = deltaY, TransformTo = new Monster() };
            bool left = false, right = false, up = false, down = false;
            props = MonsterAI.WalkSomewhere(lastMove, rand, x, y, game);
            right = bool.Parse(props[0]);
            left = bool.Parse(props[1]);
            up = bool.Parse(props[2]);
            down = bool.Parse(props[3]);
            if (down) deltaY = 1;
            else if (up) deltaY = -1;
            else if (right) deltaX = 1;
            else if (left) deltaX = -1;
            return new CreatureCommand { DeltaX = deltaX, DeltaY = deltaY, TransformTo = new Monster() };
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            return conflictedObject is Sack;
        }
    }

    public class Monster1 : ICreature
    {
        static Random rand = new Random();
        static string lastMove = "";
        public string GetImageFileName { get { return "Monster.png"; } }
        public int GetDrawingPriority { get { return 1; } }

        public CreatureCommand Act(int x, int y, Game game)
        {
            var coordOfDigger = MonsterAI.FindDigger(game);
            var x2 = coordOfDigger[0];
            var y2 = coordOfDigger[1];
            var props = MonsterAI.FollowDigger(lastMove, rand, x, y, x2, y2, game);
            var deltaX = int.Parse(props[0]);
            var deltaY = int.Parse(props[1]);
            lastMove = props[2];
            if (deltaX != 0 || deltaY != 0) return new CreatureCommand { DeltaX = deltaX, DeltaY = deltaY, TransformTo = new Monster() };
            bool left = false, right = false, up = false, down = false;
            props = MonsterAI.WalkSomewhere(lastMove, rand, x, y, game);
            right = bool.Parse(props[0]);
            left = bool.Parse(props[1]);
            up = bool.Parse(props[2]);
            down = bool.Parse(props[3]);
            if (down) deltaY = 1;
            else if (up) deltaY = -1;
            else if (right) deltaX = 1;
            else if (left) deltaX = -1;
            return new CreatureCommand { DeltaX = deltaX, DeltaY = deltaY, TransformTo = new Monster() };
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            return conflictedObject is Sack;
        }
    }

    public class MonsterAI
    {
        public static int[] FindDigger(Game game)
        {
            int flag = 0, x2 = 0, y2 = 0;
            for (var x1 = 0; x1 < game.MapWidth; x1++)
            {
                for (var y1 = 0; y1 < game.MapHeight; y1++)
                {
                    if (game.Map[x1, y1] != null && game.Map[x1, y1] is Digger)
                    {
                        x2 = x1;
                        y2 = y1;
                        flag = 1;
                        break;
                    }
                }
                if (flag == 1) break;
            }
            return new int[] { x2, y2 };
        }

        public static string[] FollowDigger(string lastMove, Random rand, int x, int y, int x2, int y2, Game game)
        {
            int deltaX = 0, deltaY = 0;
            //if ((lastMove != "left" || rand.Next(10) == 1) && x != game.MapWidth - 1 && x < x2 && (game.Map[x + 1, y] == null || game.Map[x + 1, y].ToString() == "Digger.Digger"))
            if (lastMove != "left" && x != game.MapWidth - 1 && x < x2 && (game.Map[x + 1, y] == null || game.Map[x + 1, y] is Digger))
            {
                deltaX = 1;
                lastMove = "right";
            }
            //else if ((lastMove != "right" || rand.Next(10) == 1) && x != 0 && x > x2 && (game.Map[x - 1, y] == null || game.Map[x - 1, y].ToString() == "Digger.Digger"))
            else if (lastMove != "right" && x != 0 && x > x2 && (game.Map[x - 1, y] == null || game.Map[x - 1, y] is Digger))
            {
                lastMove = "left";
                deltaX = -1;
            }
            //else if ((lastMove != "down" || rand.Next(10) == 1) && y != 0 && y > y2 && (game.Map[x, y - 1] == null || game.Map[x, y - 1].ToString() == "Digger.Digger"))
            else if (lastMove != "down" && y != 0 && y > y2 && (game.Map[x, y - 1] == null || game.Map[x, y - 1] is Digger))
            {
                deltaY = -1;
                lastMove = "up";
            }
            //else if ((lastMove != "up" || rand.Next(10) == 1) && y != game.MapHeight - 1 && y < y2 && (game.Map[x, y + 1] == null || game.Map[x, y + 1].ToString() == "Digger.Digger"))
            else if (lastMove != "up" && y != game.MapHeight - 1 && y < y2 && (game.Map[x, y + 1] == null || game.Map[x, y + 1] is Digger))
            {
                deltaY = 1;
                lastMove = "down";
            }
            return new string[] { deltaX.ToString(), deltaY.ToString(), lastMove };
        }

        public static string[] WalkSomewhere(string lastMove, Random rand, int x, int y, Game game)
        {
            bool left = false, right = false, up = false, down = false;
            if ((lastMove != "left" || rand.Next(3) == 1) && x != game.MapWidth - 1 && (game.Map[x + 1, y] == null || game.Map[x + 1, y] is Digger))
            {
                right = true;
                lastMove = "right";
            }
            if ((lastMove != "right" || rand.Next(3) == 1) && x != 0 && (game.Map[x - 1, y] == null || game.Map[x - 1, y] is Digger))
            {
                left = true;
                lastMove = "left";
            }
            if ((lastMove != "up" || rand.Next(3) == 1) && y != game.MapHeight - 1 && (game.Map[x, y + 1] == null || game.Map[x, y + 1] is Digger))
            {
                down = true;
                lastMove = "down";
            }
            if ((lastMove != "down" || rand.Next(3) == 1) && y != 0 && (game.Map[x, y - 1] == null || game.Map[x, y - 1] is Digger))
            {
                up = true;
                lastMove = "up";
            }
            return new string[] { right.ToString(), left.ToString(), up.ToString(), down.ToString(), lastMove };
        }
    }
}
