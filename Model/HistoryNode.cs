using Model.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class HistoryNode
    {
        public int Id { get; set; }
        public int PreviousX { get; set; }
        public int PreviousY { get; set; }
        public Piece CapturedPiece { get; set; }
        public int ToX { get; set; }
        public int ToY { get; set; }
        public Piece InvadingPiece { get; set; }
    }
}
