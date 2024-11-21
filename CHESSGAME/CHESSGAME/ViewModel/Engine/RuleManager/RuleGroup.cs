﻿using System;
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
        public RuleGroup Next { get; internal set; }
        protected List<IRule> Rules { get; set; } = new List<IRule>();
        protected abstract Type Type { get; }

        public void AddGroup(RuleGroup ruleGroup)
        {
            if (Next == null)
                Next = ruleGroup;
            else
                Next.AddGroup(ruleGroup);
        }

        public bool Handle(Move move, Board board)
        {
            if (move.PieceType == Type) return Rules.All(rule => rule.IsMoveValid(move, board));
            if (Next != null) return Next.Handle(move, board);
            throw new Exception("NOBODY TREATS THIS PIECE !!! " + move.PieceType);
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
            throw new Exception("NOBODY TREATS THIS PIECE !!! " + piece);
        }
    }
}