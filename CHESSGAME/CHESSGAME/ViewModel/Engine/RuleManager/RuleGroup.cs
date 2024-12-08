using System;
using System.Collections.Generic;
using System.Linq;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.Model.AModel;
using CHESSGAME.ViewModel.Engine.Rules;
using Type = CHESSGAME.Model.AModel.Pieces.Type;

namespace CHESSGAME.ViewModel.Engine.RuleManager
{
    public abstract class RuleGroup
    {
        public RuleGroup Next { get; internal set; } // Nhóm quy tắc tiếp theo trong chuỗi
        protected List<IRule> Rules { get; set; } = new List<IRule>(); // Danh sách các quy tắc của một quân cờ
        protected abstract Type Type { get; }

        public void AddGroup(RuleGroup ruleGroup) // Thêm một nhóm quy tắc vào cuối chuỗi hiện tại
        {
            if (Next == null)
                Next = ruleGroup;
            else
                Next.AddGroup(ruleGroup);
        }

        public bool Handle(Move move, Board board) // Kiểm tra tính hợp lệ của nước đi
        {
            if (move.PieceType == Type) 
                return Rules.All(rule => rule.IsMoveValid(move, board));

            if (Next != null) // Chuyển sang yêu cầu nhóm quy tắc tiếp theo
                return Next.Handle(move, board);
            throw new Exception("Nobody treats this piece! " + move.PieceType);
        }

        public List<Square> PossibleMoves(Piece piece)
        {
            List<Square> result = new List<Square>();
            if (piece.Type == Type)
            {
                result = result.Concat(Rules.First().PossibleMoves(piece)).ToList();
                Rules.ForEach(x => result = result.Intersect(x.PossibleMoves(piece)).ToList());
                return result;
            }
            if (Next != null) return Next.PossibleMoves(piece);
            throw new Exception("Nobody treats this piece! " + piece);
        }
    }
}