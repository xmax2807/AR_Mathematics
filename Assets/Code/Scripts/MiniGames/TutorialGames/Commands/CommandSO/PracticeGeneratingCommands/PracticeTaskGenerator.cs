using System;
using UnityEngine;

namespace Project.MiniGames.TutorialGames
{
    public class PracticeTaskGenerator
    {
        public enum PracticeTaskType{
            WhichIsPracticeTask,
        }

        private PracticeTask[] _tasks;
        private int _currentIndex;

        private string[] _uniqueNames;
        private PracticeTaskDataSO _practiceTaskData; 
        public PracticeTaskGenerator(string[] uniqueNames, PracticeTaskDataSO data){
            _uniqueNames = uniqueNames;
            _practiceTaskData = data;
            _currentIndex = 0;
        }
        public void GeneratePracticeTask(){
            int count = _practiceTaskData.Count;
            _tasks = new PracticeTask[count];
            
            PracticeTask template = GetTemplate();

            for(int i = 0; i < count; ++i){
                _tasks[i] = template.Clone();
            }
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

        public void NextTask(){
            ++_currentIndex;
        }
        public PracticeTask CurrentTask {
            get{
                //ensure index is not out of range
                if(_currentIndex >= _tasks.Length){
                    return null;
                }
                return _tasks[_currentIndex];
            }
        } 
    }
}