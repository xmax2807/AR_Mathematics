using Gameframe.GUI.TransitionSystem;
using Gameframe.GUI.PanelSystem;
using UnityEngine;
namespace Project.UI.Panel{

    [CreateAssetMenu(fileName ="SingleSceneLoadData", menuName ="STO/PanelViewData/SingleSceneLoadData")]
    public class SingleSceneLoadData : UnityEngine.ScriptableObject{
        [SerializeField] private string SceneName;
        public SingleSceneLoadBehaviour behaviour;
        public void LoadScene(string sceneName){
            behaviour.SceneName = sceneName;
            behaviour.Load();
        }
        public void LoadScene(){
            LoadScene(SceneName);
        }
    }
}