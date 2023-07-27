using UnityEngine;
using Project.Pattern.Command;
namespace Project.MiniGames.TutorialGames{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Context", fileName = "ContextSO")]
    public class ContextSO : ScriptableObject{
        [SerializeField] private CommandSO[] commands;
        public Context Context {
            get{
                return new Context(commands);
            }
        }
    }
}