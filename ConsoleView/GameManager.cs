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
    public class GameManager
    {
        public Board Board { get; private set; }
        public Brain Billy { get; set; }
        public UI Ui { get; set; }
        public int RoundNumber { get; set; }

        public GameManager()
        {
            Ui = new UI();
            Board = new Board(LogicFactory.CreateIMoveChecker());
            Billy = new Brain(new CheckChecker(new MoveChecker()), new MoveChecker());
        }

        public void Run()
        {
            Board.SetBoard();
            RoundNumber = 1;
            while (true)
            {    
                Ui.PrintRound(RoundNumber, Board.Grid);         
                Ui.WriteTurn(Board.IsWhitesTurn());

                int id;
                int x;
                int y;
                if (!Board.IsWhitesTurn())
                {
                    Ui.PressKeyToContinue();
                    var move = Billy.DecideMove(Board.Grid, "Black");
                    id = move[0];
                    x = move[1];
                    y = move[2];
                }
                else
                {
                    id = Ui.GetIntInput("Id:");
                    x = Ui.GetIntInputToEight("X:");
                    y = Ui.GetIntInputToEight("Y:");
                }

                if (Board.MakeMove(id, x, y))
                    RoundNumber++;
            }
        }
    }
}
