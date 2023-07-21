using UnityEngine;
using Project.Addressable;
using System;

namespace Project.MiniGames.TutorialGames
{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Commands/SpawnCommand", fileName = "SpawnCommandSO")]
    public class SpawnCommandSO : CommandSO
    {
        [SerializeField] private GameObjectReferencePack m_references;
        [SerializeField] private string[] m_names;
        [SerializeField] private SpawnCommand.SpawnAlgorithm algorithm;
        [SerializeField] private ObjectDataDeliver m_objectDataDeliver;
        public override ITutorialCommand BuildCommand()
        {
            return new SpawnCommand(m_references, algorithm: algorithm, spawnCallback: AddToDelivery);
        }

        private void AddToDelivery(GameObject[] objs)
        {
            if(m_objectDataDeliver == null){
                return;
            }
            
            Debug.Log("objs is " + objs);
            objs ??= new GameObject[0];

            string[] names = new string[objs.Length];
            for(int i = 0; i < objs.Length; ++i)
            {
                if(i >= m_names.Length){
                    names[i] = "";
                    continue;
                }
                names[i] = m_names[i];
            }


            m_objectDataDeliver.Objects = objs;
            m_objectDataDeliver.Names = names;
            Debug.Log("Added to Develivery");
        }
    }
}