using Project.Managers;
using Project.AssetIO.Firebase;
using UnityEngine;
using System.Threading.Tasks;

namespace Project.UI.Panel
{
    public class VideoPagerController : ViewPagerControllerT<PreloadableVideoPanelView>
    {
        [SerializeField] private LoadingPanelView loadingView;
        [SerializeField] private DownloadingPanelView downloadingPanel;
        LessonController lessonController => DatabaseManager.Instance.LessonController;
        LessonModel lessonModel => UserManager.Instance.CurrentLesson;

        protected override async void SetupList()
        {
            loadingView.SetupUI("Đang tải bài học, em chờ chút nhé...");
            await loadingView.ShowAsync();
            downloadingPanel.SetupUI("Đang tải bài học, bé chờ chút nhé...");
            
            if(lessonModel == null){
                await downloadingPanel.HideAsync();
                return;
            }
            await FetchPanelView(lessonModel.VideoNumbers, OnBuildUIView);
            await loadingView.HideAsync();
            await downloadingPanel.StartDownload();
            
            InvokeOnPageChanged(0);
            LoadMore();
            await preloadList[0].ShowAsync();
        }

        protected override async Task OnBuildUIView(PreloadableVideoPanelView view, int index)
        {
            var reference = lessonController.GetVideoReference(lessonModel, index);
            if (reference == null) return;

            System.Uri uri = await reference.GetDownloadUrlAsync();
            string relativePath = uri.Segments[uri.Segments.Length - 1].Replace("%2F", "/");

            bool isFileAvailableOffline = FirebaseStorageDownloadHandler.TryGetLocalFilePath(relativePath, out string fullFilePath);
            if (!isFileAvailableOffline)
            {
                downloadingPanel.AddDownloadTask(reference, fullFilePath, OnDownloadComplete);
            }
            else
            {
                Debug.Log($"{fullFilePath}");
                view.VideoUrl = fullFilePath;
                view.VideoPlayerBehaviour.VideoUrl = $"{fullFilePath}";
                //view.VideoPlayerBehaviour.PrepareVideoUrl(fullFilePath);
            }

        }

        private void OnDownloadComplete(int index, DownloadingPanelView.DownloadTask task)
        {
            if (preloadList[index] is not PreloadableVideoPanelView view) return;
            // string filePath = $"file://{task.destinationFile}";
            string filePath = $"{task.destinationFile}";
            view.VideoUrl = filePath;
            view.VideoPlayerBehaviour.VideoUrl = filePath;
        }
    }
}