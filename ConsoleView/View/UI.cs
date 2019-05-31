using Billy;
using Logic;
using Logic.MovementLogic;
using Model;
using Model.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleView.View
{
    public class UI
    {
        Board _board;
        List<GridCell> grid;
        Brain billy;
        public UI(Board b)
        {
            _board = b;
            billy = new Brain();
        }

        public void run()
        {
            _board.SetBoard();
            int round = 1;
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Round: " + round);
                GetGrid();
                PrintGrid();
                int id;
                int x;
                int y;
                Console.WriteLine("Whites Turn: " + _board.IsWhitesTurn());
                if (!_board.IsWhitesTurn())
                {
                    Console.ReadLine();
                    var move = billy.DecideMove(_board.GetGrid(), "Black", new CheckChecker(new MoveChecker()), new MoveChecker());
                    id = move[0];
                    x = move[1];
                    y = move[2];
                }
                else
                {

                    id = GetIntInput("Id:");
                    x = GetIntInputToEight("X:");
                    y = GetIntInputToEight("Y:");
                }
 
                if(_board.MakeMove(id, x, y))
                {
                    round++;
                }
              
            }
            
        
        }

        private int GetIntInput(string msg)
        {
            int input;
            do
            { 
                Console.Write(msg);
            } while (!int.TryParse(Console.ReadLine(), out input));
            return input;
        }

        private int GetIntInputToEight(string msg)
        {
            int input;
            while (true)
            {
                do
                {
                    Console.Write(msg);
                } while (!int.TryParse(Console.ReadLine(), out input));
                if (input > 8 || input < 1)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            return input;
        }
        private void PrintGrid()
        {
            Console.WriteLine("|------------------------------------------------|");
            Console.Write("|");
            for (int i = 1; i <= grid.Count(); i++)
            { 
                string current = "";
                if (grid[i - 1].Piece == null)
                {
                    Console.Write("|" + grid[i-1].XCoord +" "+grid[i-1].YCoord + " |");
                }
                else
                {
                    if (grid[i - 1].Piece.Color == "Black")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        current += "B";
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        current += "W";
                    }
                    switch (grid[i - 1].Piece.Type)
                    {
                        case PieceType.type.Pawn:
                            current += "p";
                            break;
                        case PieceType.type.Bishop:
                            current += "b";
                            break;
                        case PieceType.type.King:
                            current += "$";
                            break;
                        case PieceType.type.Knight:
                            current += "k";
                            break;
                        case PieceType.type.Queen:
                            current += "Q";
                            break;
                        case PieceType.type.Rook:
                            current += "r";
                            break;
                    }
                    current += grid[i - 1].Piece.Id;
                    if (current.Length < 4)
                        current += " ";
                    Console.Write("|" + current + "|");
                    Console.ForegroundColor = ConsoleColor.White;
                }
             
                if (i % 8 == 0)
                {
                    Console.WriteLine("|");
                    Console.WriteLine("||    ||    ||    ||    ||    ||    ||    ||    ||");
                    Console.WriteLine("|------------------------------------------------|");
                    Console.Write("|");
                }
            }
        }

        private void GetGrid()
        {
            grid = _board.GetGrid();
        }
    }
}
