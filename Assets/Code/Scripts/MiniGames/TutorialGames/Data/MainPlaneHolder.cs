using UnityEngine;
namespace Project.MiniGames.TutorialGames{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Data/MainPlaneHolder", fileName = "MainPlaneHolder")]
    public class MainPlaneHolder : ScriptableObject
    {
        public GameObject MainPlaneObj {get; private set;}

        public void SetMainPlaneObj(GameObject obj){
            MainPlaneObj = obj;
        }
    }
}