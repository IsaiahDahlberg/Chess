using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Pieces
{
    public class Piece
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public PieceType.type Type { get; set; }
        public bool HasMoved { get; set; } = false;
    }
}
