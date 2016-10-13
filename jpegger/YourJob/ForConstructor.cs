using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    public class StaticMonster : ICreature
    {
        public string GetImageFileName { get { return "Monster.png"; } }
        public int GetDrawingPriority { get { return 1; } }

        public CreatureCommand Act(int x, int y, Game game)
        {
            return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new StaticMonster() };
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            return false;
        }
    }

    public class StaticSack : ICreature
    {
        public string GetImageFileName { get { return "Sack.jpg"; } }
        public int GetDrawingPriority { get { return 1; } }

        public CreatureCommand Act(int x, int y, Game game)
        {
            return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new StaticSack() };
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            return false;
        }
    }

    public class StaticGold : ICreature
    {
        public string GetImageFileName { get { return "Gold.jpg"; } }
        public int GetDrawingPriority { get { return 2; } }

        public CreatureCommand Act(int x, int y, Game game)
        {
            return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new StaticGold() };
        }

        public bool DeadInConflict(ICreature conflictedObject, Game game)
        {
            return false;
        }
    }
}
