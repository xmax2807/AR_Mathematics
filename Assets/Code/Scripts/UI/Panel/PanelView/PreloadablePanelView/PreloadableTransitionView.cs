using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Project.Managers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
namespace Project.UI.Panel
{
    public class PreloadableTransitionView : PreloadablePanelView
    {
        [SerializeField] private RequestAndChangeScene NextLessonReq;
        [SerializeField] private RequestAndChangeScene GameSceneReq;
        [SerializeField] private RequestAndChangeScene QuizSceneReq;
        public System.Collections.Generic.List<(string, UnityAction)> buttonCreationTasks;
        private void Awake()
        {
            buttonCreationTasks = new(5);
        }
        private IEnumerator PrepareNextLesson()
        {
            if (NextLessonReq == null) yield break;

            var currentUnit = UserManager.Instance.CurrentUnitProgress;
            if (currentUnit.IsEmpty()) yield break;
            bool? attempt = null;
            LessonModel nextLesson = null;

            GameSceneReq.GetNextLessonModel().ContinueWith(task =>
            {
                nextLesson = task.Result;
                attempt = nextLesson != null;
            });
            yield return new WaitUntil(() => attempt != null);
            bool result = (bool)attempt;

            if (!result) {
                string quizReq = $",{currentUnit.chapter},{currentUnit.semester}";
                void onPreviewClick() => QuizSceneReq.Request(quizReq);
                buttonCreationTasks.Add(("Ôn tập", onPreviewClick));
                yield break;
            }


            string req = $"{nextLesson.LessonUnit},{nextLesson.LessonChapter}";
            void onButtonClick() => NextLessonReq.Request(req);

            string label = nextLesson.LessonTitle;
            buttonCreationTasks.Add((label, onButtonClick));

        }

        private IEnumerator PrepareGameLink()
        {
            if (GameSceneReq == null) yield break;

            var currentLesson = UserManager.Instance.CurrentUnitProgress;
            if (currentLesson.IsEmpty()) yield break;

            bool? attempt = null;
            GameModel[] games = null;

            GameSceneReq.GetIncomingGames().ContinueWith(task =>
            {
                games = task.Result;
                attempt = games != null;
            });
            yield return new WaitUntil(() => attempt != null);
            bool result = (bool)attempt;

            if (!result) yield break;


            foreach (GameModel gameModel in games)
            {
                string req = gameModel.GameScene;
                void onButtonClick() => GameSceneReq.ManualLoadScene(req);

                string label = gameModel.GameTitle;
                buttonCreationTasks.Add((label, onButtonClick));
            }
        }
        public override IEnumerator PrepareAsync()
        {
            if (isPrepared) yield break;
            isPrepared = true;
            yield return PrepareNextLesson();
            yield return PrepareGameLink();

        }

        public override IEnumerator UnloadAsync()
        {
            yield break;
        }
    }

}