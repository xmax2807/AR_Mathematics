using Project.Managers;
namespace Project.UI.Panel{
    public class VideoPagerController : ViewPagerControllerT<PreloadableVideoPanelView>{
        LessonController lessonController => DatabaseManager.Instance.LessonController;
        LessonModel lessonModel => UserManager.Instance.CurrentLesson;
        private void Start(){
            Setup(lessonModel.VideoNumbers, OnBuildVideoView);
            LoadMore();
            preloadList[0].ShowAsync();
        }

        private async void OnBuildVideoView(PreloadableVideoPanelView view, int index){
            System.Uri uri = await lessonController.GetVideoUrl(lessonModel, index);
            
            if(uri == null) return;
            
            view.VideoPlayerBehaviour.VideoUrl = uri.ToString();
        }
    }
}