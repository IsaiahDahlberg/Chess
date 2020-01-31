using Billy;
using ConsoleView.View;
using Logic;
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
            GameManager gm = new GameManager();
            gm.Run();
        }
    }
}
