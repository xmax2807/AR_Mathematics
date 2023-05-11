using Project.Utils.ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.QuizSystem.UIFactory
{
    public class AnswerUIFactory
    {
        private Dictionary<QuestionType, Stack<AnswerUI>> _cache;
        private readonly AnswerUISTO builtInPrefabs;
        public AnswerUIFactory(AnswerUISTO builtInPrefabs)
        {
            this.builtInPrefabs = builtInPrefabs;
            
            _cache = new Dictionary<QuestionType, Stack<AnswerUI>>(FlagExtensionMethods.GetLength<QuestionType>());
        }
        public AnswerUI CreateAnswerUI(QuestionType type)
        {
            GameObject prefab = builtInPrefabs.GetPrefab(type);
            if (prefab == null) return null;

            AnswerUI answerUI = GetPreloadedUI(type);

            if (answerUI == null)
            {
                answerUI = type switch
                {
                    QuestionType.SingleChoice => new SingleChoiceAnswerUI(prefab),
                    QuestionType.ShortAnswer => null,
                    _ => null,
                };
            }
            return answerUI;
        }
        private AnswerUI GetPreloadedUI(QuestionType type)
        {
            EnsureStackAvailable(type);

            if (_cache[type].Count > 0)
            {
                return _cache[type].Pop();
            }

            return null;
        }
        public void Relase(QuestionType type, AnswerUI UI){
            EnsureStackAvailable(type);
            _cache[type].Push(UI);
        }

        private void EnsureStackAvailable(QuestionType type){
            if(_cache == null){
                _cache = new Dictionary<QuestionType, Stack<AnswerUI>>(FlagExtensionMethods.GetLength<QuestionType>());
            }
            if(!_cache.ContainsKey(type)){
                _cache[type] = new Stack<AnswerUI>();
            }
        }
    }
}