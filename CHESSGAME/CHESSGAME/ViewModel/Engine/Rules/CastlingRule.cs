using System;
using System.Collections.Generic;
using System.Linq;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.Model.AModel;
using Type = CHESSGAME.Model.AModel.Pieces.Type;

namespace CHESSGAME.ViewModel.Engine.Rules
{
    public class CastlingRule : IRule
    {
        public bool IsMoveValid(Move move, Board board)
        {
            Square targetSquare = board.SquareAt(move.TargetCoordinate);
            Piece piece = board.PieceAt(move.StartCoordinate);

            if ((Math.Abs(move.TargetCoordinate.X - move.StartCoordinate.X) == 2) && (move.TargetCoordinate.Y == move.StartCoordinate.Y))
            {
                // Kiểm tra không có quân cản giữa vua và xe
                if (!NoPiecesBetween(move, board))
                    return false;

                // Xác định xe nhập thành
                Piece possibleRook = board.PieceAt(new Coordinate(move.TargetCoordinate.X > move.StartCoordinate.X ? 7 : 0, // Xe phải hay trái
                        move.StartCoordinate.Y));
                return !piece.HasMoved && (!possibleRook?.HasMoved == true) && (possibleRook?.Type == Type.Rook);
            }

            if ((targetSquare?.Piece?.Type == Type.Rook) && (targetSquare?.Piece?.Color == piece.Color))
            {
                if (!NoPiecesBetween(move, board))
                    return false;

                return !piece.HasMoved && !targetSquare.Piece.HasMoved; // Nhập thành ngược
            }

            return true;
        }

        public List<Square> PossibleMoves(Piece piece)
        {
            return piece.Square.Board.Squares.OfType<Square>().ToList().FindAll(
                x => IsMoveValid(new Move(piece, x), piece.Square.Board));
        }

        private static bool NoPiecesBetween(Move move, Board board) // Trả về true nếu giữa vua và xe không có quân cờ khác
            => (
                move.TargetCoordinate.X > move.StartCoordinate.X ? board.Squares.OfType<Square>().ToList() .FindAll(
                x => (x.Y == move.StartCoordinate.Y) && (x.X < 7) && (x.X > move.StartCoordinate.X))
                : 
                board.Squares.OfType<Square>().ToList().FindAll(
                x => (x.Y == move.StartCoordinate.Y) && (x.X > 0) && (x.X < move.StartCoordinate.X))
               ).All(x => x.Piece == null);
    }
}