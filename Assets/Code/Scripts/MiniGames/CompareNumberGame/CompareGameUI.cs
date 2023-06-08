using UnityEngine;

namespace Project.MiniGames.ComparisonGame
{
    public class CompareGameUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshPro firstNumber;
        [SerializeField] private TMPro.TextMeshPro secondNumber;
        [SerializeField] private TMPro.TextMeshPro comparision;

        /// <summary>
        /// Update UI from comparison question
        /// </summary>
        /// <param name="leftNumber">left number of the expression</param>
        /// <param name="rightNumber">right number of the expression</param>
        /// <param name="answer">bool value: true:left side bigger, false: right side bigger </param>
        public void UpdateUI(int leftNumber, int rightNumber, bool answer){
            firstNumber.text = leftNumber.ToString();
            secondNumber.text = rightNumber.ToString();
        }

        private void OnChosen(){}
    }
}