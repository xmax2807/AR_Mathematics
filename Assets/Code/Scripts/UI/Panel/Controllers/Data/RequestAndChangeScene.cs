using UnityEngine;
using System.Threading.Tasks;

namespace Project.UI.Panel
{
    [CreateAssetMenu(fileName = "SceneTransitionWithRequest", menuName = "STO/PanelViewData/SceneTransitionWithRequest")]
    public class RequestAndChangeScene : SingleRequestData
    {
        [SerializeField] private SingleSceneLoadData sceneLoadData;

        protected override void AddPostRequestCallback()
        {
            onPostRequest += ShouldLoadScene;
        }
        protected override void RemovePostRequestCallback()
        {
            onPostRequest -= ShouldLoadScene;
        }
        public Task<bool> TryRequst(string data){
            return TryRequest(data);
        }
        private void ShouldLoadScene(bool isRequestSuccess){
            
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