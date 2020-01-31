using Logic.MovementLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.MovementLogic
{
    public static class LogicFactory
    {
        public static IMoveChecker CreateIMoveChecker() => new MoveChecker();
        public static ICheckChecker CreateICheckChecker() => new CheckChecker(new MoveChecker());
    }
}
