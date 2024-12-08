using System.Collections.Generic;
using System.Linq;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.Model.AModel;

namespace CHESSGAME.ViewModel.Engine.Rules
{
    public class CanOnlyTakeEnemyRule : IRule
    {
        public bool IsMoveValid(Move move, Board board) =>
            move.PieceColor != board.PieceAt(move.TargetCoordinate)?.Color;

        public List<Square> PossibleMoves(Piece piece)
        {
            return piece.Square.Board.Squares.OfType<Square>().ToList().FindAll(
                x => piece.Color != x?.Piece?.Color);
        }
    }
}