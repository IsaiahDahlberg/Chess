using Logic;
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
        private Grid _grid;
        private string _billysColor;
        private CheckChecker _checkChecker;
        private IMoveChecker _moveChecker;

        public Brain(CheckChecker checkChecker, IMoveChecker moveChecker)
        {
            _checkChecker = checkChecker;
            _moveChecker = moveChecker;
        }

        public MoveSet DecideMove(Grid grid, string color)
        {
            _billysColor = color;
            _grid = grid;
            List<GridCell> billyPieces = grid.GetAllByColor(color);

            List<MoveSet> AllBestMoves = new List<MoveSet>();
            foreach (var p in billyPieces)
            {
                if (p.Piece == null)
                {
                    continue;
                }
                var highestScoredMove = HighestScoredMove(p);
                if (highestScoredMove.Score == -1)
                {
                    continue;
                }

                grid.MovePiece(highestScoredMove.Piece.Id, highestScoredMove.XCoord, highestScoredMove.YCoord);

                List<GridCell> op1 = grid.GetAllByOppositeColor(color);
                MoveSet obm1 = FindBestMoveForColor(op1);
                if (obm1.Score == -1)
                {
                    grid.RevertHistory();
                    highestScoredMove.Score += 10000;
                    AllBestMoves.Add(highestScoredMove);
                    continue;
                }
                grid.MovePiece(obm1.Piece.Id, obm1.XCoord, obm1.YCoord);
                highestScoredMove.Score -= (obm1.Score / 2);

                List<GridCell> bp1 = grid.GetAllByColor(color);
                MoveSet bbm1 = FindBestMoveForColor(bp1);
                if (bbm1.Score == -1) continue;
                grid.MovePiece(bbm1.Piece.Id, bbm1.XCoord, bbm1.YCoord);
                highestScoredMove.Score += bbm1.Score;

                List<GridCell> op2 = grid.GetAllByOppositeColor(color);
                MoveSet obm2 = FindBestMoveForColor(op2);
                if (obm2.Score == -1)
                {
                    grid.RevertHistory();
                    grid.RevertHistory();
                    grid.RevertHistory();
                    highestScoredMove.Score += 10000;
                    AllBestMoves.Add(highestScoredMove);
                    continue;
                }
                grid.MovePiece(obm2.Piece.Id, obm2.XCoord, obm2.YCoord);
                highestScoredMove.Score -= (obm2.Score / 2);

                List<GridCell> bp2 = grid.GetAllByColor(color);
                MoveSet bbm2 = FindBestMoveForColor(bp2);
                if (bbm2.Score == -1) continue;
                highestScoredMove.Score += bbm2.Score / 2;

                AllBestMoves.Add(highestScoredMove);
                grid.RevertHistory();
                grid.RevertHistory();
                grid.RevertHistory();
                grid.RevertHistory();
            }

            return PickBestMove(AllBestMoves);
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
                var capturedCell = _grid.GetByCoords(m.XCoord, m.YCoord);
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
                        _grid.MovePiece(cell.Piece.Id, x, y);
                        var piece = _grid.GetByCoords(x, y).Piece;
                        var king = _grid.GetKingByColor(piece.Color);
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
                        _grid.RevertHistory();
                    }
                }
            }
            return moveSets;
        }
    }
}
