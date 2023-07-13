using UnityEngine;
using Project.Pattern.Command;
namespace Project.MiniGames.TutorialGames{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Context", fileName = "ContextSO")]
    public class ContextSO : ScriptableObject{
        [SerializeField] private CommandSO[] commands;
        private Context context;
        public Context Context {
            get{
                context ??= new Context(commands);
                return context;
            }
        }
    }
}