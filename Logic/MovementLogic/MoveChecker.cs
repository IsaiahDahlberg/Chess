using Logic.MovementLogic.Interfaces;
using Model;
using Model.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.MovementLogic
{
    public class MoveChecker : IMoveChecker
    {
        public bool ValidMove(List<GridCell> grid, int id, int newY, int newX)
        {
            if (newY > 8 || newX > 8 || newX < 1 || newY < 1)
            {
                return false;
            }

            GridCell cell = grid.FirstOrDefault(x =>x.Piece != null && x.Piece.Id == id);

            if(cell == null)
            {
                throw new Exception("No cell found at line 21 in MoveChecker");
            }

            if (CheckForByColor(grid, newY, newX, cell.Piece.Color))
            {
                return false;
            }

            switch (cell.Piece.Type)
            {
                case PieceType.type.Pawn:
                    return PawnMoveSet(grid, cell, newY, newX);
                case PieceType.type.Bishop:
                    return BishopMoveSet(grid, cell, newY, newX);
                case PieceType.type.King:
                    return KingMoveSet(grid, cell, newY, newX);
                case PieceType.type.Knight:
                    return KnightMoveSet(grid, cell, newY, newX);
                case PieceType.type.Queen:
                    return QueenMoveSet(grid, cell, newY, newX);
                case PieceType.type.Rook:
                    return RookMoveSet(grid, cell, newY, newX);
            }

            throw new Exception("No valid cell.Piece.Type was provided. Switch Statement at 27 in MoveChecker");
        }

        private bool CheckForByColor(List<GridCell> grid, int yCoord, int xCoord, string color)
        {
            var foundCell = grid.FirstOrDefault(c => c.XCoord == xCoord && c.YCoord == yCoord);
            if (foundCell.Piece != null && foundCell.Piece.Color == color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckForAny(List<GridCell> grid, int yCoord, int xCoord)
        {
            if (grid.Any(c => c.Piece != null && c.XCoord == xCoord && c.YCoord == yCoord))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool RookMoveSet(List<GridCell> grid, GridCell cell, int newY, int newX)
        {
            if (newX == cell.XCoord && newY != cell.YCoord)
            {
                int difference = Math.Abs(newY - cell.YCoord);
                int direction = (newY - cell.YCoord) / difference;
                for (int i = cell.YCoord+direction; i != (newY); i += direction)
                {
                    if (CheckForAny(grid, i, newX))
                    {
                        return false;
                    }
                }
                return true;
            }

            if (newX != cell.XCoord && newY == cell.YCoord)
            {
                int difference = Math.Abs(newX - cell.XCoord);
                int direction = (newX - cell.XCoord) / difference;
                for (int i = cell.XCoord+direction; i != (newX); i += direction)
                {
                    if (CheckForAny(grid, newY, i))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private bool QueenMoveSet(List<GridCell> grid, GridCell cell, int newY, int newX)
        {
            if (newX == cell.XCoord && newY != cell.YCoord)
            {
                int difference = Math.Abs(newY - cell.YCoord);
                int direction = (newY - cell.YCoord) / difference;
                for (int i = cell.YCoord+direction; i != (newY); i += direction)
                {
                    if (CheckForAny(grid, i, newX))
                    {
                        return false;
                    }
                }
                return true;
            }

            if (newX != cell.XCoord && newY == cell.YCoord)
            {
                int difference = Math.Abs(newX - cell.XCoord);
                int direction = (newX - cell.XCoord) / difference;
                
                for (int i = cell.XCoord+direction; i != (newX); i += direction)
                {
                    if (CheckForAny(grid, newY, i))
                    {
                        return false;
                    }
                }
                return true;
            }

            int xDifference = Math.Abs(newX - cell.XCoord);
            int xDirection = (newX - cell.XCoord) / xDifference;
            List<int> xCoords = new List<int>();

            int yDifference = Math.Abs(newY - cell.YCoord);
            int yDirection = (newY - cell.YCoord) / yDifference;
            List<int> yCoords = new List<int>();

            if (yDifference != xDifference)
            {
                return false;
            }

            //if (yDifference == 1 )
            //{
            //    xCoords.Add(cell.XCoord + xDirection);
            //    yCoords.Add(cell.YCoord + yDirection); 
            //}

            for (int i = cell.XCoord+xDirection; i != (newX); i += xDirection)
            {
                xCoords.Add(i);
            }

            for (int i = cell.YCoord+yDirection; i != (newY); i += yDirection)
            {
                yCoords.Add(i);
            }

            for (int i = 0; i < xCoords.Count(); i++)
            {
                if (CheckForAny(grid, yCoords[i], xCoords[i]))
                {
                    return false;
                }
            }
            return true;                
        }

        private bool KnightMoveSet(List<GridCell> grid, GridCell cell, int newY, int newX)
        {
            if ((Math.Abs(newY - cell.YCoord) == 2) && (Math.Abs(newX - cell.XCoord) == 1))
            {
                return true;
            }
            if ((Math.Abs(newX - cell.XCoord) == 2) && (Math.Abs(newY - cell.YCoord) == 1))
            {
                return true;
            }
            return false;
        }

        private bool BishopMoveSet(List<GridCell> grid, GridCell cell, int newY, int newX)
        {
            int xDifference = Math.Abs(newX - cell.XCoord);
            if (xDifference == 0) return false;
            int xDirection = (newX - cell.XCoord) / xDifference;
            List<int> xCoords = new List<int>();

            int yDifference = Math.Abs(newY - cell.YCoord);
            if (yDifference == 0) return false;
            int yDirection = (newY - cell.YCoord) / yDifference;
            List<int> yCoords = new List<int>();

            if (yDifference != xDifference)
            {
                return false;
            }
            
            for (int i = cell.XCoord+xDirection; i != (newX); i += xDirection)
            {
                xCoords.Add(i);
            }

            
            for (int i = cell.YCoord+yDirection; i != (newY); i += yDirection)
            {
                yCoords.Add(i);
            }

            for (int i = 0; i < xCoords.Count(); i++)
            {
                if (CheckForAny(grid, yCoords[i], xCoords[i]))
                {
                    return false;
                }              
            }
            return true;
        }

        private bool KingMoveSet(List<GridCell> grid, GridCell cell, int newY, int newX)
        {
            if (Math.Abs(newY - cell.YCoord) <= 1 && Math.Abs(newX - cell.XCoord) <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool PawnMoveSet(List<GridCell> grid, GridCell cell, int newY, int newX)
        {
            if (cell.Piece.Color == "Black")
            {
                if (newX == cell.XCoord)
                {
                    if ((cell.YCoord - newY == 1 || (cell.YCoord == 7 && cell.YCoord - newY == 2)) && !CheckForAny(grid, newY, newX))
                    {
                        return true;
                    }
                }
                else
                {
                    if (cell.YCoord - newY == 1 && Math.Abs(cell.XCoord - newX) == 1)
                    {
                        if (CheckForByColor(grid, newY, newX, "White"))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            if (cell.Piece.Color == "White")
            {
                if (newX == cell.XCoord)
                {
                    if ((newY - cell.YCoord == 1 || (cell.YCoord == 2 && newY - cell.YCoord == 2)) && !CheckForAny(grid, newY, newX))
                    {
                        return true;
                    }
                }
                else
                {
                    if (newY - cell.YCoord == 1 && Math.Abs(newX - cell.XCoord) == 1)
                    {
                        if (CheckForByColor(grid, newY, newX, "Black"))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            throw new Exception("Cell.Piece.Color was not a valid Color. Valid Colors are White and Black");
        }
    }
}
