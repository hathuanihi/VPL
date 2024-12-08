using System.Collections.Generic;
using System.Linq;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.Model.AModel;

namespace CHESSGAME.ViewModel.Engine.Rules
{
    internal class CanOnlyTakeEnemyRuleKing : IRule
    {
        public bool IsMoveValid(Move move, Board board)
        {
            if (move.PieceColor == board.PieceAt(move.TargetCoordinate)?.Color)
                return board.PieceAt(move.TargetCoordinate).Type == Type.Rook;

            return true;
        }

        public List<Square> PossibleMoves(Piece piece)
        {
            return piece.Square.Board.Squares.OfType<Square>().ToList().FindAll(
                x => IsMoveValid(new Move(piece, x), piece.Square.Board));
        }
    }
}