using System.Collections.Generic;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.Model.AModel;

namespace CHESSGAME.ViewModel.Core
{
    public abstract class PlayerControler
    {
        public Player Player { get; set; }

        /// <summary>
        /// Tells the player's controller that it's their turn to play
        /// </summary>
        public abstract void Play(Move move);

        /// <summary>
        /// Gives the player the move to make
        /// </summary>
        /// <param name="move"></param>
        public abstract void Move(Move move);

        /// <summary>
        /// Method called if movement is invalid
        /// </summary>
        /// <param name="reasonsList">List of rules not verified by the movement</param>
        public abstract void InvalidMove(List<string> reasonsList);

        /// <summary>
        /// Asks the player to return the list of possible moves for a given room
        /// </summary>
        /// <param name="piece">Part for which we want to know the possible movements</param>
        /// <returns>List of boxes accessible by the part passed as a parameter</returns>
        public abstract List<Square> PossibleMoves(Piece piece);

        public abstract void Stop();
    }
}