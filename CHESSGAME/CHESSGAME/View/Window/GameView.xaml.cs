using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using CHESSGAME.ViewModel.Engine;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.View.ModelView;
using CHESSGAME.View.FlyoutContent;

namespace CHESSGAME.View.Window
{
    /// <summary>
    ///     Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView
    {
        private BoardView _boardView;
        private MainWindow _mainWindow;

        public GameView(MainWindow mainWindow, ViewModel.Core.Game game, BoardView boardView)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _boardView = boardView;
            Game = game;

            game.StateChanged += _boardView.GameStateChanged;

            game.StateChanged += state =>
            {
                switch (state)
                {
                    case BoardState.BlackCheckMate:
                        _mainWindow.ShowMessageAsync("End of the game", "The black player is checkmate.",
                            MessageDialogStyle.AffirmativeAndNegative);
                        break;
                    case BoardState.WhiteCheckMate:
                        _mainWindow.ShowMessageAsync("End of the game", "The white player is checkmate.",
                            MessageDialogStyle.AffirmativeAndNegative);
                        break;
                    case BoardState.BlackPat:
                        _mainWindow.ShowMessageAsync("Draw", "The black player is stale.",
                            MessageDialogStyle.AffirmativeAndNegative);
                        break;
                    case BoardState.WhitePat:
                        _mainWindow.ShowMessageAsync("Draw", "The white player is stale.",
                            MessageDialogStyle.AffirmativeAndNegative);
                        break;
                }
            };

            GameViewFlyout gameViewFlyout = new GameViewFlyout(this);
            _mainWindow.Flyout.Content = gameViewFlyout.Content;
            UcBoardView.Content = boardView;

            game.Container.MoveDone += move =>
            {
                LabelPlayerTurn.Content = move.PieceColor == Color.Black ? "White" : "Black";
            };

            game.Container.MoveUndone += move =>
            {
                LabelPlayerTurn.Content = move.PieceColor == Color.Black ? "White" : "Black";
            };

        }

        public ViewModel.Core.Game Game { get; set; }

        private void ButtonUndo_OnClick(object sender, RoutedEventArgs e) => Game.Undo();
        private void ButtonRedo_OnClick(object sender, RoutedEventArgs e) => Game.Redo();

        #region Flyout

        private void Grid_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_mainWindow.Flyout.IsOpen) return;
            _mainWindow.Flyout.IsOpen = false;
        }

        private void ButtonMenu_OnClick(object sender, RoutedEventArgs e)
        {
            if (_mainWindow.Flyout.IsOpen) return;
            _mainWindow.Flyout.IsOpen = true;
        }

        public async Task Quit()
        {
            _mainWindow.Flyout.IsOpen = false;

            var result =
                await
                    _mainWindow.ShowMessageAsync("Quit the game",
                        "Are you sure you want to leave the game? If your game is not saved, it will be lost...",
                        MessageDialogStyle.AffirmativeAndNegative);

            if (result == MessageDialogResult.Affirmative)
            {
                _mainWindow.Flyout.Content = null;
                _mainWindow.MainControl.Content = new Home(_mainWindow);
            }
        }

        #endregion
    }
}