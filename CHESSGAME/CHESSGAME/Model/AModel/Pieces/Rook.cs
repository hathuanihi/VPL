using System;

namespace CHESSGAME.Model.AModel.Pieces
{
    [Serializable]
    public class Rook : Piece
    {
        public Rook(Color color, Square square) : base(color, square)
        {
            Type = Type.Rook;
        }

        public Rook(Color color) : base(color)
        {
            Type = Type.Rook;
        }

        public override Piece Clone(Square square) => new Rook(Color, square);
        public override string ToString() => "Rook";
    }
}