using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using CHESSGAME.Model.IO;
using CHESSGAME.Model.AModel;
using Microsoft.Win32;

namespace CHESSGAME.View.Window
{
    /// <summary>
    ///     Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        private MainWindow _mainWindow;

        public Home(MainWindow mw)
        {
            InitializeComponent();
            _mainWindow = mw;
        }

        private void CreateNewGameButton_OnClick(object sender, RoutedEventArgs e) // Xử lý sự kiện click vào nút tạo ván game mới
        {
            _mainWindow.MainControl.Content = new GameModeSelection(new Container(), _mainWindow);
        }

        private void UseSaveButton_OnClick(object sender, RoutedEventArgs e) // Xử lý sự kiện click vào nút tải ván game cũ
        {
            ILoader loader = new BinaryLoader();

            const string directorySaveName = "Save";
            string fullSavePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" +
                                  directorySaveName;

            if (!Directory.Exists(fullSavePath))
                Directory.CreateDirectory(fullSavePath);

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = loader.Filter(),
                InitialDirectory = fullSavePath
            };

            if (openFileDialog.ShowDialog() != true) return;

            Container container = loader.Load(openFileDialog.FileName);

            _mainWindow.MainControl.Content = new GameModeSelection(container, _mainWindow);
        }

    }
}