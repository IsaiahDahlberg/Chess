using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.MovementLogic.Interfaces
{
    public interface ICheckChecker
    {
        bool CheckForCheck(Grid grid, int kingId, int kingX, int kingY);
        bool CheckMate(Grid grid, int kingId, int kingX, int kingY);
    }
}
