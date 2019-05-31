using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.MovementLogic.Interfaces
{
    public interface IMoveChecker
    {
      bool ValidMove(List<GridCell> grid, int id, int newY, int newX);
    }
}
