using UnityEngine;
using System.Linq;

namespace Project.QuizSystem.UIFactory{
    [CreateAssetMenu(menuName ="STO/Quiz/AnswerUISTO", fileName = "AnswerUISTO")]
    public class AnswerUISTO : ScriptableObject{
        [System.Serializable]
        public struct AnswerUIGroup{
            public QuestionType type;
            public GameObject Prefab;
        }
        public AnswerUIGroup[] Groups;
        public GameObject GetPrefab(QuestionType type){
            return Groups.First((x)=>x.type == type).Prefab;
        }
    }
}