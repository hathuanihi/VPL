using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CHESSGAME.Model.Command
{
    public class CompensableConversation : ICompensableConversation
    {
        private ObservableCollection<ICompensableCommand> _moveList;
        private Stack<ICompensableCommand> _redoCommands = new Stack<ICompensableCommand>();
        private Stack<ICompensableCommand> _undoCommands = new Stack<ICompensableCommand>();

        public CompensableConversation(ObservableCollection<ICompensableCommand> moveList)
        {
            _moveList = moveList;
            foreach (ICompensableCommand command in _moveList)
                _undoCommands.Push(command);
        }
        public void Execute(ICompensableCommand command)
        {
            command.Execute();
            _undoCommands.Push(command);
            _redoCommands.Clear();
        }
        public ICompensableCommand Undo()
        {
            if (_undoCommands.Count == 0) return null;

            ICompensableCommand command = _undoCommands.Pop();
            command.Compensate();
            _redoCommands.Push(command);

            return command;
        }
        public ICompensableCommand Redo()
        {
            if (_redoCommands.Count == 0) return null;

            ICompensableCommand command = _redoCommands.Pop();
            command.Execute();
            _undoCommands.Push(command);

            return command;
        }
    }
}