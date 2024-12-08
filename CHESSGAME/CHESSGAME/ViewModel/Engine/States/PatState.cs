﻿using System.Collections.Generic;
using System.Linq;
using CHESSGAME.ViewModel.Engine.RuleManager;
using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;

namespace CHESSGAME.ViewModel.Engine.States
{
    internal class PatState : IState
    {
        public bool IsInState(Board board, Color color)
        {
            // Nhóm các quy tắc di chuyển
            RuleGroup ruleGroup = new PawnRuleGroup();
            ruleGroup.AddGroup(new BishopRuleGroup());
            ruleGroup.AddGroup(new KingRuleGroup());
            ruleGroup.AddGroup(new KnightRuleGroup());
            ruleGroup.AddGroup(new QueenRuleGroup());
            ruleGroup.AddGroup(new RookRuleGroup());

            List<Square> possibleSquares = new List<Square>();

            foreach (Square square in board.Squares.OfType<Square>().Where(x => x?.Piece?.Color == color))
                if (square.Piece != null)
                    possibleSquares = possibleSquares.Concat(ruleGroup.PossibleMoves(square.Piece)).ToList();
            return possibleSquares.Count == 0;
        }

        public string Explain() => "It's a stalemate.";
    }
}