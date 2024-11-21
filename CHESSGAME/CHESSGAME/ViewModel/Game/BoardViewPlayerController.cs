using System.Collections.Generic;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.Model.AModel;
using CHESSGAME.View.ModelView;
using CHESSGAME.ViewModel.Core;

namespace CHESSGAME.ViewModel.Game
{
    public class BoardViewPlayerController : PlayerControler
    {
        private BoardView _boardView;

        public BoardViewPlayerController(BoardView boardView)
        {
            _boardView = boardView;
        }

        public bool IsPlayable { get; set; }

        public override void Play(Move move)
        {
            IsPlayable = true;
        }

        public override void Move(Move move)
        {
            IsPlayable = false;
            Player.Move(move);
        }

        public override void InvalidMove(List<string> reasonsList)
        {
            IsPlayable = true;
            //TODO 
        }

        public override List<Square> PossibleMoves(Piece piece)
        {
            return Player.PossibleMoves(piece);
        }

        public override void Stop()
        {
            IsPlayable = false;
        }
    }
}