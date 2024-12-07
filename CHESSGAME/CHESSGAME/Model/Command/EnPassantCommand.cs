using System;
using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;
using Type = CHESSGAME.Model.AModel.Pieces.Type;

namespace CHESSGAME.Model.Command
{
    [Serializable]
    public class EnPassantCommand : ICompensableCommand
    {
        private ICompensableCommand _firstMove; // Xử lý di chuyển tốt từ vị trí ban đầu sang ô trung gian
        private ICompensableCommand _secondMove; // Xử lý di chuyển tốt từ ô trung gian sang ô mục tiêu, loại bỏ quân tốt bị bắt


        public EnPassantCommand(Move move, Board board)
        {
            Move = move;

            bool isWhite = move.PieceColor == Color.White; // Trả về true nếu tốt đang di chuyển là trắng
            bool isLeft = move.StartCoordinate.X > move.TargetCoordinate.X; // Trả về true nếu tốt di chuyển sang trái

            int x = move.StartCoordinate.X + (isLeft ? -1 : 1);
            int y = move.StartCoordinate.Y;

            Square startSquare = board.SquareAt(move.StartCoordinate); // Ô mà tốt đang đứng đầu
            Square secondSquare = board.Squares[x, y]; // Ô mà tốt sẽ di chuyển đến
            Square thirdSquare = board.Squares[x, y + (isWhite ? -1 : 1)]; // Nơi bắt quân

            _firstMove = new MoveCommand(new Move(startSquare, secondSquare, Move.PieceType, Move.PieceColor), board);
            _secondMove = new MoveCommand(new Move(secondSquare, thirdSquare, Move.PieceType, Move.PieceColor), board);
        }

        private EnPassantCommand(EnPassantCommand command, Board board)
        {
            Move = command.Move;
            _firstMove = command._firstMove.Copy(board);
            _secondMove = command._secondMove.Copy(board);
        }

        public void Execute() // Thực thi
        {
            _firstMove.Execute();
            _secondMove.Execute();
        }

        public void Compensate() // Hoàn tác
        {
            _secondMove.Compensate();
            _firstMove.Compensate();
        }

        public bool TakePiece => true; // Trả về true vì thực hiện bắt quân

        public Move Move { get; }

        public Type PieceType => Move.PieceType;

        public Color PieceColor => Move.PieceColor;

        public ICompensableCommand Copy(Board board) => new EnPassantCommand(this, board);

        public override string ToString() => "En passant from " + Move.StartCoordinate + " to " + Move.TargetCoordinate;
    }
}