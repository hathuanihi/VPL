using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CHESSGAME.Model.AModel;
using CHESSGAME.ViewModel.Engine;
using CHESSGAME.ViewModel.Game;
using CHESSGAME.Model.Command;
using Color = CHESSGAME.Model.AModel.Pieces.Color;
using Type = CHESSGAME.Model.AModel.Pieces.Type;
using System.Security.Cryptography;

namespace CHESSGAME.View.ModelView
{
    /// <summary>
    ///     Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : UserControl // Lớp đại diện cho bàn cờ
    {
        #region Fields lưu trữ trạng thái của bàn cờ và tương tác của người dùng

        private SquareView _lastChangedSquareView;
        private List<SquareView> _possibleMoves = new List<SquareView>();
        private List<SquareView> _lastMove = new List<SquareView>();
        private SquareView _clickedSquare;
        private PieceView _selectedPiece;
        private bool _mouseDown;
        private bool _selected;
        private Point _mouseDownPoint;
        private bool _initDragAndDropOnMouseMove;
        private bool _hasBeginDragAndDrop;
        private Container _container;

        #endregion

        #region Constructor

        public BoardView(Container container)
        {
            InitializeComponent();
            Board = container.Board;
            _container = container;

            // Các hàng, cột được thêm vào grid để khởi tạo bàn cờ
            for (int i = 0; i < Board.Size; i++)
            {
                Grid.RowDefinitions.Add(new RowDefinition());
                Grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            //Tạo các ô bàn cờ (SquareView), thêm vào grid
            foreach (var square in Board.Squares)
            {
                var squareView = new SquareView(square)
                {
                    UcPieceView = { LayoutTransform = LayoutTransform },
                    LayoutTransform = LayoutTransform
                };
                SquareViews.Add(squareView);
                Grid.Children.Add(squareView); 
            }

            // Tạo nhãn A-H 
            for (int i = 0; i < Board.Size; i++)
            {
                Label label = new Label
                {
                    Content = (char)('A' + i),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                Grid.SetColumn(label, i);
                Grid.SetRow(label, 8);
                Grid.Children.Add(label);
            }

            // Tạo nhãn 1-8
            for (int i = Board.Size; i > 0; i--)
            {
                Label label = new Label
                {
                    Content = Board.Size - i + 1,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(label, 8);
                Grid.SetRow(label, i - 1);
                Grid.Children.Add(label);
            }

            // Theo dõi các thay đổi trong danh sách di chuyển, cập nhật màu sắc của ô cờ tương ứng khi có di chuyển mới
            _container.Moves.CollectionChanged += (sender, args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add ||
                    args.Action == NotifyCollectionChangedAction.Remove)
                {
                    if (_container.Moves.Count != 0)
                    {
                        //Console.WriteLine("Changed color");
                        _lastMove.ForEach(ResetSquareViewColor);
                        _lastMove.Clear();

                        ICompensableCommand command = _container.Moves.Last();
                        SquareView startSquare = SquareAt(command.Move.StartCoordinate);
                        SquareView targetSquare = SquareAt(command.Move.TargetCoordinate);

                        _lastMove.Add(targetSquare);

                        _lastMove.Add(startSquare);
                    }
                }
            };
        }

        #endregion

        #region Properties

        public List<SquareView> SquareViews { get; } = new List<SquareView>(); // Danh sách các ô cờ 
        public Board Board { get; set; }

        // Danh sách các điều khiển của người chơi cho bàn cờ
        public List<BoardViewPlayerController> BoardViewPlayerControllers { get; set; } = new List<BoardViewPlayerController>(); 

        #endregion

        #region EventHandling

        protected override void OnMouseDown(MouseButtonEventArgs e) // Xử lý sự kiện khi nhấn chuột trái.
        {
            base.OnMouseDown(e);
            if (e.ChangedButton != MouseButton.Left) return;
            _mouseDown = true;
            _mouseDownPoint = e.GetPosition(Grid);
            _clickedSquare = SquareAt(e.GetPosition(Grid));

            if (_clickedSquare == null) return;

            if (_selected && _possibleMoves.Contains(_clickedSquare)) return;

            ResetBoardColor();
            _selectedPiece = _clickedSquare.PieceView;

            // Controllers được sử dụng để lấy danh sách nước đi hợp lệ cho quân cờ được chọn 
            List<BoardViewPlayerController> concernedControllers =
                BoardViewPlayerControllers.FindAll(x => (x.Player.Color == (_selectedPiece?.Piece.Color) && x.IsPlayable));

            if (concernedControllers.Count == 0) return;
            _initDragAndDropOnMouseMove = true;

            // Vẽ các nước đi hợp lệ
            foreach (Square square in concernedControllers.First().PossibleMoves(_selectedPiece.Piece))
            {
                SquareView squareView = Grid.Children.Cast<SquareView>().First(x => Grid.GetRow(x) == square.Y && Grid.GetColumn(x) == square.X);

                squareView.SetResourceReference(BackgroundProperty,(square.X + square.Y) % 2 == 0
                        ? "CleanWindowCloseButtonBackgroundBrush"
                        : "CleanWindowCloseButtonPressedBackgroundBrush");

                _possibleMoves.Add(squareView);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e) // Xử lý sự kiện nhả chuột
        {
            base.OnMouseUp(e);
            if (e.ChangedButton != MouseButton.Left) return;
            _mouseDown = false;
            _initDragAndDropOnMouseMove = false;

            var concernedControllers = ConcernedControllers();

            if (concernedControllers.Count == 0) return;

            Move move = null;
            SquareView squareView = SquareAt(e.GetPosition(Grid));

            //Nếu ô được nhấp chuột không hợp lệ
            if (squareView == null)
            {
                if (_selectedPiece == null) return;
                Canvas.Children.Remove(_selectedPiece);
                _clickedSquare.Square.Piece = _selectedPiece.Piece;
                _selectedPiece = null;
                _selected = false;
                return;
            }
            SquareView clickedSquareView = SquareAt(_mouseDownPoint);

            // Kiểm tra xem người chơi có nhấp vào cùng một ô hay không
            bool select = Equals(squareView.Square.Coordinate, clickedSquareView?.Square?.Coordinate); 

            if (select)
            {
                if (_hasBeginDragAndDrop) // Kéo thả
                {
                    Canvas.Children.Remove(_selectedPiece);
                    _clickedSquare.PieceView = _selectedPiece;
                    _selectedPiece = null;
                    _hasBeginDragAndDrop = false;
                    ResetBoardColor();
                    return;
                }
                //First click
                if (!_selected)
                {
                    //Console.WriteLine("Selection");
                    _selected = true;
                }

                //Second click
                else
                {
                    if (!_possibleMoves.Contains(_clickedSquare)) return;

                    _selected = false;
                    ResetBoardColor();

                    // Nếu là quân Tốt và đang đặt ở hàng cuối cùng
                    if ((_selectedPiece.Piece.Type == Type.Pawn) &&
                    (squareView.Square.Y == (_selectedPiece.Piece.Color == Color.White ? 0 : 7)))
                    {
                        var promoteDialog = new View.Window.Feature.PieceTypeSelectionWindow(_selectedPiece.Piece.Color);
                        promoteDialog.ShowDialog(); // Hiển thị cửa sổ thăng cấp

                        move = new Move(_selectedPiece.Piece.Square, squareView.Square, _selectedPiece.Piece.Type,
                            _selectedPiece.Piece.Color, promoteDialog.ChosenType);
                    }
                    else
                    {
                        move = new Move(_selectedPiece.Piece, squareView.Square);
                    }
                    concernedControllers.ForEach(x => x.Move(move));
                }
            }
            else //Kéo thả đến ô hợp lệ
            {
                if (!_possibleMoves.Contains(squareView))
                {
                    ResetDragAndDrop();
                    ResetBoardColor();
                    return;
                }
                Canvas.Children.Remove(_selectedPiece);
                ResetBoardColor();

                if ((_selectedPiece.Piece.Type == Type.Pawn) && (squareView.Square.Y == (_selectedPiece.Piece.Color == Color.White ? 0 : 7)))
                {
                    var promoteDialog = new View.Window.Feature.PieceTypeSelectionWindow(_selectedPiece.Piece.Color);
                    promoteDialog.ShowDialog();
                    move = new Move(_selectedPiece.Piece.Square, squareView.Square, _selectedPiece.Piece.Type,
                        _selectedPiece.Piece.Color, promoteDialog.ChosenType);
                }
                else
                {
                    move = new Move(_selectedPiece.Piece, squareView.Square);
                }
                concernedControllers.ForEach(x => x.Move(move));
                _selected = false;
                _hasBeginDragAndDrop = false;
            }
        }
        protected override void OnMouseMove(MouseEventArgs e) // Xử lý sự kiện di chuyển chuột
        {
            base.OnMouseMove(e);

            Point p = e.GetPosition(Grid); // Lấy vị trí chuột trong hệ tọa độ của grid

            if (p.X < 0 || p.Y < 0 || p.X > Grid.ActualWidth || p.Y > Grid.ActualHeight)
            {
                if (_selectedPiece == null) return;
                Canvas.Children.Remove(_selectedPiece);
                _clickedSquare.Square.Piece = _selectedPiece.Piece;
                _selectedPiece = null;
                _selected = false;
                return;
            }

            // Kiểm tra các điều kiện kéo thả
            if (!_mouseDown) return;
            if (_selectedPiece == null) return;

            // Quá trình kéo thả
            if (_initDragAndDropOnMouseMove && (_mouseDownPoint - e.GetPosition(Grid)).Length > 5)
            {
                _clickedSquare.PieceView = null;

                var width = _selectedPiece.ActualWidth;
                var height = _selectedPiece.ActualHeight;

                // Thêm quân cờ vào Canvas
                Canvas.Children.Add(_selectedPiece);

                _selectedPiece.Height = height;
                _selectedPiece.Width = width;

                // Đánh dấu quá trình kéo thả đã bắt đầu
                _initDragAndDropOnMouseMove = false;
                _hasBeginDragAndDrop = true;
            }

            // Điều chỉnh lại vị trí quân cờ theo tọa độ chuột
            Canvas.SetTop(_selectedPiece, e.GetPosition(this).Y - _selectedPiece.ActualHeight / 2);
            Canvas.SetLeft(_selectedPiece, e.GetPosition(this).X - _selectedPiece.ActualWidth / 2);
        }

        #endregion

        #region Coloration

        private void ResetBoardColor() // Đặt lại màu sắc cho bàn cờ về trạng thái cơ bản
        {
            _possibleMoves.ForEach(ResetSquareViewColor);
            _lastMove.ForEach(x => x.SetResourceReference(BackgroundProperty, (x.Square.X + x.Square.Y) % 2 == 0
                    ? "CheckBoxBrush"
                    : "CheckBoxMouseOverBrush"));
            _possibleMoves.Clear();
        }

        private static void ResetSquareViewColor(SquareView squareView) // Đặt lại màu sắc cho ô cờ
        {
            squareView.SetResourceReference(BackgroundProperty,
                (squareView.Square.X + squareView.Square.Y) % 2 == 0 ? "AccentColorBrush" : "AccentColorBrush4");
        }

        public void GameStateChanged(BoardState state) // Thay đổi trạng thái trò chơi khi có sự kiện xảy ra
        {
            SquareView squareView = null;

            ResetBoardColor();

            switch (state)
            {
                case BoardState.Normal: // Trạng thái bình thường, khôi phục màu sắc gốc
                    if (_lastChangedSquareView != null)
                        ResetSquareViewColor(_lastChangedSquareView);
                    break;

                case BoardState.WhiteCheck: // Vua trắng bị chiếu
                    squareView = SquareViews.First(
                            x => (x.Square?.Piece?.Type == Type.King) && (x.Square?.Piece?.Color == Color.White));
                    squareView.SetResourceReference(BackgroundProperty, "ValidationBrush5");
                    break;

                case BoardState.BlackCheck: // Vua đen bị chiếu
                    squareView = SquareViews.First(
                            x => (x.Square?.Piece?.Type == Type.King) && (x.Square?.Piece?.Color == Color.Black));
                    squareView.SetResourceReference(BackgroundProperty, "ValidationBrush5");
                    break;

                case BoardState.BlackCheckMate: // Vua đen bị chiếu hết
                    squareView = SquareViews.First(
                            x => (x.Square?.Piece?.Type == Type.King) && (x.Square?.Piece?.Color == Color.Black));
                    squareView.SetResourceReference(BackgroundProperty, "TextBrush");
                    break;

                case BoardState.WhiteCheckMate: // Vua trắng bị chiếu hết
                    squareView = SquareViews.First(
                            x => (x.Square?.Piece?.Type == Type.King) && (x.Square?.Piece?.Color == Color.White));
                    squareView.SetResourceReference(BackgroundProperty, "TextBrush");
                    break;

                case BoardState.BlackPat: // Hòa cờ khi vua đen không thể di chuyển && không bị chiếu
                    squareView = SquareViews.First(
                            x => (x.Square?.Piece?.Type == Type.King) && (x.Square?.Piece?.Color == Color.Black));
                    squareView.SetResourceReference(BackgroundProperty, "WhiteColorBrush");
                    break;

                case BoardState.WhitePat: // Hòa cờ khi vua trắng không thể di chuyển && không bị chiếu
                    squareView = SquareViews.First(
                            x => (x.Square?.Piece?.Type == Type.King) && (x.Square?.Piece?.Color == Color.White));
                    squareView.SetResourceReference(BackgroundProperty, "WhiteColorBrush");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
            _lastChangedSquareView = squareView; // Lưu lại các ô đã được thay đổi
        }

        #endregion

        #region Utilities

        private void ResetDragAndDrop() // Đặt lại trạng thái kéo thả
        {
            if (_selectedPiece == null) return;
            Canvas.Children.Remove(_selectedPiece);
            _clickedSquare.PieceView = _selectedPiece;
            _selectedPiece = null;
            _hasBeginDragAndDrop = false;
        }

        private SquareView SquareAt(Point point) // Xác định các ô cờ dựa vào tọa độ
        {
            var row = 0;
            var col = 0;
            var accumulatedHeight = 0.0; // Tổng chiều cao
            var accumulatedWidth = 0.0; // Tổng chiều rộng

            foreach (var rowDefinition in Grid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            foreach (var columnDefinition in Grid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }

            var clickedControl = Grid.Children.OfType<UIElement>().First(x => (Grid.GetRow(x) == row) && (Grid.GetColumn(x) == col));

            return clickedControl as SquareView;
        }

        private SquareView SquareAt(Coordinate coordinate) => // Xác định ô cờ dựa vào tọa độ logic trên bàn cờ
            Grid.Children.Cast<UIElement>()
                .FirstOrDefault(e => Grid.GetColumn(e) == coordinate.X && Grid.GetRow(e) == coordinate.Y) as SquareView;

        private List<BoardViewPlayerController> ConcernedControllers() => // Xác định controller phù hợp với quân cờ được chọn 
                BoardViewPlayerControllers.FindAll(x => (x.Player.Color == (_selectedPiece?.Piece.Color) && x.IsPlayable));

        #endregion

        #region Miscellaneous

        // Đăng ký thuộc tính phụ thuộc cho Borderbrush, cho phép tùy chỉnh màu viền của SquareView
        public static readonly DependencyProperty SetTextProperty = DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(SquareView));
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo) // Cập nhật kích thước ô grid khi SquareView bị thay đổi
        {
            base.OnRenderSizeChanged(sizeInfo);
            var minNewSizeOfParentUserControl = Math.Min(sizeInfo.NewSize.Height, sizeInfo.NewSize.Width);
            Grid.Width = minNewSizeOfParentUserControl;
            Grid.Height = minNewSizeOfParentUserControl;
        }
        #endregion
    }
}
