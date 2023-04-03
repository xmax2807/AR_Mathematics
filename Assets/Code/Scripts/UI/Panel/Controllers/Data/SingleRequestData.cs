using UnityEngine;
using Project.Managers;

namespace Project.UI.Panel
{
    [CreateAssetMenu(fileName = "SingleRequestData", menuName = "STO/PanelViewData/SingleRequestData")]
    public class SingleRequestData : UnityEngine.ScriptableObject
    {
        public void Request(string lessonName)
        {
            DatabaseManager.Instance.LessonController
            .GetLessonModel(lessonName)
            .ContinueWith(task => {
                UserManager.Instance.CurrentLesson = task.Result;
            });
        }
    }
}