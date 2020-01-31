using Logic.MovementLogic;
using Logic.MovementLogic.Interfaces;
using Logic.StaticHelpers;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
    public class Board
    {
        public Grid Grid { get; private set; }

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
            Grid = new Grid(BoardSetup.CreateGridCellList());
        }

        public bool MakeMove(int pieceId, int newX, int newY)
        {
            string currentColorsTurn = WhitesTurn ? "White" : "Black";
            string opponentsColor = WhitesTurn ? "Black" : "White";

            var cell = Grid.GetByPieceId(pieceId);

            if (cell == null)
                return false;

            if (cell != null && cell.Piece.Color != currentColorsTurn)
                return false;

            if (!_moveChecker.ValidMove(Grid, pieceId, newY, newX))
                return false;

            Grid.MovePiece(pieceId, newX, newY);

            if (CheckForCheck(currentColorsTurn))
            {
                Grid.RevertHistory();
                return false;
            }

            CheckForCheckMate(opponentsColor);

            WhitesTurn = !WhitesTurn;

            return true;
        }

        private void CheckForCheckMate(string opponentsColor)
        {
            var opponentking = Grid.GetKingByColor(opponentsColor);
            if (_checkChecker.CheckMate(Grid, opponentking.Piece.Id, opponentking.XCoord, opponentking.YCoord))
            {
                throw new Exception("Checkmate on " + opponentsColor);
            }
        }

        private bool CheckForCheck(string currentColorsTurn)
        {
            GridCell king = Grid.GetKingByColor(currentColorsTurn);
            if (_checkChecker.CheckForCheck(Grid, king.Piece.Id, king.XCoord, king.YCoord))
            {                
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsWhitesTurn()
        {
            return WhitesTurn;
        } 
    }
}
