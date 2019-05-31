using Logic.MovementLogic.Interfaces;
using Logic.StaticHelpers;
using Model;
using Model.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.MovementLogic
{
    public class CheckChecker
    {
        private IMoveChecker _moveChecker;
        public CheckChecker(IMoveChecker checker)
        {
            _moveChecker = checker;
        }

        public bool CheckForCheck(List<GridCell> grid, int kingId, int kingX, int kingY)
        {
            string color = grid.FirstOrDefault(x => x.Piece != null && x.Piece.Id == kingId).Piece.Color;
            foreach (var p in grid)
            {
                if (p.Piece != null && p.Piece.Color != color)
                {
                    if (_moveChecker.ValidMove(grid, p.Piece.Id, kingY, kingX))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckMate(List<GridCell> grid, int kingId, int kingX, int kingY)
        {
            if (!CheckForCheck(grid, kingId, kingX, kingY))
            {
                return false;
            }

            if (!KingsPossibleMoves(grid, kingId, kingX, kingY)) return false;

            return CheckAllPiecesForCheckMate(grid, kingId);
        }

        private bool KingsPossibleMoves(List<GridCell> grid, int kingId, int kingX, int kingY)
        {
            List<int[]> possibleMoves = new List<int[]>()
            {
                new int[2] { kingX + 1, kingY },
                 new int[2] { kingX + 1, kingY - 1 },
                 new int[2] { kingX, kingY - 1 },
                 new int[2] { kingX - 1, kingY - 1 },
                 new int[2] { kingX - 1, kingY },
                 new int[2] { kingX - 1, kingY + 1 },
                 new int[2] { kingX, kingY + 1 },
                 new int[2] { kingX + 1, kingY + 1 },
            };

            foreach (var coords in possibleMoves)
            {
                if (_moveChecker.ValidMove(grid, kingId, coords[0], coords[1]))
                {
                    if (!CheckForCheck(grid, kingId, coords[0], coords[1]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckAllPiecesForCheckMate(List<GridCell> grid, int kingId)
        {
            string color = grid.FirstOrDefault(x => x.Piece != null && x.Piece.Id == kingId).Piece.Color;
            List<GridCell> colorPieces = grid.FindAll(x => x.Piece != null && x.Piece.Color == color);
            foreach (var cell in colorPieces)
            {
                for (int y = 1; y <= 8; y++)
                {
                    for (int x = 1; x <= 8; x++)
                    {
                        if (_moveChecker.ValidMove(grid, cell.Piece.Id, y, x))
                        {
                            var king = grid.FirstOrDefault(k => k.Piece != null && k.Piece.Color == cell.Piece.Color && k.Piece.Type == PieceType.type.King);
                            if (CheckForCheck(grid, king.Piece.Id, king.XCoord, king.YCoord))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
