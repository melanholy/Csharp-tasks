using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    public interface ICreature
    {
        int GetDrawingPriority { get; }
        string GetImageFileName { get; }
        CreatureCommand Act(int x, int y, Game game);
        bool DeadInConflict(ICreature conflictedObject, Game game);
    }
}
