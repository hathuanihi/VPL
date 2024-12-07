using System;
using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;
using Type = CHESSGAME.Model.AModel.Pieces.Type;

namespace CHESSGAME.Model.Command
{
    [Serializable]
    public class PromoteCommand : ICompensableCommand
    {
        private Board _board;
        private ICompensableCommand _moveCommand;
        private Piece _oldPawn; // Lưu trữ tốt ban đầu để phục vụ hoàn tác

        public PromoteCommand(Move move, Board board)
        {
            // Ném ngoại lệ vì phong cấp yêu cầu xác định loại quân muốn thay thế
            if (move?.PromotePieceType == null)
                throw new NullReferenceException("Can't build a promote command!");
            _board = board;
            Move = move;

            _moveCommand = new MoveCommand(move, board);
            _oldPawn = board.PieceAt(move.StartCoordinate);
        }

        private PromoteCommand(PromoteCommand promoteCommand, Board board)
        {
            Move = promoteCommand.Move;
            _board = board;
            _moveCommand = promoteCommand._moveCommand.Copy(board);
            _oldPawn = board.PieceAt(Move.StartCoordinate);
        }

        public void Execute() // Thực thi
        {
            _moveCommand.Execute();

            Square square = _board.SquareAt(Move.TargetCoordinate);
            Piece piece;
            switch (Move.PromotePieceType)
            {
                case Type.Bishop:
                    piece = new Bishop(Move.PieceColor, square);
                    break;
                case Type.King:
                    piece = new King(Move.PieceColor, square);
                    break;
                case Type.Queen:
                    piece = new Queen(Move.PieceColor, square);
                    break;
                case Type.Pawn:
                    piece = new Pawn(Move.PieceColor, square);
                    break;
                case Type.Knight:
                    piece = new Knight(Move.PieceColor, square);
                    break;
                case Type.Rook:
                    piece = new Rook(Move.PieceColor, square);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            square.Piece = piece;
        }

        public void Compensate() // Hoàn tác
        {
            _board.SquareAt(Move.TargetCoordinate).Piece = _oldPawn;
            _moveCommand.Compensate();
        }

        public bool TakePiece => _moveCommand.TakePiece; 

        public Move Move { get; }

        public Type PieceType => Move.PieceType;

        public Color PieceColor => Move.PieceColor;

        public ICompensableCommand Copy(Board board) => new PromoteCommand(this, board);

        public override string ToString() =>
            "Promotion  " + Move.PromotePieceType + " to " + Move.TargetCoordinate;
    }
}