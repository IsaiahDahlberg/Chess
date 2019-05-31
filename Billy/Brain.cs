using Logic.MovementLogic;
using Logic.MovementLogic.Interfaces;
using Logic.StaticHelpers;
using Model;
using Model.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billy
{
    public class Brain
    {
        private List<GridCell> _grid;
        private string _billysColor;
        private CheckChecker _checkChecker;
        private IMoveChecker _moveChecker;

        public int[] DecideMove(List<GridCell> grid, string color, CheckChecker checkChecker, IMoveChecker moveChecker)
        {
            _grid = grid;
            _billysColor = color;
            _checkChecker = checkChecker;
            _moveChecker = moveChecker;

            List<GridCell> billyPieces = grid.FindAll(x => x.Piece != null && x.Piece.Color == color);

            List<MoveSet> AllBestMoves = new List<MoveSet>();
            foreach (var p in billyPieces)
            {
                if (p.Piece == null)
                {
                    continue;
                }
                var h = HighestScoredMove(p);
                if (h.Score == -1)
                {
                    continue;
                }

                _grid = UpdateGrid.MovePiece(_grid, h.Piece.Id, h.XCoord, h.YCoord);

                List<GridCell> op1 = grid.FindAll(x => x.Piece != null && x.Piece.Color != color);
                MoveSet obm1 = FindBestMoveForColor(op1);
                if (obm1.Score == -1)
                {
                    _grid = UpdateGrid.RevertHistory(_grid);
                    h.Score += 10000;
                    AllBestMoves.Add(h);
                    continue;
                }
                _grid = UpdateGrid.MovePiece(_grid, obm1.Piece.Id, obm1.XCoord, obm1.YCoord);
                h.Score -= (obm1.Score / 2);

                List<GridCell> bp1 = grid.FindAll(x => x.Piece != null && x.Piece.Color == color);
                MoveSet bbm1 = FindBestMoveForColor(bp1);
                if (bbm1.Score == -1) continue;
                _grid = UpdateGrid.MovePiece(_grid, bbm1.Piece.Id, bbm1.XCoord, bbm1.YCoord);
                h.Score += bbm1.Score;

                List<GridCell> op2 = grid.FindAll(x => x.Piece != null && x.Piece.Color != color);
                MoveSet obm2 = FindBestMoveForColor(op2);
                if (obm2.Score == -1)
                {
                    _grid = UpdateGrid.RevertHistory(_grid);
                    _grid = UpdateGrid.RevertHistory(_grid);
                    _grid = UpdateGrid.RevertHistory(_grid);
                    h.Score += 10000;
                    AllBestMoves.Add(h);
                    continue;
                }
                _grid = UpdateGrid.MovePiece(_grid, obm2.Piece.Id, obm2.XCoord, obm2.YCoord);
                h.Score -= (obm2.Score / 2);

                List<GridCell> bp2 = grid.FindAll(x => x.Piece != null && x.Piece.Color == color);
                MoveSet bbm2 = FindBestMoveForColor(bp2);
                if (bbm2.Score == -1) continue;
                h.Score += bbm2.Score / 2;

                AllBestMoves.Add(h);
                _grid = UpdateGrid.RevertHistory(_grid);
                _grid = UpdateGrid.RevertHistory(_grid);
                _grid = UpdateGrid.RevertHistory(_grid);
                _grid = UpdateGrid.RevertHistory(_grid);
            }

            return ConvertMoveSetToIntArray(PickBestMove(AllBestMoves));
        }

        private MoveSet PickBestMove(List<MoveSet> AllBestMoves)
        {
            List<MoveSet> move = AllBestMoves.FindAll(x => x.Score == AllBestMoves.Max(y => y.Score));
            MoveSet selected;
            if (move.Count() > 2)
            {
                Random r = new Random();
                selected = move[r.Next(0, move.Count())];
            }
            else
            {
                selected = move[0];
            }
            return selected;
        }

        private int[] ConvertMoveSetToIntArray(MoveSet move)
        {
            return new int[3] { move.Piece.Id, move.XCoord, move.YCoord };
        }

        private MoveSet FindBestMoveForColor(List<GridCell> grid)
        {
            List<MoveSet> moves = new List<MoveSet>();
            foreach (var p in grid)
            {
                moves.Add(HighestScoredMove(p));
            }
            return moves.FirstOrDefault(j => j.Score == moves.Max(x => x.Score));
        }

        private MoveSet HighestScoredMove(GridCell cell)
        {
            List<MoveSet> moveSets = FindPossibleMoves(cell);
            if (!moveSets.Any())
            {
                return new MoveSet()
                {
                    Score = -1
                };
            }
            moveSets = ScoreMoves(moveSets);
            MoveSet highestScoredMove = moveSets[0];
            foreach (var m in moveSets)
            {
                if (m.Score >= highestScoredMove.Score)
                {
                    highestScoredMove = m;
                }
            }
            return highestScoredMove;
        }

        private List<MoveSet> ScoreMoves(List<MoveSet> moveSets)
        {
            foreach (var m in moveSets)
            {
                if(m.Piece.Type == PieceType.type.Pawn && (m.YCoord == 1 || m.YCoord == 8))
                {
                    m.Score += 300;
                }
                var capturedCell = _grid.FirstOrDefault(x => x.XCoord == m.XCoord && x.YCoord == m.YCoord);
                if (capturedCell.Piece != null)
                {
                    switch (capturedCell.Piece.Type)
                    {
                        case PieceType.type.Pawn:
                            m.Score += 20;
                            break;
                        case PieceType.type.Bishop:
                            m.Score += 120;
                            break;
                        case PieceType.type.Knight:
                            m.Score += 120;
                            break;
                        case PieceType.type.Rook:
                            m.Score += 200;
                            break;
                        case PieceType.type.Queen:
                            m.Score += 500;
                            break;
                        case PieceType.type.King:
                            m.Score += 500000000;
                            break;
                    }
                }
                m.Score -= m.YCoord / 4;
            }
            return moveSets;
        }

        private List<MoveSet> FindPossibleMoves(GridCell cell)
        {
            List<MoveSet> moveSets = new List<MoveSet>();
            for (int y = 1; y <= 8; y++)
            {
                for (int x = 1; x <= 8; x++)
                {
                    if (_moveChecker.ValidMove(_grid, cell.Piece.Id, y, x))
                    {
                        _grid = UpdateGrid.MovePiece(_grid, cell.Piece.Id, x, y);
                        var piece = _grid.FirstOrDefault(c => c.XCoord == x && c.YCoord == y && c.Piece != null).Piece;
                        var king = _grid.FirstOrDefault(k => k.Piece != null && k.Piece.Color == piece.Color && k.Piece.Type == PieceType.type.King);
                        if (!_checkChecker.CheckForCheck(_grid, king.Piece.Id, king.XCoord, king.YCoord))
                        {
                            MoveSet move = new MoveSet()
                            {
                                XCoord = x,
                                YCoord = y,
                                Piece = piece,
                                Score = 16
                            };
                            moveSets.Add(move);
                        }
                        _grid = UpdateGrid.RevertHistory(_grid);
                    }
                }
            }
            return moveSets;
        }

        private class MoveSet
        {
            public int Score { get; set; }
            public Piece Piece { get; set; }
            public int XCoord { get; set; }
            public int YCoord { get; set; }
        }
    }
}
