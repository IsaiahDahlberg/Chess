using Model.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class GridCell
    {
        public int XCoord { get; set; }
        public int YCoord { get; set; }
        public Piece Piece { get; set; }
    }
}
