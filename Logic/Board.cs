using Logic.MovementLogic;
using Logic.MovementLogic.Interfaces;
using Logic.StaticHelpers;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Board
    {
        private static List<GridCell> _grid;
        private IMoveChecker _moveChecker;
        private CheckChecker _checkChecker; 
        private static bool WhitesTurn = true;

        public Board(IMoveChecker move)
        {
            _moveChecker = move;
            _checkChecker = new CheckChecker(move);
        }        

        public void SetBoard()
        {
            _grid = BoardSetup.SetBoard();
        }

        public bool MakeMove(int pieceId, int newX, int newY)
        {
            string currentColorsTurn = WhitesTurn ? "White" : "Black";
            string opponentsColor = WhitesTurn ? "Black" : "White";

            var cell = _grid.FirstOrDefault(k => k.Piece != null && k.Piece.Id == pieceId);

            if (cell == null)
            {
                return false;
            }

            var piece = cell.Piece;

            if (cell != null && cell.Piece.Color != currentColorsTurn)
            {
                return false;
            }

            if (!_moveChecker.ValidMove(_grid, pieceId, newY, newX))
            {
                return false;
            }

            _grid = UpdateGrid.MovePiece(_grid, pieceId, newX, newY);

            if (CheckForCheck(currentColorsTurn))
            {
                _grid = UpdateGrid.RevertHistory(_grid);
                return false;
            }

            CheckForCheckMate(opponentsColor);

            WhitesTurn = !WhitesTurn;

            return true;
        }

        private void CheckForCheckMate(string opponentsColor)
        {
            var opponentking = _grid.FirstOrDefault(k => k.Piece != null && k.Piece.Color == opponentsColor && k.Piece.Type == Model.Pieces.PieceType.type.King);
            if (_checkChecker.CheckMate(_grid, opponentking.Piece.Id, opponentking.XCoord, opponentking.YCoord))
            {
                throw new Exception("Checkmate on " + opponentsColor);
            }
        }

        private bool CheckForCheck(string currentColorsTurn)
        {
            GridCell king = _grid.FirstOrDefault(k => k.Piece != null && k.Piece.Color == currentColorsTurn && k.Piece.Type == Model.Pieces.PieceType.type.King);
            if (_checkChecker.CheckForCheck(_grid, king.Piece.Id, king.XCoord, king.YCoord))
            {                
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<GridCell> GetGrid()
        {
            return _grid;
        }

        public bool IsWhitesTurn()
        {
            return WhitesTurn;
        } 
    }
}
