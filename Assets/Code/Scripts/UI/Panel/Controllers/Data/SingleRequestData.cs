using UnityEngine;
using Project.Managers;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace Project.UI.Panel
{
    [CreateAssetMenu(fileName = "SingleRequestData", menuName = "STO/PanelViewData/SingleRequestData")]
    public class SingleRequestData : UnityEngine.ScriptableObject
    {
        protected const char separator = ',';
        public enum RequestType
        {
            None, Lesson, Quiz, Game, Achievement
        }
        [SerializeField] protected RequestType requestType = RequestType.None;
        protected event System.Action<bool> onPostRequest;
        void OnEnable()
        {
            AddPostRequestCallback();
        }
        void OnDisable()
        {
            RemovePostRequestCallback();
        }

        public void Request(string data)
        {
            TimeCoroutineManager.Instance.WaitUntil(
                () => requestType != RequestType.None,
                async () => await Request(requestType, data),
                timeout: 2f
            );
        }
        public async Task<LessonModel> GetNextLessonModel()
        {
            UserManager.CurrentUnit currentUnit = UserManager.Instance.CurrentUnitProgress;
            LessonModel result = await DatabaseManager.Instance.LessonController.GetLessonModel(currentUnit.unit + 1, currentUnit.chapter);            
            return result;
        }
        public async Task<GameModel[]> GetIncomingGames(){
            UserManager.CurrentUnit currentUnit = UserManager.Instance.CurrentUnitProgress;
            List<GameModel> result = await DatabaseManager.Instance.GameController.GetListGames(currentUnit.unit, currentUnit.chapter);
            return result.ToArray();
        }
        private async Task<bool> Request(RequestType type, string data, bool postRequest = true)
        {
            Task<bool> requestResult = null;
            switch (type)
            {
                case RequestType.Lesson:
                    requestResult = RequestLesson(data);
                    break;
                case RequestType.Quiz:
                    requestResult = RequestQuizzes(data);
                    break;
                case RequestType.Game:
                    requestResult = RequestGames(data);
                    break;
            }

            if (requestResult == null)
            {
                return false;
            }

            bool result = await requestResult;

            if (postRequest)
            {
                onPostRequest?.Invoke(result);
            }
            return true;
        }

        private async Task<bool> RequestGames(string unitChapter)
        {
            string[] datas = unitChapter.Split(separator);
            if (datas.Length != 2) return false;

            bool parseResult = int.TryParse(datas[0], out int unit);
            parseResult = int.TryParse(datas[1], out int chapter) && parseResult;
            
            if(parseResult == false) return false;

            var Result = await DatabaseManager.Instance.GameController.GetListGames(unit, chapter);
            if (Result == null) return false;

            UserManager.Instance.CurrentGameScenes = Result.ToArray();
            return true;
        }

        protected async Task<bool> RequestLesson(string lessonName)
        {
            string[] datas = lessonName.Split(separator);
            if (datas.Length != 2) return false;

            bool parseResult = int.TryParse(datas[0], out int unit);
            parseResult = int.TryParse(datas[1], out int chapter) && parseResult;

            if (!parseResult) return false;

            var Result = await DatabaseManager.Instance.LessonController.GetLessonModel(unit, chapter);
            if (Result == null) return false;

            UserManager.Instance.CurrentLesson = Result;
            UserManager.Instance.CurrentUnitProgress = new UserManager.CurrentUnit()
            {
                unit = unit,
                chapter = chapter,
                semester = Result.LessonSemester,
            };
            return true;
        }

        protected async Task<bool> RequestQuizzes(string data)
        {
            string[] datas = data.Split(separator);
            if (datas.Length > 3 || datas.Length <= 0) return false;

            bool parseResult = int.TryParse(datas[0], out int unit);
            if (!parseResult) unit = -1;
            parseResult = int.TryParse(datas[1], out int chapter);
            if (!parseResult) chapter = -1;
            parseResult = int.TryParse(datas[2], out int semester);
            if (!parseResult) semester = -1;

            var Result = await DatabaseManager.Instance.QuizController.GetQuizModelsAsync(
                unit == -1 ? null : unit,
                chapter == -1 ? null : chapter,
                semester == -1 ? null : semester
            );

            if (Result == null) return false;

            UserManager.Instance.CurrentQuizzes = Result;
            UserManager.Instance.CurrentUnitProgress = new UserManager.CurrentUnit()
            {
                unit = unit,
                chapter = chapter,
                semester = semester,
            };
            return true;
        }
        protected virtual void AddPostRequestCallback() { }
        protected virtual void RemovePostRequestCallback() { }
    }
}