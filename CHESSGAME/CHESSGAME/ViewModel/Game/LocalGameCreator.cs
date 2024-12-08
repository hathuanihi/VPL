using CHESSGAME.View.ModelView;
using CHESSGAME.ViewModel.Core;
using CHESSGAME.ViewModel.Engine;
using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;

namespace CHESSGAME.ViewModel.Game
{
    public class LocalGameCreator : GameCreator
    {
        public override Mode Mode => Mode.Local;

        public override Core.Game CreateGame(Container container, BoardView boardView, Color color, GameCreatorParameters parameters)
        {
            IEngine engine = new RealEngine(container);
            PlayerController whitePlayerController = new BoardViewPlayerController(boardView);
            PlayerController blackPlayerController = new BoardViewPlayerController(boardView);
            Player whitePlayer = new Player(Color.White, whitePlayerController);
            Player blackPlayer = new Player(Color.Black, blackPlayerController);

            Core.Game game = new Core.Game(engine, whitePlayer, blackPlayer, container, true);

            whitePlayer.Game = game;
            blackPlayer.Game = game;

            whitePlayerController.Player = whitePlayer;
            blackPlayerController.Player = blackPlayer;

            boardView.BoardViewPlayerControllers.Add((BoardViewPlayerController)whitePlayerController);
            boardView.BoardViewPlayerControllers.Add((BoardViewPlayerController)blackPlayerController);

            return game;
        }
    }
}