using UnityEngine;
using Project.Managers;
using System.Threading.Tasks;
using System.Collections;

namespace Project.UI.Panel
{
    [CreateAssetMenu(fileName = "SingleRequestData", menuName = "STO/PanelViewData/SingleRequestData")]
    public class SingleRequestData : UnityEngine.ScriptableObject
    {
        protected const char separator = ',';
        public enum RequestType{
            None, Lesson, Quiz, Game, Achievement
        }
        [SerializeField] protected RequestType requestType = RequestType.None;
        protected event System.Action<bool> onPostRequest;
        void OnEnable(){
            AddPostRequestCallback();
        }
        void OnDisable(){
            RemovePostRequestCallback();
        }
        
        public void Request(string data){
            TimeCoroutineManager.Instance.WaitUntil(
                ()=> requestType != RequestType.None,
                async ()=> await Request(requestType, data),
                timeout: 2f
            );
        }
        public Task<bool> TryRequest(string data){
            return Request(requestType, data, postRequest: false);
        }
        private async Task<bool> Request(RequestType type,string data, bool postRequest = true){
            Task<bool> requestResult = null;
            switch(type){
                case RequestType.Lesson: requestResult = RequestLesson(data);
                break;
                case RequestType.Quiz: requestResult = RequestQuizzes(data);
                break;
            }

            if(requestResult == null){ 
                return false;
            }

            bool result = await requestResult;
            
            if(postRequest){
                onPostRequest?.Invoke(result);
            }
            return true;
        }
        protected async Task<bool> RequestLesson(string lessonName)
        {
            string[] datas = lessonName.Split(separator);
            if(datas.Length != 2) return false;

            bool parseResult = int.TryParse(datas[0], out int unit);
            parseResult = int.TryParse(datas[1], out int chapter) && parseResult;

            if(!parseResult) return false;

            var Result = await DatabaseManager.Instance.LessonController.GetLessonModel(unit, chapter);
            if(Result == null) return false;

            UserManager.Instance.CurrentLesson = Result;
            return true;
        }

        protected async Task<bool> RequestQuizzes(string data){
            string[] datas = data.Split(separator);
            if(datas.Length > 3 || datas.Length <= 0) return false;

            bool parseResult = int.TryParse(datas[0], out int unit);
            if(!parseResult) unit = -1;
            parseResult = int.TryParse(datas[1], out int chapter);            
            if(!parseResult) chapter = -1;
            parseResult = int.TryParse(datas[2], out int semester);
            if(!parseResult) semester = -1;

            var Result = await DatabaseManager.Instance.QuizController.GetQuizModelsAsync(
                unit == -1 ? null : unit,
                chapter == -1 ? null : chapter,
                semester == -1 ? null : semester
            );
            
            if(Result == null) return false;

            UserManager.Instance.CurrentQuizzes = Result;
            return true;
        }
        protected virtual void AddPostRequestCallback(){}
        protected virtual void RemovePostRequestCallback(){}
    }
}