using System.Windows;
using CHESSGAME.ViewModel.Game;
using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.View.ModelView;

namespace CHESSGAME.View.Window
{
    /// <summary>
    /// Interaction logic for GameModeSelection.xaml
    /// </summary>
    public partial class GameModeSelection
    {
        private MainWindow _mainWindow;
        private Container _container;

        public GameModeSelection(Container container, MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _container = container;
        }

        private void TileAiPlay_OnClick(object sender, RoutedEventArgs e) // Xử lý sự kiện chơi với máy
        {
            _mainWindow.MainControl.Content = new AiOptionSelection(_mainWindow, _container);
        }

        private void LocalGameButton_OnClick(object sender, RoutedEventArgs e) // Xử lý sự kiện chơi hai người
        {
            GameFactory gameFactory = new GameFactory();
            BoardView boardView = new BoardView(_container);
            ViewModel.Core.Game game = gameFactory.CreateGame(Mode.Local, _container, boardView, Color.White, null);

            _mainWindow.MainControl.Content = new GameView(_mainWindow, game, boardView);
        }
    }
}