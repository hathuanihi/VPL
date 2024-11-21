using System.Collections.Generic;
using CHESSGAME.ViewModel.Engine;
using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;

namespace CHESSGAME.ViewModel.Core
{
    public class Game
    {
        private Player _currentPlayer;
        private readonly bool _canUndoRedo;
        private Player WhitePlayer { get; }
        private Player BlackPlayer { get; }
        private IEngine Engine { get; }
        public Container Container { get; set; }

        /// <summary>
        /// Construct a game with an engine and two players
        /// </summary>
        /// <param name="engine">The engin the game will use</param>
        /// <param name="whitePlayer">White player</param>
        /// <param name="blackPlayer">Black player</param>
        /// <param name="container">Model container</param>
        /// <param name="canUndoRedo"></param>
        public Game(IEngine engine, Player whitePlayer, Player blackPlayer, Container container, bool canUndoRedo)
        {
            _canUndoRedo = canUndoRedo;
            WhitePlayer = whitePlayer;
            BlackPlayer = blackPlayer;
            Engine = engine;
            Container = container;
            WhitePlayer.MoveDone += PlayerMoveHandler;
            BlackPlayer.MoveDone += PlayerMoveHandler;

            _currentPlayer = WhitePlayer;
            OnBoardStateChanged();

            _currentPlayer.Play(null);
        }

        /// <summary>
        /// Delegate called when a player makes a move.
        /// </summary>
        /// <remarks>
        /// We check if the move is valid and if so we ask the other player to play.
        /// Otherwise we tell the player that the move is invalid so that he gives us another move.
        /// We carry out these actions as long as the game is not checkmate or checkmate.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="move"></param>
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

        #region Undo Redo

        /// <summary>
        /// Asks the engine to cancel the last move played
        /// </summary>
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

        /// <summary>
        /// Asks the engine to redo the last canceled stroke
        /// </summary>
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

        #endregion

        #region Delegate and Events

        public delegate void StateHandler(BoardState state);
        public event StateHandler StateChanged;
        private void OnBoardStateChanged() => StateChanged?.Invoke(Engine.CurrentState());

        public delegate void PlayerDisconnectedEventHandler(string message);
        public event PlayerDisconnectedEventHandler PlayerDisconnectedEvent;

        #endregion

    }
}