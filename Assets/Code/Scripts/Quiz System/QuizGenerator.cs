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
        private static Dictionary<(int,int), IRandomizableQuestion> RandomizableQuestionDict;

        private bool isInitialized = false;
        private void Awake(){
            RandomizableQuestionDict ??= new(){
                    {(2,1), new ShapeSCQ(4)},
                    {(3,1), new ShapeSCQ(4)},
                    {(1,3), new EquationSCQ(10,4)},
                    {(3,3), new EquationSCQ(10,4)},
                    {(2,4), new EquationSCQ(20,4)},
                    {(2,5), new EquationSCQ(90,4)},
                    {(6,5), new EquationSCQ(100,4)},
                    {(7,5), new EquationSCQ(100,4)},
                    {(8,5), new EquationSCQ(100,4)},
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
        public IQuestion CreateRandomQuestion(int unit, int chapter){
            (int,int) unitChapter =  (unit, chapter);
            if(RandomizableQuestionDict.ContainsKey(unitChapter)){
                return RandomizableQuestionDict[unitChapter].Random();
            }
            return null;
        }
        public IQuestion CreateQuestion(int unit, int chapter){
            (int,int) unitChapter =  (unit, chapter);
            if(RandomizableQuestionDict.ContainsKey(unitChapter)){
                return RandomizableQuestionDict[unitChapter].GetClone();
            }
            return null;
        }
    }
}