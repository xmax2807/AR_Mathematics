using UnityEngine;
using UnityEngine.UI;
using Project.UI.GameObjectUI;
using Project.UI.TrueFalseUI;
using Project.QuizSystem;
using System;
using Project.MiniGames.UI;

namespace Project.MiniGames.ComparisonGame
{
    public class CompareGameUI : MonoBehaviour
    {
        #region GameObjectUI
        [SerializeField] private TMPro.TextMeshPro firstNumber;
        [SerializeField] private TMPro.TextMeshPro secondNumber;
        [SerializeField] private TMPro.TextMeshPro comparision;
        [SerializeField] private TMPro.TextMeshPro questionText;
        [SerializeField] private TrueFalseButtonGameObject leftSide;
        [SerializeField] private TrueFalseButtonGameObject rightSide;
        #endregion

        #region UI Canvas
        [SerializeField] private NextMissionUI nextMissionUIPrefab;
        private NextMissionUI nextMissionUI;
        public event System.Action NextQuestionButtonClicked;
        #endregion

        private ComparisonTask currentTask;
        private bool isLeftNumberBigger;

        private void Awake(){
            nextMissionUI = Instantiate(nextMissionUIPrefab);
        }
        private void OnEnable(){
            leftSide.OnButtonTouch += OnChosen;
            rightSide.OnButtonTouch += OnChosen;
            if(nextMissionUI != null){
                nextMissionUI.OnNextButtonClicked += NextQuestionButtonClicked;
                nextMissionUI.OnNextButtonClicked += ResetUIState;
            }
        }

        private void ResetUIState()
        {
            leftSide.Reset();
            rightSide.Reset();
        }

        /// <summary>
        /// Update UI from comparison question
        /// </summary>
        /// <param name="leftNumber">left number of the expression</param>
        /// <param name="rightNumber">right number of the expression</param>
        public void UpdateUI(ComparisonTask task){
            currentTask = task;
            questionText.text = task.TaskDescription;
            firstNumber.text = task.LeftNumber.ToString();
            secondNumber.text = task.RightNumber.ToString();
            comparision.text = "|";
            isLeftNumberBigger = task.LeftNumber > task.RightNumber;
        }
        
        private ComparisonQuestion.ExpressionBiggerSide realAnswer;
        private ComparisonQuestion.ExpressionBiggerSide playerAnswered;
        private bool isCorrectAns;
        private void OnChosen(ITouchableObject sender){
            bool isLeftButton = sender.Equals(leftSide);
            bool isRightButton = sender.Equals(rightSide);

            if(isLeftButton == false && isRightButton == false){
                //None of buttons was clicked
                return;
            }

            realAnswer = currentTask.Answer;
            playerAnswered = isLeftButton ? ComparisonQuestion.ExpressionBiggerSide.Left : ComparisonQuestion.ExpressionBiggerSide.Right; 
            
            isCorrectAns = currentTask.IsCorrect(playerAnswered);

            InteractionEventsBehaviour.Instance.BlockRaycast();
            ShowResult();
        }

        private void ShowResult(){
            ChangeButtonOptionsUI();
            ChangeOperatorText(isLeftNumberBigger);
            Managers.TimeCoroutineManager.Instance.WaitForSeconds(1,ShowUICanvas);
        }

        private void ShowUICanvas(){
            nextMissionUI.ShowUI(isCorrectAns);
        }

        private void ChangeButtonOptionsUI(){
            TrueFalseButtonGameObject trueButton = realAnswer == ComparisonQuestion.ExpressionBiggerSide.Left ? leftSide : rightSide;
            TrueFalseButtonGameObject falseButton = realAnswer == ComparisonQuestion.ExpressionBiggerSide.Left ? rightSide : leftSide;

            trueButton.ChangeUI(true);
            if(!isCorrectAns){
                falseButton.ChangeUI(false);
            }
        }

        private void ChangeOperatorText(bool isLeftNumberBigger){            
            comparision.text = isLeftNumberBigger ? ">" : "<";
        }
    }
}