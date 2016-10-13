using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    public class Sack : ICreature
    {
        public string GetImageFileName { get { return "Sack.jpg"; } }
        public int GetDrawingPriority { get { return 1; } }
        static string transform;

        public CreatureCommand Act(int x, int y, Game game)
        {
            var delta = 0;
            var cond = y != game.MapHeight - 1 &&
                (
                    game.Map[x, y + 1] == null
                    || game.Map[x, y + 1] is Digger
                    || game.Map[x, y + 1] is Monster
                );
            if (cond)
            {
                delta = 1;
                transform = String.Format("{0} {1}", x, y + 1);
            }
            if (transform != null && y != game.MapHeight - 1 && x.ToString() == transform.Split(' ')[0] && y.ToString() == transform.Split(' ')[1] && game.Map[x, y + 1] is Terrain)
                return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Gold() };
            if (y == game.MapHeight - 1) return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Gold() };
            return new CreatureCommand { DeltaX = 0, DeltaY = delta, TransformTo = new Sack() };
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            return false;
        }
    }
}
