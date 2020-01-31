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
        public List<GridCell> Map { get; private set; }

        public Grid(List<GridCell> gridCells)
        {
            _history = new History();
            Map = gridCells;
        }

        public GridCell GetByCoords(int xCoord, int yCoord) =>
            Map.FirstOrDefault(c => c.XCoord == xCoord && c.YCoord == yCoord);

        public GridCell GetByPieceId(int pieceId) =>
            Map.FirstOrDefault(k => k.Piece != null && k.Piece.Id == pieceId);

        public GridCell GetKingByColor(string color) =>
            Map.FirstOrDefault(k => k.Piece != null && k.Piece.Color == color && k.Piece.Type == Model.Pieces.PieceType.type.King);

        public List<GridCell> GetAllByColor(string color) =>
            Map.FindAll(x => x.Piece != null && x.Piece.Color == color);

        public List<GridCell> GetAllByOppositeColor(string color) =>
            Map.FindAll(x => x.Piece != null && x.Piece.Color != color);

        public void MovePiece(int pieceId, int newX, int newY)
        {
            var invadingCell = GetByPieceId(pieceId);
            var capturedCell = GetByCoords(newX, newY);

            _history.Add(capturedCell, invadingCell);

            invadingCell.Piece.HasMoved = true;  
            
            if(invadingCell.Piece.Type == Model.Pieces.PieceType.type.Pawn && (newY == 1 || newY == 8))
                invadingCell.Piece.Type = Model.Pieces.PieceType.type.Queen;

            capturedCell.Piece = invadingCell.Piece;
            invadingCell.Piece = null;
        }

        public void RevertHistory()
        {
            var node = _history.Revert();

            Map.FirstOrDefault(x => x.XCoord == node.PreviousX && x.YCoord == node.PreviousY).Piece = node.InvadingPiece;
            Map.FirstOrDefault(x => x.XCoord == node.ToX && x.YCoord == node.ToY).Piece = node.CapturedPiece;
        }
    }
}
