using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Project.Managers;
using UnityEngine;
using UnityEngine.UI;
namespace Project.UI.Panel
{
    public class PreloadableTransitionView : PreloadablePanelView
    {
        [SerializeField] private RequestAndChangeScene NextLessonReq;
        public event System.Func<Button> OnCreateLinkButton;

        private bool ResultRequest = false;
        private bool isUIPrepared = false;

        private IEnumerator PrepareNextLesson()
        {
            if (NextLessonReq == null) yield break;

            var currentLesson = UserManager.Instance.CurrentLesson;
            if(currentLesson == null) yield break;
            string req = $"{currentLesson.LessonUnit + 1},{currentLesson.LessonChapter}";
            bool? result = null;

            NextLessonReq.TryRequest(req).ContinueWith(task => result = task.Result);
            yield return new WaitUntil(() => result != null);
            ResultRequest = (bool)result;
        }
        public override IEnumerator PrepareAsync()
        {
            if (isPrepared) yield break;
            isPrepared = true;
            yield return PrepareNextLesson();
            CreateButton();
        }

        private void CreateButton()
        {
            if(!ResultRequest) return;

            Button button = OnCreateLinkButton?.Invoke();

            if (button == null) return;

            var currentLesson = UserManager.Instance.CurrentLesson;
            string req = $"{currentLesson.LessonUnit},{currentLesson.LessonChapter}";

            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = UserManager.Instance.CurrentLesson.LessonTitle;
            button.onClick.AddListener(() => NextLessonReq.Request(req));
        }

        public override IEnumerator UnloadAsync()
        {
            yield break;
        }
    }

}