using System;
using System.Collections.Generic;
using System.Linq;
using Project.Utils.ExtensionMethods;

namespace Project.MiniGames.TutorialGames
{
    public class PracticeTaskGenerator
    {
        public enum PracticeTaskType
        {
            WhichIsPracticeTask,
        }

        private PracticeTask[] _tasks;
        private int _currentIndex;

        private string[] _uniqueNames;
        private PracticeTaskDataSO _practiceTaskData;
        public PracticeTaskGenerator(string[] uniqueNames, PracticeTaskDataSO data)
        {
            _uniqueNames = uniqueNames;
            _practiceTaskData = data;
            _currentIndex = 0;
        }
        public void GeneratePracticeTask()
        {
            int count = _practiceTaskData.Count;
            _tasks = new PracticeTask[count];
            int[] answerIndexes = GenerateAnswerIndexRandomly();
            PracticeTask template = GetTemplate();

            for (int i = 0; i < count; ++i)
            {
                _tasks[i] = template.Clone();
                _tasks[i].BuildTask(answerIndexes[i]);
            }
        }

        private int[] GenerateAnswerIndexRandomly()
        {
            // if there is only one array element no need to shuffle
            if(_uniqueNames.Length <= 1){
                int index = _uniqueNames.Length == 0 ? -1 : 0;
                return new int[] { index };
            }

            // start shuffle and generate the answers
            List<int> result = new();
            int[] shuffle = new int[_uniqueNames.Length];

            // assign index to each array element
            for (int i = 0; i < _uniqueNames.Length; ++i)
            {
                shuffle[i] = i;
            }

            int startIndex = 0;
            while (startIndex < _practiceTaskData.Count)
            {
                int endIndex = Math.Min(_uniqueNames.Length, _practiceTaskData.Count - startIndex);

                int[] newShuffle = shuffle.Shuffle();

                if (result.Count > 0)
                {
                    // ensure the there is no 2 duplicate indexes stand next each other in the array
                    // the loop will try reshuffle 6 times to avoid infinite loop
                    // if the loop tried 6 times and still no success, break the loop
                    int tryTime = 6;
                    while (result.Last() == newShuffle.First() || tryTime > 0)
                    {
                        newShuffle.SelfShuffle();
                        --tryTime;
                    }
                }
                result.AddRange(newShuffle.Take(endIndex));

                startIndex += endIndex;
            }
            // Debug.Log all indexes
            for (int i = 0; i < _practiceTaskData.Count; ++i)
            {
                UnityEngine.Debug.Log(i + " : " + result[i]);
            }
            return result.ToArray();
        }

        private PracticeTask GetTemplate()
        {
            string questionFormat = _practiceTaskData.QuestionFormat;
            return _practiceTaskData.Type switch
            {
                PracticeTaskType.WhichIsPracticeTask => new WhichIsPracticeTask(questionFormat, _uniqueNames),
                _ => null,
            };
        }

        public void NextTask()
        {
            ++_currentIndex;
        }
        public PracticeTask this[int index]
        {
            get
            {
                if (index < 0 || index >= _tasks.Length)
                {
                    return null;
                }
                return _tasks[index];
            }
        }
    }
}