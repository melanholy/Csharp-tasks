using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    public class Digger : ICreature
    {
        public string GetImageFileName { get { return "Digger.png"; } }
        public int GetDrawingPriority { get { return 0; } }

        public static string GetImageFileName1(Game game)
        {
            if (game.Score <= game.MaxScore / 7) return "Digger.png";
            if (game.Score <= game.MaxScore * 2 / 7) return "Digger1.png";
            if (game.Score <= game.MaxScore * 3 / 7) return "Digger2.png";
            if (game.Score <= game.MaxScore * 4 / 7) return "Digger3.png";
            if (game.Score <= game.MaxScore * 5 / 7) return "Digger4.png";
            if (game.Score <= game.MaxScore * 6 / 7) return "Digger5.png";
            return "Digger6.png";
        }

        public CreatureCommand Act(int x, int y, Game game)
        {
            var cond1 = game.DeltaX == 1 && (x == game.MapWidth - 1 || game.Map[x + 1, y] != null && game.Map[x + 1, y] is Sack);
            var cond2 = game.DeltaX == -1 && (x == 0 || game.Map[x - 1, y] != null && game.Map[x - 1, y] is Sack);
            var cond3 = game.DeltaY == 1 && (y == game.MapHeight - 1 || game.Map[x, y + 1] != null && game.Map[x, y + 1] is Sack);
            var cond4 = game.DeltaY == -1 && (y == 0 || game.Map[x, y - 1] != null && game.Map[x, y - 1] is Sack);
            if (cond1) return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Digger() };
            if (cond2) return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Digger() };
            if (cond3) return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Digger() };
            if (cond4) return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Digger() };
            return new CreatureCommand { DeltaX = game.DeltaX, DeltaY = game.DeltaY, TransformTo = new Digger() };
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            return conflictedObject is Monster || conflictedObject is Sack;
        }
    }
}
