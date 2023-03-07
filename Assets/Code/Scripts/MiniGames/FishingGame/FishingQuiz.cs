using UnityEngine;
using Project.QuizSystem;
using System.Collections.Generic;

namespace Project.MiniGames.FishingGame{
    public class FishingQuiz : MonoBehaviour{
        [SerializeField] private string TemplateQuest;
        [SerializeField] private int maxQuest = 20;
        
        private ShapeQuestion[] quests;

        private void Awake(){
            quests = new ShapeQuestion[maxQuest];
            for(int i = 0; i < maxQuest; i++){
                quests[i] = new ShapeQuestion(TemplateQuest);
            }
        }
    }
}