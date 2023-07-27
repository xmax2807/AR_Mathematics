using Project.Managers;
using Project.AssetIO.Firebase;
using UnityEngine;
using System.Threading.Tasks;
using System.IO;

namespace Project.UI.Panel
{
    public class VideoPagerController : ViewPagerControllerT<PreloadableVideoPanelView>
    {
        [SerializeField] private LoadingPanelView loadingView;
        [SerializeField] private DownloadingPanelView downloadingPanel;
        LessonController LessonController => DatabaseManager.Instance.LessonController;
        public LessonModel LessonModel => UserManager.Instance.CurrentLesson;

        protected override async void SetupList()
        {
            loadingView.SetupUI("Đang tải bài học, em chờ chút nhé...");
            await loadingView.ShowAsync();
            downloadingPanel.SetupUI("Đang tải bài học, em chờ chút nhé...");

            // if(LessonModel == null){
            //     await LessonController.GetLessonModel(unit: unit.unit, chapter: unit.chapter);
            // }
            UserManager.CurrentUnit unit = UserManager.Instance.CurrentUnitProgress;
            UserManager.Instance.CurrentLesson = await LessonController.GetLessonModel(unit: unit.unit, chapter: unit.chapter);

            await FetchPanelView(LessonModel.VideoNumbers, OnBuildUIView);
            await loadingView.HideAsync();
            await downloadingPanel.StartDownload();
            
            InvokeOnPageChanged(0);
            LoadMore();
            await preloadList[0].ShowAsync();
        }

        protected override async Task OnBuildUIView(PreloadableVideoPanelView view, int index)
        {
            Firebase.Storage.StorageReference reference = LessonController.GetVideoReference(LessonModel, index);
            if (reference == null) return;

            System.Uri uri = await reference.GetDownloadUrlAsync();
            Debug.Log("Got video at uri: " + uri.AbsolutePath);
            string relativePath = uri.Segments[uri.Segments.Length - 1].Replace("%2F", Path.DirectorySeparatorChar.ToString());
            Debug.Log("Relative path: " + relativePath);

            bool isFileAvailableOffline = FirebaseStorageDownloadHandler.TryGetLocalFilePath(relativePath, out string fullFilePath);
            Debug.Log($"Full file path: {fullFilePath}");
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