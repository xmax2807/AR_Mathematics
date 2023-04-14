using System.Collections.Generic;
using Project.Utils.ExtensionMethods;
using UnityEngine;

namespace Project.QuizSystem.QuizUIContent{
    public class QuizUIContentFactory
    {
        private Dictionary<QuestionContentType, Stack<QuizUIContent>> _cache;
        private readonly QuizUIContentSTO builtInPrefabs;

        private readonly QuestionVisitor QuestionVisitor;
        public QuizUIContentFactory(QuizUIContentSTO builtInPrefabs, QuestionVisitor visitor)
        {
            this.builtInPrefabs = builtInPrefabs;
            this.QuestionVisitor = visitor;
            
            _cache = new Dictionary<QuestionContentType, Stack<QuizUIContent>>(FlagExtensionMethods.GetLength<QuestionContentType>());
        }
        public QuizUIContent CreateQuizContentUI(QuestionContentType type)
        {
            GameObject prefab = builtInPrefabs.GetPrefab(type);
            if (prefab == null) return null;

            QuizUIContent answerUI = GetPreloadedUI(type);

            if (answerUI == null)
            {
                answerUI = type switch
                {
                    QuestionContentType.Image => new ImageQuizContent(prefab, QuestionVisitor),
                    QuestionContentType.Text => new TMProUIQuizContent(prefab, QuestionVisitor),
                    QuestionContentType.Slider => null,
                    _ => null,
                };
            }
            return answerUI;
        }
        private QuizUIContent GetPreloadedUI(QuestionContentType type)
        {
            EnsureStackAvailable(type);

            if (_cache[type].Count > 0)
            {
                return _cache[type].Pop();
            }

            return null;
        }
        public void Relase(QuestionContentType type, QuizUIContent UI){
            EnsureStackAvailable(type);
            _cache[type].Push(UI);
        }

        private void EnsureStackAvailable(QuestionContentType type){
            if(_cache == null){
                _cache = new Dictionary<QuestionContentType, Stack<QuizUIContent>>(FlagExtensionMethods.GetLength<QuestionType>());
            }
            if(!_cache.ContainsKey(type)){
                _cache[type] = new Stack<QuizUIContent>();
            }
        }
    }
}