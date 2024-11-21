﻿using System;

namespace CHESSGAME.Model.AModel.Pieces
{
    [Serializable]
    public class Knight : Piece
    {
        public Knight(Color color, Square square) : base(color, square)
        {
            Type = Type.Knight;
        }

        public Knight(Color color) : base(color)
        {
            Type = Type.Knight;
        }

        public override Piece Clone(Square square) => new Knight(Color, square);

        public override string ToString() => "Knight";
    }
}