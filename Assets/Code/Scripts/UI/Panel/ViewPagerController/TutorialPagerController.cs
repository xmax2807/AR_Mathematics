using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;
using Project.Utils.ExtensionMethods;

namespace Project.UI.Panel{
    public class TutorialPagerController : ViewPagerControllerT<PreloadableVideoPanelView>{
        
        [System.Serializable]
        public struct TutorialData{
            public string VideoUrl;
            public string TutorialName;
        }
        [SerializeField] private TutorialData[] datas;
        protected override async void SetupList()
        {            
            if(datas == null || datas.Length == 0){
                Debug.Log("Tutorial videos is not assigned");
                return;
            }
            await FetchPanelView(datas.Length, OnBuildUIView);
            //await downloadingPanel.StartDownload();
            
            LoadMore();
        }

        protected override async Task OnBuildUIView(PreloadableVideoPanelView view, int index)
        {
            bool isDone = false;
            IEnumerator task = Managers.ResourceManager.Instance.GetLocalFile<VideoClip>(datas[index].VideoUrl,(clip)=> {
                isDone = true;
                OnGotVideo(view, clip);
            });
            StartCoroutine(task);
            while(isDone == false){
                await Task.Delay(10);
            }
        }

        private void OnGotVideo(PreloadableVideoPanelView view,VideoClip clip){
            if(clip == null){
                Debug.Log("TutorialController: Clip is null");
                return;
            }
            string fullFilePath = clip.originalPath;
            Debug.Log($"TutorialController: got video url: {fullFilePath}");
            view.Clip = clip;
            //view.VideoPlayerBehaviour.VideoUrl = $"{fullFilePath}";
        }

        public void MoveTo(string name){
            int index = datas.FindIndex((x)=>x.TutorialName == name);
            if(index == -1){
                Debug.Log("TutorialController: invalid name: " + name);
                return;
            }
            MoveTo(index);
        }
    }
}