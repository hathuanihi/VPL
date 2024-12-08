using CHESSGAME.ViewModel.IA;
using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.View.ModelView;
using CHESSGAME.ViewModel.Core;
using CHESSGAME.ViewModel.Engine;

namespace CHESSGAME.ViewModel.Game
{
    public class AiGameCreator : GameCreator
    {
        public override Mode Mode => Mode.AI;

        public override Core.Game CreateGame(Container container, BoardView boardView, Color color, GameCreatorParameters parameters)
        {
            IEngine engine = new RealEngine(container);
            PlayerController whitePlayerController = new BoardViewPlayerController(boardView);
            PlayerController blackPlayerController = new UciProcessController(container, parameters.AiSkillLevel);
            Player whitePlayer = new Player(Color.White, whitePlayerController);
            Player blackPlayer = new Player(Color.Black, blackPlayerController);

            Core.Game game = new Core.Game(engine, whitePlayer, blackPlayer, container, true);

            whitePlayer.Game = game;
            blackPlayer.Game = game;

            whitePlayerController.Player = whitePlayer;
            blackPlayerController.Player = blackPlayer;

            boardView.BoardViewPlayerControllers.Add((BoardViewPlayerController)whitePlayerController);
            return game;
        }
    }
}