using System.Collections.Generic;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.Model.AModel;

namespace CHESSGAME.ViewModel.Core
{
    public class Player
    {
        public Color Color { get; internal set; }

        private PlayerControler _playerControler;

        public Game Game { get; set; }

        public Player(Color color, PlayerControler playerControler)
        {
            Color = color;
            _playerControler = playerControler;
        }

        /// <summary>
        /// Notifies the player that it is their turn to play and that the Game can receive a move from them.
        /// As long as this movement is not valid, this method is called.
        /// </summary>
        public void Play(Move move) => _playerControler.Play(move);

        public void Stop() => _playerControler.Stop();

        public List<Square> PossibleMoves(Piece piece) => Game.PossibleMoves(piece);

        public void Move(Move move)
        {
            MoveDone?.Invoke(this, move);
        }

        public delegate void MoveHandler(Player sender, Move move);
        public event MoveHandler MoveDone;
    }
}
