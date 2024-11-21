using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.View.ModelView;

namespace CHESSGAME.ViewModel.Game
{
    public abstract class GameCreator
    {
        public abstract Mode Mode { get; }

        /// <summary>
        /// Asks the part creator to return a working part with the desired parameters
        /// </summary>
        /// <param name="container"></param>
        /// <param name="boardView"></param>
        /// <param name="color">La couleur</param>
        /// <returns></returns>
        public abstract Core.Game CreateGame(Container container, BoardView boardView, Color color, GameCreatorParameters parameters);
    }

}