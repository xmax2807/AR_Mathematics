namespace Project.Pattern.Command{
    public interface IUndoable{
        public void Undo();
    }
    public interface IUndoableCommand : ICommand, IUndoable{
    }
}