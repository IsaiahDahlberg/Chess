using Model;
using Model.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.StaticHelpers
{
    public static class BoardSetup
    {
        private static int startingId;

        public static List<GridCell> CreateGridCellList()
        {
            startingId = 0;
            List<GridCell> grid = CreateGrid();
            grid = SetPieces("White", grid);
            grid = SetPieces("Black", grid);
            return grid;
        }

        private static List<GridCell> CreateGrid()
        {
            List<GridCell> grid = new List<GridCell>();
            for (int y = 1; y <= 8; y++)
            {
                for (int x = 1; x <= 8; x++)
                {
                    GridCell cell = new GridCell()
                    {
                        XCoord = x,
                        YCoord = y
                    };
                    grid.Add(cell);
                }
            }
            return grid;
        }

        private static List<GridCell> SetPieces(string color, List<GridCell> grid)
        {
            int pawnYCoord;
            int primaryYCoord;
            if (color == "Black")
            {
                pawnYCoord = 7;
                primaryYCoord = 8;
            }
            else
            {
                pawnYCoord = 2;
                primaryYCoord = 1;
            }
            for (int i = 1; i <= 8; i++)
            {
                grid.FirstOrDefault(c => c.XCoord == i && c.YCoord == pawnYCoord).Piece = CreatePawn(color);
                grid.FirstOrDefault(c => c.XCoord == i && c.YCoord == primaryYCoord).Piece = CreatePrimary(primaryYCoord, i, color);
            }
            return grid;
        }

        private static Piece CreatePrimary(int yCoord, int xCoord, string color)
        {
            Piece piece = new Piece()
            {
                Color = color,
                Id = AssignId()
            };
            switch (xCoord)
            {
                case 1:
                    piece.Type = PieceType.type.Rook;
                    break;
                case 2:
                    piece.Type = PieceType.type.Knight;
                    break;
                case 3:
                    piece.Type = PieceType.type.Bishop;
                    break;
                case 4:
                    if (color == "White")
                    {
                        piece.Type = PieceType.type.Queen;
                    }
                    else
                    {
                        piece.Type = PieceType.type.King;
                    }
                    break;
                case 5:
                    if (color == "Black")
                    {
                        piece.Type = PieceType.type.Queen;
                    }
                    else
                    {
                        piece.Type = PieceType.type.King;
                    }
                    break;
                case 6:
                    piece.Type = PieceType.type.Bishop;
                    break;
                case 7:
                    piece.Type = PieceType.type.Knight;
                    break;
                case 8:
                    piece.Type = PieceType.type.Rook;
                    break;
            }
            return piece;
        }

        private static Piece CreatePawn(string color)
        {
            Piece pawn = new Piece()
            {
                Color = color,
                Id = AssignId(),
                Type = PieceType.type.Pawn
            };
            return pawn;
        }

        private static int AssignId()
        {
            startingId += 1;
            return startingId;
        }
    }
}