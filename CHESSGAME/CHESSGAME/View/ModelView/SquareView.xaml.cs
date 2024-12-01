using System.ComponentModel;
using System.Windows.Controls;
using CHESSGAME.Model.AModel;

namespace CHESSGAME.View.ModelView
{
    /// <summary>
    ///     Interaction logic for SquareView.xaml
    /// </summary>
    public partial class SquareView : UserControl // Lớp đại diện cho ô cờ trong bàn cờ
    {
        public SquareView(Square square) 
        {
            InitializeComponent();
            Square = square;
            DataContext = this;
            Square.PropertyChanged += SquarePropertyChangeHandler; // Sự kiện theo dõi các thay đổi trong thuộc tính của Square

            // Nếu ô chứa quân cờ
            if (square.Piece != null)
                PieceView = new PieceView(square.Piece);

            // Thiết lập màu nền cho ô (đen/trắng)
            SetResourceReference(BackgroundProperty, (square.X + square.Y) % 2 == 0 ? "AccentColorBrush" : "AccentColorBrush4");

            // Đặt vị trí của ô trong grid dựa trên tọa độ X, Y
            Grid.SetColumn(this, square.X);
            Grid.SetRow(this, square.Y);
        }

        public PieceView PieceView
        {
            get { return UcPieceView.Content as PieceView; }
            set { UcPieceView.Content = value; }
        }

        public Square Square { get; set; }

        private void SquarePropertyChangeHandler(object sender, PropertyChangedEventArgs e)
        {
            PieceView = Square.Piece != null ? new PieceView(Square.Piece) : null;
            UcPieceView.Content = PieceView;
        }

        public override string ToString() => Square.ToString();
    }
}