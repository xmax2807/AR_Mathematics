using UnityEngine;

namespace Project.MiniGames.TutorialGames
{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Data/ObjectDataDeliver", fileName = "ObjectDataDeliver")]
    public class ObjectDataDeliver : ScriptableObject{
        public GameObject[] Objects;
        public string[] Names;

        public void OnDisable(){
            Objects = null;
            Names = null;
        }
    }
}