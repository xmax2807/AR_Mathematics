using UnityEngine;
using UnityEngine.UI;

namespace Project.QuizSystem.UIFactory{
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
    public class AnswerUIController : MonoBehaviour{
        private Canvas canvas;
        private GraphicRaycaster graphicRaycaster;

        void Awake(){
            canvas = GetComponent<Canvas>();
            graphicRaycaster = GetComponent<GraphicRaycaster>();
        }

        public void LockUI() => graphicRaycaster.enabled = false;
        public void UnlockUI() => graphicRaycaster.enabled = true;
    }
}