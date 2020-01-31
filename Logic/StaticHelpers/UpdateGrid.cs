using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.StaticHelpers
{
    public static class UpdateGrid
    {
        private static History _history = new History();
        public static List<GridCell> MovePiece(List<GridCell> grid, int pieceId, int newX, int newY)
        {
            _history.Add(grid, pieceId, newX, newY);
            var piece = grid.FirstOrDefault(x => x.Piece != null && x.Piece.Id == pieceId).Piece;
            piece.HasMoved = true;                                 
            if(piece.Type == Model.Pieces.PieceType.type.Pawn && (newY == 1 || newY == 8))
            {
                piece.Type = Model.Pieces.PieceType.type.Queen;
            }
            grid.FirstOrDefault(x => x.Piece != null && x.Piece.Id == pieceId).Piece = null;
            grid.FirstOrDefault(x => x.XCoord == newX && x.YCoord == newY).Piece = piece;
            return grid;
        }

        public static List<GridCell> RevertHistory(List<GridCell> grid)
        {
            var node = _history.Revert();

            grid.FirstOrDefault(x => x.XCoord == node.PreviousX && x.YCoord == node.PreviousY).Piece = node.InvadingPiece;
            grid.FirstOrDefault(x => x.XCoord == node.ToX && x.YCoord == node.ToY).Piece = node.CapturedPiece;
            return grid;
        }
    }
}
