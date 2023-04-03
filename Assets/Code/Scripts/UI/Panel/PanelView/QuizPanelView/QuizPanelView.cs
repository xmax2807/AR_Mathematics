using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Project.QuizSystem;

namespace Project.UI.Panel
{
    public class QuizPanelView : MonoBehaviour
    {
        [SerializeField] private LayoutGroup Layout;
        private Quiz quiz;
        void OnEnable(){
            quiz.OnQuestionChanged += ChangeQuest;
        }
        void OnDisable(){
            quiz.OnQuestionChanged -= ChangeQuest;
        }

        private void ChangeQuest(IQuestion newQuestion){
            newQuestion.UpdateUI(Layout);
        }
    }
}
