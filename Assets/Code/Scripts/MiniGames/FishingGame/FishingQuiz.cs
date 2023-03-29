using UnityEngine;
using Project.QuizSystem;
using Project.Utils.ExtensionMethods;

namespace Project.MiniGames.FishingGame{
    public class FishingQuiz{
        private string TemplateQuest;
        private int maxQuest;
        private int current;
        private ShapeQuestion[] quests;
        public event System.Action<string> OnQuestionChanged;
        public string CurrentQuestion {get;private set;}
        public FishingQuiz(string templateQuest, int maxQuestNumber){
            if(maxQuestNumber <= 0) return;

            maxQuest = maxQuestNumber;
            TemplateQuest = templateQuest;

            quests = new ShapeQuestion[maxQuest];
            
            for(int i = 0; i < maxQuest; i++){
                do{

                    quests[i] = new ShapeQuestion(templateQuest);
                }
                while(quests.CheckAnyDuplicateWithinRange(quests[i], i, 3));
            }
            CurrentQuestion = quests[0].GetQuestion();
        }
        public bool IsCorrect(Shape.ShapeType type){
            bool result = quests[current].IsCorrect(new Shape(type));

            if(result) {
                current++;
                if(current == quests.Length){
                    return true;
                }
                CurrentQuestion = quests[current].GetQuestion();
                OnQuestionChanged?.Invoke(CurrentQuestion);
            }

            return result;
        }
    }
}