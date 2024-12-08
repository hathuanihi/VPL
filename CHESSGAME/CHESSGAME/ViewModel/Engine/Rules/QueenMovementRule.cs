using System.Collections.Generic;
using System.Linq;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.Model.AModel;

namespace CHESSGAME.ViewModel.Engine.Rules
{
    public class QueenMovementRule : IRule
    {
        private BishopMovementRule _bishopRule = new BishopMovementRule();
        private RookMovementRule _rookRule = new RookMovementRule();

        public bool IsMoveValid(Move move, Board board) =>
            _bishopRule.IsMoveValid(move, board) || _rookRule.IsMoveValid(move, board);

        public List<Square> PossibleMoves(Piece piece)
        {
            return _bishopRule.PossibleMoves(piece).Concat(_rookRule.PossibleMoves(piece)).ToList();
        }
    }
}