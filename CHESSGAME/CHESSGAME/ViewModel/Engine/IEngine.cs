using System.Collections.Generic;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.Model.AModel;

namespace CHESSGAME.ViewModel.Engine
{
    public interface IEngine
    {
        bool DoMove(Move move); // Trả về true nếu nước đi hợp lệ
        List<Square> PossibleMoves(Piece piece);
        Move Undo();
        Move Redo();

        BoardState CurrentState();
    }
    public enum BoardState // Các trạng thái
    {
        Normal,
        WhiteCheck,
        BlackCheck,
        BlackCheckMate,
        WhiteCheckMate,
        BlackPat,
        WhitePat
    }
}