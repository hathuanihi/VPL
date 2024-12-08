using System.Collections.Generic;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.Model.AModel;

namespace CHESSGAME.ViewModel.Engine.Rules
{
    public interface IRule
    {
        bool IsMoveValid(Move move, Board board); // Trả về true nếu nước đi hợp lệ
        List<Square> PossibleMoves(Piece piece); // Danh sách các nước đi hợp lệ 
    }
}