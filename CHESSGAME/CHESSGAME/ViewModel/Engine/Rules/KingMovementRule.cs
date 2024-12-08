﻿using System;
using System.Collections.Generic;
using System.Linq;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.Model.AModel;
using Type = CHESSGAME.Model.AModel.Pieces.Type;

namespace CHESSGAME.ViewModel.Engine.Rules
{
    public class KingMovementRule : IRule
    {
        public bool IsMoveValid(Move move, Board board)
        {
            if (((board.PieceAt(move.TargetCoordinate)?.Color == move.PieceColor) &&
                 (board.PieceAt(move.TargetCoordinate)?.Type == Type.Rook)) ||
                ((Math.Abs(move.TargetCoordinate.X - move.StartCoordinate.X) == 2) &&
                 (move.TargetCoordinate.Y == move.StartCoordinate.Y)))
                return true;

            return (Math.Abs(move.StartCoordinate.X - move.TargetCoordinate.X) <= 1) &&
                   (Math.Abs(move.StartCoordinate.Y - move.TargetCoordinate.Y) <= 1);
        }

        public List<Square> PossibleMoves(Piece piece)
        {
            return piece.Square.Board.Squares.OfType<Square>().ToList().FindAll(
                x => IsMoveValid(new Move(piece, x), piece.Square.Board));
        }
    }
}