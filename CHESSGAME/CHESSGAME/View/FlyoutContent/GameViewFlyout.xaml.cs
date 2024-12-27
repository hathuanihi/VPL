using System;
using System.IO;
using System.Reflection;
using System.Windows;
using CHESSGAME.Model.IO;
using Microsoft.Win32;
using CHESSGAME.Files;

namespace CHESSGAME.View.FlyoutContent
{
    /// <summary>
    ///     Interaction logic for GameViewFlyout.xaml
    /// </summary>
    public partial class GameViewFlyout
    {
        private View.Window.GameView _gameView;

        public GameViewFlyout(View.Window.GameView gameView)
        {
            InitializeComponent();
            _gameView = gameView;
        }

        /// <summary>
        ///     Action perform when click on the save tile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TileSave_OnClick(object sender, RoutedEventArgs e)
        {
            Sound.PlayClickSound();
            ISaver saver = new BinarySaver();
            string directorySaveName = "Save";
            string fullSavePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" +
                                  directorySaveName;
            Console.WriteLine(fullSavePath);
            if (Directory.Exists(fullSavePath) == false) Directory.CreateDirectory(fullSavePath);
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = saver.Filter(),
                InitialDirectory = fullSavePath
            };
            if (saveFileDialog.ShowDialog() == true) saver.Save(_gameView.Game.Container, saveFileDialog.FileName);
        }

        private async void TileQuit_OnClick(object sender, RoutedEventArgs e)
        {
            Sound.PlayClickSound();
            await _gameView.Quit();
        }
    }
}