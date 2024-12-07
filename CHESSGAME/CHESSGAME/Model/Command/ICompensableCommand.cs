using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;

namespace CHESSGAME.Model.Command
{
    public interface ICompensableCommand
    {
        bool TakePiece { get; } // Trả về true nếu bắt quân cờ đối thủ
        Move Move { get; }
        Type PieceType { get; }
        Color PieceColor { get; }
        void Execute(); // Thực thi command
        void Compensate(); // Undo
        ICompensableCommand Copy(Board board);
        string ToString();
    }
}