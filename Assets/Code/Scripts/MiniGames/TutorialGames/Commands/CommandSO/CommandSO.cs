using UnityEngine;
namespace Project.MiniGames.TutorialGames
{
    public abstract class CommandSO : ScriptableObject{
        public abstract ITutorialCommand BuildCommand();
    }
}