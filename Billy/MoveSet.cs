using Model.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billy
{
    public class MoveSet
    {
        public int Score { get; set; }
        public Piece Piece { get; set; }
        public int XCoord { get; set; }
        public int YCoord { get; set; }
    }
}
