using Billy;
using Logic;
using Logic.MovementLogic;
using Model;
using Model.Pieces;
using System;
using System.Collections.Generic;

namespace ConsoleView.View
{
    public class UI
    {
        public void PrintRound(int roundNumber, List<GridCell> gridCells)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Round: " + roundNumber);

            GridPrinter.Print(gridCells);
        }

        public void WriteTurn(bool isWhiteTurn)
        {
            Console.WriteLine("Whites Turn: " + isWhiteTurn);
        }

        public void PressKeyToContinue()
        {
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadLine();
        }

        public int GetIntInput(string msg)
        {
            int input;
            do
            { 
                Console.Write(msg);
            } while (!int.TryParse(Console.ReadLine(), out input));
            return input;
        }

        public int GetIntInputToEight(string msg)
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
    }
}
