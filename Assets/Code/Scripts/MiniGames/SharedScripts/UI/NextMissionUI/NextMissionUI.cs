using System;
using System.Collections;
using Project.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Project.MiniGames.UI{
    [RequireComponent(typeof(Canvas))]
    public class NextMissionUI : MonoBehaviour{
        private Canvas canvas;
        [SerializeField] private Button nextQuestButton;
        [SerializeField] private Animator TrueFalseAnswerGIF;
        public event System.Action OnNextButtonClicked;
        private void Awake(){
            this.transform.SetParent(GameManager.RootCanvas.transform, false);
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
        }

        private void OnEnable(){
            nextQuestButton?.onClick.AddListener(InvokeButtonClick); 
        }
        private void InvokeButtonClick(){
            OnNextButtonClicked?.Invoke();
            canvas.enabled = false;
        }

        public void ShowUI(bool result){
            nextQuestButton.interactable = result;
            StartCoroutine(VideoStart(TrueFalseAnswerGIF, result, ()=> ShouldShowNextMission(result)));
            canvas.enabled = true;
        }

        private void ShouldShowNextMission(bool isCorrect){
            if(isCorrect == false){
                canvas.enabled = false;
                ARGameEventManager.Instance.RaiseEvent<bool>(ARGameEventManager.EndGameEventName, false);
            }
        }

        IEnumerator VideoStart(Animator anim, bool isCorrectAns, Action postCallback)
        {
            anim.gameObject.SetActive(true);
            anim.SetBool("isCorrect", isCorrectAns);
            // if (RightAns == true)
            // {
            //     anim.Play("RightAnswerAnimation");
            // }
            // else
            // {
            //     anim.Play("WrongAnswerAnimation");
            // }
            //anim.transform.LookAt(mainCam.transform);
            var animInfo = anim.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(animInfo.length);

            anim.gameObject.SetActive(false);
            postCallback?.Invoke();
        }
    }
}