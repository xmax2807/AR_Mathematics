using UnityEngine;
using System.Linq;

namespace Project.QuizSystem.QuizUIContent{
    [CreateAssetMenu(menuName ="STO/Quiz/QuizUIContentSTO", fileName = "QuizUIContentSTO")]
    public class QuizUIContentSTO : ScriptableObject{
        [System.Serializable]
        public struct ContentUIGroup{
            public QuestionContentType contentType;
            public GameObject Prefab;
        }
        public ContentUIGroup[] Groups;
        public GameObject GetPrefab(QuestionContentType type){
            return Groups.First((x)=>x.contentType == type).Prefab;
        }
    }
}