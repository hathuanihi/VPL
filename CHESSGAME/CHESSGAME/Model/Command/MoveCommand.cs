using System;
using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;
using Type = CHESSGAME.Model.AModel.Pieces.Type;

namespace CHESSGAME.Model.Command
{
    [Serializable]
    public class MoveCommand : ICompensableCommand
    {
        private Board _board;

        private bool _hasChangedState;
        private Piece _piece;
        private Piece _removedPiece;
        private Square _startSquare;
        private Square _targetSquare;
        public MoveCommand(Move move, Board board)
        {
            Move = move;
            _board = board;

            TakePiece = board.PieceAt(Move.TargetCoordinate) != null;
        }

        private MoveCommand(MoveCommand command, Board board)
        {
            _board = board;
            Move = command.Move;

            TakePiece = board.PieceAt(Move.TargetCoordinate) != null;
        }

        public void Execute()
        {
            _targetSquare = _board.SquareAt(Move.TargetCoordinate);
            _startSquare = _board.SquareAt(Move.StartCoordinate);
            _piece = _startSquare.Piece;

            // Đánh dấu trạng thái đã di chuyển
            if (!_piece.HasMoved)
            {
                _piece.HasMoved = true;
                _hasChangedState = true;
            }

            // Đánh dấu ô đang trống 
            if (_targetSquare.Piece == null)
            {
                _startSquare.Piece = null;
                _piece.Square = _targetSquare;
                _targetSquare.Piece = _piece;
            }
            else
            {
                _removedPiece = _targetSquare.Piece;
                _targetSquare.Piece = null;
                _piece.Square.Piece = null;
                _piece.Square = _targetSquare;
                _targetSquare.Piece = _piece;
            }
        }
        public void Compensate()
        {
            if (_hasChangedState) _piece.HasMoved = false;

            _targetSquare.Piece = _removedPiece;
            _startSquare.Piece = _piece;
            _piece.Square = _startSquare;
        }

        public bool TakePiece { get; }

        public Move Move { get; }

        public Type PieceType => Move.PieceType;

        public Color PieceColor => Move.PieceColor;

        public ICompensableCommand Copy(Board board) => new MoveCommand(this, board);

        public override string ToString() => _piece + " from " + Move.StartCoordinate + " to " + Move.TargetCoordinate;
    }
}