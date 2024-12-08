using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;

namespace CHESSGAME.ViewModel.Engine.States
{
    public interface IState
    {
        bool IsInState(Board board, Color color); // Trả về true nếu bàn cờ đang ở trạng thái được định nghĩa
        string Explain(); // Trạng thái mà lớp đang triển khai
    }
}