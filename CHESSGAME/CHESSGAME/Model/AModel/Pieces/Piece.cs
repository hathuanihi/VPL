using System;
using System.Runtime.Serialization;

namespace CHESSGAME.Model.AModel.Pieces
{
    [Serializable]
    public abstract class Piece
    {
        protected Piece(Color color, Square square)
        {
            Color = color;
            Square = square;
        }

        protected Piece(Color color)
        {
            Color = color;
            Square = null;
        }

        public Color Color { get; }
        public Square Square { get; set; }

        public bool HasMoved { get; set; } = false; // kiểm tra nếu quân cờ đã được di chuyển

        public Type Type { get; set; }

        public abstract Piece Clone(Square square);
    }

    [Serializable]
    public enum Type
    {
        [EnumMember]
        Bishop,
        [EnumMember]
        King,
        [EnumMember]
        Queen,
        [EnumMember]
        Pawn,
        [EnumMember]
        Knight,
        [EnumMember]
        Rook
    }

    [Serializable]
    public enum Color
    {
        [EnumMember]
        White,
        [EnumMember]
        Black
    }
}