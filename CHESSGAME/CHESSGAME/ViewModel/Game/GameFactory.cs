using System.Collections.Generic;
using System.Linq;
using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.View.ModelView;

namespace CHESSGAME.ViewModel.Game
{
    public class GameFactory
    {
        public List<GameCreator> GameCreators = new List<GameCreator>();

        public GameFactory()
        {
            GameCreators.Add(new LocalGameCreator());
            GameCreators.Add(new AiGameCreator());
        }

        /// <summary>
        /// Returns a game instance in the game mode passed as a parameter
        /// </summary>
        /// <param name="mode">Desired game mode</param>
        /// <returns>A part in the game mode passed as a parameter</returns>
        public Core.Game CreateGame(Mode mode, Container container, BoardView boardView, Color color, GameCreatorParameters parameters)
        {
            return GameCreators.FindAll(x => x.Mode == mode).First().CreateGame(container, boardView, color, parameters);
        }
    }


    /// <summary>
    /// Define the different possible game modes
    /// </summary>
    public enum Mode
    {
        Local,
        AI
    }
}