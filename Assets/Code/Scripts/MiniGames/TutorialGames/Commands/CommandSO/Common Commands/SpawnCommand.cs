using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
            yield return new WaitUntil(()=>Managers.AddressableManager.Instance.IsInitialized);
            
            Debug.Log("Getting references");
            Task<GameObject[]> task = Managers.AddressableManager.Instance.PreLoadAssets(m_pack.References);
            yield return new TaskAwaitInstruction(task);
            // while(!task.IsCompleted){
            //     yield return null;
            // }
            Debug.Log("Got references");

            GameObject[] m_objects = task.Result;
            for(int i = 0; i < m_objects.Length; ++i){
                Project.Managers.SpawnerManager.Instance.SpawnObjectInParent(m_objects[i], parentTransform);
            }
            yield break;
        }
    }
}