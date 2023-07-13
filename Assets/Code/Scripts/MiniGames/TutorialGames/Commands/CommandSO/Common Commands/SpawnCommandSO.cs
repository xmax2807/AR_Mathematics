using UnityEngine;
using Project.Addressable;
namespace Project.MiniGames.TutorialGames
{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Commands/SpawnCommand", fileName = "SpawnCommandSO")]
    public class SpawnCommandSO : CommandSO
    {
        [SerializeField] private GameObjectReferencePack m_references;
        public override ITutorialCommand BuildCommand()
        {
            return new SpawnCommand(m_references);
        }
    }
}