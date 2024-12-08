using System.Collections.Generic;
using System.Linq;
using CHESSGAME.ViewModel.Engine.Rules;
using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;

namespace CHESSGAME.ViewModel.Engine.States
{
    public class CheckState : IState
    {
        public bool IsInState(Board board, Color color) // Kiểm tra xem vua có đang bị chiếu
        {
            Board tempBoard = new Board(board);

            #region Các quy tắc di chuyển của từng quân cờ
            List<IRule> queenMovementCheckRules = new List<IRule> 
            { 
                new QueenMovementRule(), 
                new CanOnlyTakeEnemyRule() 
            };

            List<IRule> pawnMovementCheckRules = new List<IRule> 
            { 
                new PawnMovementRule(), 
                new CanOnlyTakeEnemyRule() 
            };

            List<IRule> kingMovementCheckRules = new List<IRule> 
            { 
                new KingMovementRule(), 
                new CanOnlyTakeEnemyRule(), 
                new CastlingRule() 
            };

            List<IRule> knightMovementCheckRules = new List<IRule> 
            {
                new KnightMovementRule(),
                new CanOnlyTakeEnemyRule()
            };

            List<IRule> rookMovementCheckRules = new List<IRule> 
            { 
                new CanOnlyTakeEnemyRule(), 
                new RookMovementRule() 
            };

            List<IRule> bishopMovementCheckRules = new List<IRule>
            {
                new CanOnlyTakeEnemyRule(),
                new BishopMovementRule()
            };
            #endregion

            Dictionary<Type, List<IRule>> rulesGroup = new Dictionary<Type, List<IRule>>
            {
                {Type.Queen, queenMovementCheckRules},
                {Type.Pawn, pawnMovementCheckRules},
                {Type.Knight, knightMovementCheckRules},
                {Type.Rook, rookMovementCheckRules},
                {Type.Bishop, bishopMovementCheckRules},
                {Type.King, kingMovementCheckRules}
            };


            // Tìm quân vua
            Piece concernedKing = tempBoard.Squares.OfType<Square>().First(
                x => (x?.Piece?.Type == Type.King) && (x?.Piece?.Color == color)).Piece;

            bool result = false; 

            foreach (KeyValuePair<Type, List<IRule>> rules in rulesGroup)
            {
                List<Square> possibleMoves = new List<Square>();
                concernedKing.Type = rules.Key;

                // Tính toán tất cả nước đi có thể của loại quân này theo quy tắc di chuyển
                possibleMoves = possibleMoves.Concat(rules.Value.First().PossibleMoves(concernedKing)).ToList();
                rules.Value.ForEach(
                    x => possibleMoves = possibleMoves.Intersect(x.PossibleMoves(concernedKing)).ToList());

                // Kiểm tra xem có ô vuông nào chứa quân cờ đối phương có thể tấn công vua
                if (possibleMoves.Any(x => x?.Piece?.Type == rules.Key))
                    result = true;
            }

            // Khôi phục loại quân vua 
            concernedKing.Type = Type.King;
            return result;
        }
        public string Explain() => "The King is in check";
    }
}