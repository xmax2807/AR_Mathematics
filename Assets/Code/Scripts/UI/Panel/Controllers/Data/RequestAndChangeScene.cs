using UnityEngine;
using Project.Managers;

namespace Project.UI.Panel
{
    [CreateAssetMenu(fileName = "SceneTransitionWithRequest", menuName = "STO/PanelViewData/SceneTransitionWithRequest")]
    public class RequestAndChangeScene : SingleRequestData
    {
        [SerializeField] private SingleSceneLoadData sceneLoadData;

        protected override void AddPostRequestCallback()
        {
            Debug.Log("added");
            onPostRequest += ShouldLoadScene;
        }
        protected override void RemovePostRequestCallback()
        {
            Debug.Log("removed");
            onPostRequest -= ShouldLoadScene;
        }
        private void ShouldLoadScene(bool isRequestSuccess){
            Debug.Log(isRequestSuccess);
            if(isRequestSuccess){
                sceneLoadData.LoadScene();
            }
            else{
                //TODO: Show dialog could not load scene
                Debug.Log("request failed");
            }
        }
    }
}