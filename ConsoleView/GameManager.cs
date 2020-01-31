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
                Ui.Print(RoundNumber, Board.Grid);         
                Ui.WriteTurn(Board.IsWhitesTurn());
          
                if (!Board.IsWhitesTurn())
                {
                    Ui.PressKeyToContinue();
                    var billysMove = Billy.DecideMove(Board.Grid, "Black");
                    LetPlayerMovePiece(billysMove.Piece.Id, billysMove.XCoord, billysMove.YCoord);
                }
                else
                {
                    LetPlayerMovePiece(
                        Ui.GetIntInput("Id:"),
                        Ui.GetIntInputToEight("X:"),
                        Ui.GetIntInputToEight("Y:"));
                }
            }
        }

        private void LetPlayerMovePiece(int id, int x, int y)
        {
            if (Board.MakeMove(id, x, y))
                RoundNumber++;
        }
    }
}
