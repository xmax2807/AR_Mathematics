using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.MiniGames.TutorialGames
{
    public class SpawnCommand : ITutorialCommand
    {
        private Addressable.GameObjectReferencePack m_pack;
        private Transform parentTransform;
        public SpawnCommand(Addressable.GameObjectReferencePack pack, Transform parentTransform = null){
            this.m_pack = pack;
            this.parentTransform = parentTransform;
        }
        
        public IEnumerator Execute(ICommander commander)
        {
            Task<GameObject[]> task = Managers.AddressableManager.Instance.PreLoadAssets(m_pack.References);
            yield return new TaskAwaitInstruction(task);

            GameObject[] m_objects = task.Result;
            for(int i = 0; i < m_objects.Length; ++i){
                Project.Managers.SpawnerManager.Instance.SpawnObjectInParent(m_objects[i], parentTransform);
            }
            yield break;
        }
    }
}