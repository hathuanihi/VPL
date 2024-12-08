using System.Collections.Generic;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.Model.AModel;

namespace CHESSGAME.ViewModel.Core
{
    public abstract class PlayerController
    {
        public Player Player { get; set; }
        public abstract void Play(Move move); // Lượt đi của ai
        public abstract void Move(Move move); 
        public abstract void InvalidMove(List<string> reasonsList);
        public abstract List<Square> PossibleMoves(Piece piece);
        public abstract void Stop();
    }
}