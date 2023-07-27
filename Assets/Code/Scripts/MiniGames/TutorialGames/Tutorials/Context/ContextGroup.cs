using UnityEngine;

namespace Project.MiniGames.TutorialGames
{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/ContextGroup", fileName = "ContextGroup")]
    public class ContextGroup : ScriptableObject
    {
        [SerializeField] ContextSO[] contexts;
        public int Length => contexts.Length;
        public Context this[int key]
        {
            get => contexts[key]?.Context;
        }
    }
}