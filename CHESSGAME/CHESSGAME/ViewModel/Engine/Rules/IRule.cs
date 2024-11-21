using System.Collections.Generic;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.Model.AModel;

namespace CHESSGAME.ViewModel.Engine.Rules
{
    public interface IRule
    {
        /// <summary>
        ///     Check if a move is correct against a rule
        /// </summary>
        /// <param name="move">Move to check</param>
        /// <param name="board">Board to apply the move on</param>
        /// <returns>False if the move is invalidated by this rule</returns>
        bool IsMoveValid(Move move, Board board);

        /// <summary>
        ///     Retrieves all boxes that match the rule for the given part
        /// </summary>
        /// <param name="piece">Part that performs the movement</param>
        /// <returns>List of boxes for which the rule is checked </returns>
        List<Square> PossibleMoves(Piece piece);
    }
}