using System.Collections.Generic;
using System.Linq;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.Model.AModel;

namespace CHESSGAME.ViewModel.Engine.Rules
{
    public class PawnMovementRule : IRule
    {
        public bool IsMoveValid(Move move, Board board)
        {
            Square targetSquare = board.SquareAt(move.TargetCoordinate);
            Piece piece = board.PieceAt(move.StartCoordinate);
            Square square = board.SquareAt(move.StartCoordinate);
            bool isWhite = piece.Color == Color.White;

            // Trả về true nếu Tốt đang ở vị trí đầu tiên
            bool isStartPosition = ((piece.Square.Y == 1) && !isWhite) || ((piece.Square.Y == 6) && isWhite);

            if (targetSquare.Piece == null)
            {
                bool normalMove =
                        // Mỗi lượt đi chỉ được di chuyển lên ô gần nhất
                        ((piece.Square.Y - targetSquare.Y == (isWhite ? 1 : -1)) ||

                        // Lượt đi đầu tiên được di chuyển 2 ô
                        (isStartPosition && (piece.Square.Y - targetSquare.Y == (isWhite ? 2 : -2)))) &&

                        // Di chuyển theo cột
                        (piece.Square.X == targetSquare.X) &&
                        (board.Squares[square.X, isWhite ? square.Y - 1 : square.Y + 1].Piece == null);

                Pawn leftPiece = square.X > 0 ? board.Squares[square.X - 1, square.Y]?.Piece as Pawn : null;
                Pawn rightPiece = square.X < 7 ? board.Squares[square.X + 1, square.Y]?.Piece as Pawn : null;

                if (leftPiece?.EnPassant == true && leftPiece.Color != piece.Color)
                    if ((targetSquare.X == square.X - 1) && (piece.Square.Y - targetSquare.Y == (isWhite ? 1 : -1)))
                        return true;
                if (rightPiece?.EnPassant == true && rightPiece.Color != piece.Color)
                    if ((targetSquare.X == square.X + 1) && (piece.Square.Y - targetSquare.Y == (isWhite ? 1 : -1)))
                        return true;

                return normalMove;
            }
            return // Điều kiện ăn đối phương
                ((piece.Square.X == targetSquare.X - 1) || (piece.Square.X == targetSquare.X + 1)) &&
                (piece.Square.Y - targetSquare.Y == (isWhite ? 1 : -1));
        }

        public List<Square> PossibleMoves(Piece piece)
        {
            return piece.Square.Board.Squares.OfType<Square>().ToList().FindAll(
                x => IsMoveValid(new Move(piece, x), piece.Square.Board));
        }
    }
}