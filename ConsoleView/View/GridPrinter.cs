using Model;
using Model.Pieces;
using System;
using System.Collections.Generic;

namespace ConsoleView.View
{
    public static class GridPrinter
    {
        public static void Print(List<GridCell> gridCells)
        {
            Console.WriteLine("|------------------------------------------------|");
            Console.Write("|");
            foreach (var cell in gridCells)
            {
                WritePieceFromCell(cell.Piece);

                if (cell.Piece == null)
                    Console.Write("|" + cell.XCoord + " " + cell.YCoord + " |");

                if (cell.XCoord == 8)
                {
                    Console.WriteLine("|");
                    Console.WriteLine("||    ||    ||    ||    ||    ||    ||    ||    ||");
                    Console.WriteLine("|------------------------------------------------|");
                    Console.Write("|");
                }
            }
        }

        private static void WritePieceFromCell(Piece piece)
        {
            if (piece is null)
                return;

            string uiCell = GetColorFromCell(piece.Color);
            uiCell += GetPieceLetter(piece.Type);
            uiCell += piece.Id;

            if (uiCell.Length < 4)
                uiCell += " ";

            Console.Write("|" + uiCell + "|");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static string GetPieceLetter(PieceType.type type)
        {
            switch (type)
            {
                case PieceType.type.Pawn:
                    return "p";
                case PieceType.type.Bishop:
                    return "b";
                case PieceType.type.King:
                    return "$";
                case PieceType.type.Knight:
                    return "k";
                case PieceType.type.Queen:
                    return "Q";
                case PieceType.type.Rook:
                    return "r";
                default:
                    throw new ArgumentNullException("No PieceType.Type was set");
            }
        }

        private static string GetColorFromCell(string color)
        {
            if (color == "Black")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                return "B";
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                return "W";
            }
        }
    }
}
