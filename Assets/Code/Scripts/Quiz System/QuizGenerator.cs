using UnityEngine;
using Project.UI.UISetPack;
using UnityEngine.UI;
using Project.Utils.ExtensionMethods;
using Project.Managers;
using Project.QuizSystem.UIFactory;
using Project.QuizSystem.QuizUIContent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Utils;

namespace Project.QuizSystem
{
    public class QuizGenerator : MonoBehaviour
    {
        [SerializeField] private AnswerUISTO BuiltInAnswerUIPrefabs;
        [SerializeField] private QuizUIContentSTO BuiltInQuizContentPrefabs;
        [SerializeField] private GroupUISpritePack spriteGroupPack;

        private AnswerUIFactory AnswerUIFactory;
        private QuizUIContentFactory ContentUIFactory;
        private QuestionVisitor QuestionVisitor;
        private static Dictionary<(int,int, string), IRandomizableQuestion> RandomizableQuestionDict;

        private bool isInitialized = false;
        private void Awake(){
            RandomizableQuestionDict ??= new(){
                    {(2,1,""), new ShapeSCQ(4)},
                    {(3,1,""), new ShapeSCQ(4)},
                    {(4,2,"ss"), new InEqualitySCQ(maxNumber:10, optionsLength:4)},
                    {(4,2,"sx"), new NumberOrderSCQ(optionsLength:4, maxNumber: 10, false)},
                    {(4,2,"ln"), new ExtremeNumberSCQ(optionsLength:4, maxNumber:10, isMinimumFinding: false)},
                    {(4,2,"bn"), new ExtremeNumberSCQ(optionsLength:4, maxNumber:10, isMinimumFinding: true)},
                    {(1,3,""), new EquationSCQ(10,4)},
                    {(3,3,""), new EquationSCQ(10,4)},
                    {(2,4,""), new EquationSCQ(20,4)},
                    {(2,5,""), new EquationSCQ(90,4)},
                    {(6,5,"bn"), new ExtremeNumberSCQ(optionsLength:4, maxNumber:100, isMinimumFinding: true, minNumber: 10)},
                    {(6,5,"ln"), new ExtremeNumberSCQ(optionsLength:4, maxNumber:100, isMinimumFinding: false, minNumber: 10)},
                    {(6,5,"ss"), new NumberOrderSCQ(optionsLength:4, maxNumber: 100, false)},
                    {(6,5,"sx"), new NumberOrderSCQ(optionsLength:4, maxNumber: 100, false)},
                    {(7,5,""), new EquationSCQ(100,4)},
                    {(8,5,""), new EquationSCQ(100,4)},
                };
            QuestionVisitor =  new QuestionVisitor(spriteGroupPack);
            AnswerUIFactory = new AnswerUIFactory(BuiltInAnswerUIPrefabs);
            ContentUIFactory = new QuizUIContentFactory(BuiltInQuizContentPrefabs, QuestionVisitor);
        }
        public async Task InitAsset(){
            if(isInitialized) return;
            await spriteGroupPack.Init();
            isInitialized = true;
        }
        public AnswerUI GetAnswerUI(QuestionType type) {
            AnswerUI UI = AnswerUIFactory.CreateAnswerUI(type);
            return UI;
        }
        public QuizUIContent.QuizUIContent GetQuizUIContent(QuestionContentType type){
            return ContentUIFactory.CreateQuizContentUI(type);
        }
        public IQuestion CreateRandomQuestion(int unit, int chapter, string title){
            (int,int,string) quizType =  (unit, chapter, title);
            if(RandomizableQuestionDict.ContainsKey(quizType)){
                return RandomizableQuestionDict[quizType].Random();
            }
            return null;
        }
        public IQuestion CreateQuestion(int unit, int chapter, string title){
            (int,int,string) quizType =  (unit, chapter, title);
            if(RandomizableQuestionDict.ContainsKey(quizType)){
                return RandomizableQuestionDict[quizType].Clone();
            }
            return null;
        }
    }
}