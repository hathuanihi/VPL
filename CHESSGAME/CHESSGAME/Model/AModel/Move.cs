using CHESSGAME.Model.AModel.Pieces;
using System;
using System.Runtime.Serialization;
using Type = CHESSGAME.Model.AModel.Pieces.Type;

namespace CHESSGAME.Model.AModel
{
    [Serializable]
    [DataContract]
    public class Move
    {
        [DataMember]
        public Coordinate StartCoordinate { get; set; }

        [DataMember]
        public Coordinate TargetCoordinate { get; set; }

        [DataMember]
        public Color PieceColor { get; set; }

        [DataMember]
        public Type PieceType { get; set; }

        [DataMember]
        public Type PromotePieceType { get; set; }

        #region Constructors
        public Move(Piece piece, Square targetSquare)
        {
            PieceColor = piece.Color;
            PieceType = piece.Type;
            StartCoordinate = piece.Square.Coordinate;
            TargetCoordinate = targetSquare.Coordinate;
        }
        public Move(Square startSquare, Square targetSquare, Type pieceType, Color pieceColor)
        {
            PieceColor = pieceColor;
            PieceType = pieceType;
            StartCoordinate = startSquare.Coordinate;
            TargetCoordinate = targetSquare.Coordinate;
        }
        public Move(Square startSquare, Square targetSquare, Type pieceType, Color pieceColor, Type promotePieceType)
        {
            PieceColor = pieceColor;
            PieceType = pieceType;
            StartCoordinate = startSquare.Coordinate;
            TargetCoordinate = targetSquare.Coordinate;
            PromotePieceType = promotePieceType;
        }
        #endregion
    }
}