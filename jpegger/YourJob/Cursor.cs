using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    class DiggerCursor : ICreature
    {
        public string GetImageFileName { get { return "cursor.png"; } }
        public int GetDrawingPriority { get { return -1; } }

        public CreatureCommand Act(int x, int y, Game game)
        {
            var cond1 = game.DeltaX == 1 && (x == game.MapWidth - 1);
            var cond2 = game.DeltaX == -1 && (x == 0);
            var cond3 = game.DeltaY == 1 && (y == game.MapHeight - 1);
            var cond4 = game.DeltaY == -1 && (y == 0);
            if (cond1) return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new DiggerCursor() };
            if (cond2) return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new DiggerCursor() };
            if (cond3) return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new DiggerCursor() };
            if (cond4) return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new DiggerCursor() };
            return new CreatureCommand { DeltaX = game.DeltaX, DeltaY = game.DeltaY, TransformTo = new DiggerCursor() };
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            return false;
        }
    }
}
