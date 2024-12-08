using System.Collections.Generic;
using System.Linq;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.Model.AModel;

namespace CHESSGAME.ViewModel.Engine.Rules
{
    public class RookMovementRule : IRule
    {
        public bool IsMoveValid(Move move, Board board) // Trả về false nếu nước đi không phải ngang hay dọc
        {
            Square targetSquare = board.SquareAt(move.TargetCoordinate);
            Piece piece = board.PieceAt(move.StartCoordinate);

            // Nếu không thẳng hàng
            if (!((piece.Square.X == targetSquare.X) ^ (piece.Square.Y == targetSquare.Y))) return false;

            return board.Squares.OfType<Square>().Where(x => piece.Square.Y == targetSquare.Y
                        ? Between(piece.Square.X, targetSquare.X, x.X) && (x.Y == targetSquare.Y)
                        : // Ngang
                        Between(piece.Square.Y, targetSquare.Y, x.Y) && (x.X == targetSquare.X)) // Dọc                                                                          
                        .All(betweenSquare => betweenSquare.Piece == null); // Các ô từ ô đầu đến đích phải là ô trống
        }

        public List<Square> PossibleMoves(Piece piece) // Danh sách các ô mà Xe có thể di chuyển
        {
            return piece.Square.Board.Squares.OfType<Square>().ToList().FindAll(
                x => IsMoveValid(new Move(piece, x), piece.Square.Board));
        }

        private static bool Between(int i, int j, int x)  // Trả về true nếu ô x nằm giữa 2 ô i j
            => i > j ? (i > x) && (j < x) : (j > x) && (i < x);
    }
}