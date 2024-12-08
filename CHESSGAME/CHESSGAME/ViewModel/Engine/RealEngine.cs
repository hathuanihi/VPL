using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CHESSGAME.Model.Command;
using CHESSGAME.ViewModel.Engine.RuleManager;
using CHESSGAME.ViewModel.Engine.States;
using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;
using Type = CHESSGAME.Model.AModel.Pieces.Type;

namespace CHESSGAME.ViewModel.Engine
{
    public class RealEngine : IEngine
    {
        private Container _container; // Lưu trữ bàn cờ, nước đi đã thực thiện
        private CompensableConversation _conversation; // Quản lý undo, redo
        private Pawn _enPassantPawnBlack;
        private Pawn _enPassantPawnWhite;
        private ObservableCollection<ICompensableCommand> _moves; // Danh sách các lệnh đã thực hiện
        private RuleGroup _ruleGroups; // Nhóm các quy tắc di chuyển
        public RealEngine(Container container)
        {
            Board = container.Board;
            _container = container;
            _moves = container.Moves;

            _conversation = new CompensableConversation(container.Moves);

            _ruleGroups = new PawnRuleGroup();
            _ruleGroups.AddGroup(new BishopRuleGroup());
            _ruleGroups.AddGroup(new KingRuleGroup());
            _ruleGroups.AddGroup(new KnightRuleGroup());
            _ruleGroups.AddGroup(new QueenRuleGroup());
            _ruleGroups.AddGroup(new RookRuleGroup());
        }
        public Board Board { get; }
        public bool DoMove(Move move) // Thực hiện nước đi và trả về true nếu hợp lệ
        {
            // Kiểm tra nếu người chơi chọn cùng một ô
            if (move.StartCoordinate == move.TargetCoordinate) return false;

            Piece piece = Board.PieceAt(move.StartCoordinate);
            Piece targetPiece = Board.PieceAt(move.TargetCoordinate);

            if (_ruleGroups.Handle(move, Board))
            {
                ICompensableCommand command;
                if ((move.PieceType == Type.King) 
                   && (((targetPiece?.Type == Type.Rook) 
                   && (move.PieceColor == targetPiece.Color))
                   || (Math.Abs(move.TargetCoordinate.X - move.StartCoordinate.X) == 2)))
                    command = new CastlingCommand(move, Board);
                else if ((move.PieceType == Type.Pawn) 
                   && (targetPiece == null) 
                   && (move.StartCoordinate.X != move.TargetCoordinate.X))
                    command = new EnPassantCommand(move, Board);
                else if ((move.PieceType == Type.Pawn) &&
                         (move.TargetCoordinate.Y == (move.PieceColor == Color.White ? 0 : 7)))
                    command = new PromoteCommand(move, Board);
                else
                    command = new MoveCommand(move, Board);

                //En passant
                if (move.PieceColor == Color.White)
                {
                    if (_enPassantPawnWhite != null)
                    {
                        _enPassantPawnWhite.EnPassant = false;
                        _enPassantPawnWhite = null;
                    }
                }
                else
                {
                    if (_enPassantPawnBlack != null)
                    {
                        _enPassantPawnBlack.EnPassant = false;
                        _enPassantPawnBlack = null;
                    }
                }
                if ((move.PieceType == Type.Pawn) && (Math.Abs(move.StartCoordinate.Y - move.TargetCoordinate.Y) == 2))
                    if (move.PieceColor == Color.White)
                    {
                        _enPassantPawnWhite = (Pawn)piece;
                        _enPassantPawnWhite.EnPassant = true;
                    }
                    else
                    {
                        _enPassantPawnBlack = (Pawn)piece;
                        _enPassantPawnBlack.EnPassant = true;
                    }

                // Số lần đi kể từ nước đi ăn quân gần nhất 
                if (Board.PieceAt(move.TargetCoordinate) == null)
                    _container.HalfMoveSinceLastCapture++;
                else
                    _container.HalfMoveSinceLastCapture = 0;

                _conversation.Execute(command); // Thực thi
                _moves.Add(command); // Thêm vào danh sách các lệnh đã thực hiện

                return true;
            }

            return false;
        }


        public BoardState CurrentState() // TRạng thái hiện tại của trò chơi
        {
            IState checkState = new CheckState();
            IState patState = new PatState();


            Color color = _moves.Count == 0 ? Color.White : _moves[_moves.Count - 1].PieceColor;

            bool check = checkState.IsInState(Board, color == Color.White ? Color.Black : Color.White);

            bool pat = patState.IsInState(Board, color == Color.White ? Color.Black : Color.White);

            if (pat && check)
                return color == Color.Black ? BoardState.WhiteCheckMate : BoardState.BlackCheckMate;
            if (pat)
                return color == Color.Black ? BoardState.WhitePat : BoardState.BlackPat;
            if (check)
                return color == Color.Black ? BoardState.WhiteCheck : BoardState.BlackCheck;

            return BoardState.Normal;
        }

        public List<Square> PossibleMoves(Piece piece)
        {
            return _ruleGroups.PossibleMoves(piece);
        }
        public Move Undo()
        {
            ICompensableCommand command = _conversation.Undo();
            if (command == null) return null;

            if (_container.HalfMoveSinceLastCapture != 0)
                _container.HalfMoveSinceLastCapture--;
            else
            {
                int count = 0;
                for (int i = _moves.Count - 1; i > 0; i--)
                {
                    if (!_moves[i].TakePiece)
                        count++;
                    else
                        break;
                }
                _container.HalfMoveSinceLastCapture = count;
            }

            _moves.Remove(command);
            return command.Move;
        }
        public Move Redo()
        {
            ICompensableCommand command = _conversation.Redo();
            if (command == null) return null;

            if (!command.TakePiece)
                _container.HalfMoveSinceLastCapture++;
            else
                _container.HalfMoveSinceLastCapture = 0;

            _moves.Add(command);
            return command.Move;
        }
    }
}