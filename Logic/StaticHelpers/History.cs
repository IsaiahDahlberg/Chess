using Model;
using Model.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.StaticHelpers
{
    public class History
    {
        private List<HistoryNode> _history;

        public History()
        {
            _history = new List<HistoryNode>();
        }
        public void Add(List<GridCell> grid, int pieceId, int newX, int newY)
        {

            var invadingCell = grid.FirstOrDefault(x => x.Piece != null && x.Piece.Id == pieceId);
            var capturedCell = grid.FirstOrDefault(x => x.XCoord == newX && x.YCoord == newY);

            Piece invadingPiece = new Piece()
            {
                Color = invadingCell.Piece.Color,
                Id = invadingCell.Piece.Id,
                Type = invadingCell.Piece.Type
            };

            HistoryNode node = new HistoryNode()
            {
                Id = FindNextNodeId(),
                CapturedPiece = capturedCell.Piece,
                InvadingPiece = invadingPiece,
                PreviousX = invadingCell.XCoord,
                PreviousY = invadingCell.YCoord,
                ToX = capturedCell.XCoord,
                ToY = capturedCell.YCoord
            };

            _history.Add(node);
        }

        public HistoryNode Revert()
        {
            if (!_history.Any())
                throw new Exception("History is empty");

            HistoryNode node = _history.FirstOrDefault(x => x.Id == _history.Max(y => y.Id));
            _history.Remove(node);
            return node;
        }

        private int FindNextNodeId()
        {
            if (!_history.Any())
                return 1;
            return _history.Max(x => x.Id) + 1;
        }
    }

}
