using UnityEngine;
using Project.Managers;
using System.Threading.Tasks;

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
                ()=> Request(requestType, data),
                timeout: 2f
            );
        }
        private async void Request(RequestType type,string data){
            Task<bool> requestResult = null;
            switch(type){
                case RequestType.Lesson: requestResult = RequestLesson(data);
                break;
            }

            if(requestResult == null){
                return;
            }

            bool result = await requestResult;
            onPostRequest?.Invoke(result);
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

            Debug.Log("Request Result: " + Result.LessonChapter);
            UserManager.Instance.CurrentLesson = Result;
            return true;
        }
        protected virtual void AddPostRequestCallback(){}
        protected virtual void RemovePostRequestCallback(){}
    }
}