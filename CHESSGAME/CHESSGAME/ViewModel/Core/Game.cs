using System.Collections.Generic;
using CHESSGAME.ViewModel.Engine;
using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;

namespace CHESSGAME.ViewModel.Core
{
    public class Game
    {
        private Player _currentPlayer;
        private readonly bool _canUndoRedo; // Trả về true nếu undo redo có thể thực hiện
        private Player WhitePlayer { get; }
        private Player BlackPlayer { get; }
        private IEngine Engine { get; } // Xử lý logic trò chơi
        public Container Container { get; set; } // Chứa thông tin mô hình trò chơi
        public Game(IEngine engine, Player whitePlayer, Player blackPlayer, Container container, bool canUndoRedo)
        {
            _canUndoRedo = canUndoRedo;
            WhitePlayer = whitePlayer;
            BlackPlayer = blackPlayer;
            Engine = engine;
            Container = container;
            WhitePlayer.MoveDone += PlayerMoveHandler; // Sự kiện người chơi thực hiện nước đi
            BlackPlayer.MoveDone += PlayerMoveHandler;

            _currentPlayer = WhitePlayer; // Lượt đầu tiên luôn là người chơi trắng
            OnBoardStateChanged();

            _currentPlayer.Play(null);
        }
        private void PlayerMoveHandler(Player sender, Move move)
        {
            if (sender != _currentPlayer)
            {
                sender.Stop();
            }
            else
            {
                if (Engine.DoMove(move))
                {
                    _currentPlayer.Stop();
                    ChangePlayer();
                    OnBoardStateChanged();
                }

                _currentPlayer.Play(move);
            }
        }

        private void ChangePlayer() => _currentPlayer = _currentPlayer == WhitePlayer ? BlackPlayer : WhitePlayer;

        public List<Square> PossibleMoves(Piece piece) => Engine.PossibleMoves(piece);

        public void Undo()
        {
            if (!_canUndoRedo) return;

            Move move = Engine.Undo();
            if (move == null) return;

            _currentPlayer.Stop();
            ChangePlayer();
            OnBoardStateChanged();
            _currentPlayer.Play(null);
        }

        public void Undo(int count)
        {
            if (!_canUndoRedo) return;

            Move lastMove = null;
            _currentPlayer.Stop();
            for (int i = 0; i < count; i++)
            {
                Move move = Engine.Undo();
                if (move != null)
                {
                    ChangePlayer();
                    lastMove = move;
                }
            }
            _currentPlayer.Play(lastMove);
            if (lastMove != null)
                OnBoardStateChanged();
        }
        public void Redo()
        {
            if (!_canUndoRedo) return;

            Move move = Engine.Redo();
            if (move == null) return;

            _currentPlayer.Stop();
            ChangePlayer();
            StateChanged?.Invoke(Engine.CurrentState());
            _currentPlayer.Play(null);
        }

        public delegate void StateHandler(BoardState state);
        public event StateHandler StateChanged; // Trạng thái bàn cờ bị thay đổi
        private void OnBoardStateChanged() => StateChanged?.Invoke(Engine.CurrentState());
    }
}