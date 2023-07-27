using System.Collections;
using Project.Pattern.Command;
namespace Project.MiniGames.TutorialGames{
    public interface ITutorialCommand
    {
        IEnumerator Execute(ICommander commander);
    }
    public interface IUndoableTutorialCommand : ITutorialCommand{
        IEnumerator Undo(ICommander commander);
    }
}