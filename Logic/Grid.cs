using Logic.StaticHelpers;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
    public class Grid
    {
        private readonly History _history;
        public List<GridCell> GridMap { get; private set; }

        public Grid(List<GridCell> gridCells)
        {
            _history = new History();
            GridMap = gridCells;
        }

        public void MovePiece(int pieceId, int newX, int newY)
        {
            _history.Add(GridMap, pieceId, newX, newY);
            var piece = GridMap.FirstOrDefault(x => x.Piece != null && x.Piece.Id == pieceId).Piece;
            piece.HasMoved = true;                                 
            if(piece.Type == Model.Pieces.PieceType.type.Pawn && (newY == 1 || newY == 8))
            {
                piece.Type = Model.Pieces.PieceType.type.Queen;
            }
            GridMap.FirstOrDefault(x => x.Piece != null && x.Piece.Id == pieceId).Piece = null;
            GridMap.FirstOrDefault(x => x.XCoord == newX && x.YCoord == newY).Piece = piece;
        }

        public void RevertHistory()
        {
            var node = _history.Revert();

            GridMap.FirstOrDefault(x => x.XCoord == node.PreviousX && x.YCoord == node.PreviousY).Piece = node.InvadingPiece;
            GridMap.FirstOrDefault(x => x.XCoord == node.ToX && x.YCoord == node.ToY).Piece = node.CapturedPiece;
        }

        public GridCell GetPiece(int pieceId)
        {
            return GridMap.FirstOrDefault(k => k.Piece != null && k.Piece.Id == pieceId);
        }

        public GridCell GetKing(string color)
        {
            return GridMap.FirstOrDefault(k => k.Piece != null && k.Piece.Color == color && k.Piece.Type == Model.Pieces.PieceType.type.King);
        }
    }
}
