using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.View.ModelView;

namespace CHESSGAME.ViewModel.Game
{
    public abstract class GameCreator
    {
        public abstract Mode Mode { get; }
        public abstract Core.Game CreateGame(Container container, BoardView boardView, Color color, GameCreatorParameters parameters);
    }

}