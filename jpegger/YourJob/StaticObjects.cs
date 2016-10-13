using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    public class Terrain : ICreature
    {
        public string GetImageFileName { get { return "Terrain.jpg"; } }
        public int GetDrawingPriority { get { return 10; } }

        public CreatureCommand Act(int x, int y, Game game)
        {
            return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Terrain() };
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            return !(conflictedObject is DiggerCursor);
        }
    }

    public class Gold : ICreature
    {
        public string GetImageFileName { get { return "Gold.jpg"; } }
        public int GetDrawingPriority { get { return 2; } }
        public CreatureCommand Act(int x, int y, Game game)
        {
            return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Gold() };
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            if (conflictedObject is Digger) game.IncreaseScore();
            return true;
        }
    }
}
