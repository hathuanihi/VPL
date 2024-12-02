using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using CHESSGAME.Model.IO;
using Container = CHESSGAME.Model.AModel.Container;
using GameModeSelection = CHESSGAME.View.Window.GameModeSelection;
using Home = CHESSGAME.View.Window.Home;

namespace CHESSGAME
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            ItemSource = ThemeManager.Accents.Select(
                        a => new AccentColorMenuData
                             {
                                Name = a.Name,
                                ColorBrush = a.Resources["AccentColorBrush"] as Brush
                             }).ToList();
        }

        public List<AccentColorMenuData> ItemSource { get; set; } // Danh sách chứa các màu sắc chủ đề để sử dụng trong giao diện người dùng


        private void MenuItemQuit_OnClick(object sender, RoutedEventArgs e) // Xử lý sự kiện Quit
        {
            Application.Current.Shutdown();
        }

        protected override void OnClosing(CancelEventArgs e) // Phương thức được gọi khi cửa sổ chính đang đóng
        {
            base.OnClosing(e);

            if (File.Exists("log.temp"))
                File.Delete("log.temp");
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e) // Xử lý sự kiện khi tải cửa sổ
        {
            if (Environment.GetCommandLineArgs().Length != 1)
            {
                ILoader loader = new BinaryLoader();
                Container container = null;

                try
                {
                    container = loader.Load(Environment.GetCommandLineArgs()[1]);
                }
                catch (Exception)
                {
                    this.ShowMessageAsync("Impossible to read selected file.", Environment.GetCommandLineArgs()[1]);
                }

                if (container != null)
                    MainControl.Content = new GameModeSelection(container, this);
                else
                    MainControl.Content = new Home(this);
            }
            else
                MainControl.Content = new Home(this);
        }
    }
}

public class AccentColorMenuData // Lớp này đại diện cho một màu sắc chủ đề, chứa tên, màu sắc viền và màu sắc chính
{
    private ICommand _changeAccentCommand;
    public string Name { get; set; }
    public Brush BorderColorBrush { get; set; }
    public Brush ColorBrush { get; set; }

    public ICommand ChangeAccentCommand
    {
        get
        {
            return _changeAccentCommand ??
                   (_changeAccentCommand =
                       new SimpleCommand { CanExecuteDelegate = x => true, ExecuteDelegate = x => DoChangeTheme(x) });
        }
    }

    protected virtual void DoChangeTheme(object sender)
    {
        var theme = ThemeManager.DetectAppStyle(Application.Current);
        var accent = ThemeManager.GetAccent(Name);
        ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
    }
}

public class SimpleCommand : ICommand
{
    public Predicate<object> CanExecuteDelegate { get; set; }
    public Action<object> ExecuteDelegate { get; set; }

    public bool CanExecute(object parameter)
    {
        if (CanExecuteDelegate != null)
            return CanExecuteDelegate(parameter);
        return true;
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object parameter)
    {
        ExecuteDelegate?.Invoke(parameter);
    }
}