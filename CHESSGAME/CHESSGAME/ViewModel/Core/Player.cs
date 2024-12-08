using System.Collections.Generic;
using CHESSGAME.Model.AModel.Pieces;
using CHESSGAME.Model.AModel;

namespace CHESSGAME.ViewModel.Core
{
    public class Player
    {
        public Color Color { get; internal set; }

        private PlayerController _playerControler;

        public Game Game { get; set; }

        public Player(Color color, PlayerController playerControler)
        {
            Color = color;
            _playerControler = playerControler;
        }
        public void Play(Move move) => _playerControler.Play(move);

        public void Stop() => _playerControler.Stop();

        public List<Square> PossibleMoves(Piece piece) => Game.PossibleMoves(piece);

        public void Move(Move move)
        {
            MoveDone?.Invoke(this, move);
        }

        public delegate void MoveHandler(Player sender, Move move);
        public event MoveHandler MoveDone; // Người chơi đã thực hiện nước đi
    }
}
