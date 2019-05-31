using ConsoleView.View;
using Logic.MovementLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleView
{
    class Program
    {
        static void Main(string[] args)
        {
            UI ui = new UI(new Logic.Board(LogicFactory.CreateIMoveChecker()));
            ui.run();
        }
    }
}
