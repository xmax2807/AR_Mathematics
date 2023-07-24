using UnityEngine;
namespace Project.MiniGames.TutorialGames{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Data/PracticeTaskDataSO", fileName = "PracticeTaskData")]
    public class PracticeTaskDataSO : ScriptableObject{
        public int Count;
        public PracticeTaskGenerator.PracticeTaskType Type;

        [Tooltip("Format of the question, for approriate question type, it should be have {0},{i} in it.")]
        public string QuestionFormat;
    }
}