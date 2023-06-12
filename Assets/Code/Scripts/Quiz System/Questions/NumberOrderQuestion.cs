using System;
using System.Collections.Generic;
using Project.QuizSystem.SaveLoadQuestion;
using Project.Utils.ExtensionMethods;
using System.Linq;
using Project.Utils;

namespace Project.QuizSystem{
    public class NumberOrderQuestion : BaseQuestion<int[]>, IRandomizableQuestion<int[]>, ISavableQuestion
    {
        private int m_count;
        private int m_maxNumber;
        private bool m_isDescending;
        private IComparer<int> m_comparer;
        public NumberOrderQuestion(string question, int count, int maxNumber, IComparer<int> orderComparer) : base(question)
        {
            if(m_count >= maxNumber){
                //throw new Exception("count can't larger than maxNumber");
                maxNumber = m_count + 10;
            }
            m_count = count;
            m_maxNumber = maxNumber;
            m_comparer = orderComparer;
            _answer = new int[m_count];
            m_isDescending = false;
        }
        public NumberOrderQuestion(string question, int count, int maxNumber) : this(question, count, maxNumber, new IntNumberComparer(false)){}
        public NumberOrderQuestion(string question, int count, IComparer<int> comparer) : this(question, count, maxNumber: 10, comparer){}
        public NumberOrderQuestion(string question, int maxNumber) : this(question, 5, maxNumber){}
        public NumberOrderQuestion(string question, int maxNumber, bool IsDescending = false) : this(question, 5, maxNumber){
            m_comparer = new IntNumberComparer(IsDescending); 
            m_isDescending = IsDescending;
        }

        public override QuestionType QuestionType => QuestionType.Other;

        public override QuestionContentType QuestionContentType => QuestionContentType.None;

        public override IQuestion Clone() => new NumberOrderQuestion(this._question, this.m_count, this.m_maxNumber, this.m_comparer);

        public IQuestion GetClone()
        {
            return Clone();
        }
        protected override IEqualityComparer<int[]> GetEqualityComparer()
        {
            return new ListComparer<int>(EqualityComparer<int>.Default);
        }
        public override string GetQuestion()
        {
            return ConvertOptionToString(this._answer.Shuffle());
        }

        public IQuestion Random(Random rand = null)
        {
            var instance = new NumberOrderQuestion(this._question, this.m_count, this.m_maxNumber, this.m_comparer);
            instance.Randomize(rand);
            return instance;
        }

        public void Randomize(Random rand = null)
        {
            rand ??= Managers.SpawnerManager.RandomInstance;
            
            int min = Math.Min(4, m_maxNumber - 1);
            m_count = rand.Next(min,m_maxNumber);

            //Start creating the answer (list of numbers)
            HashSet<int> duplicateChecker = new(m_count);
            _answer = new int[m_count];

            //random first number
            _answer[0] = rand.Next(1,m_maxNumber + 1);
            duplicateChecker.Add(_answer[0]);

            int minRange = 1;
            int maxRange = m_maxNumber + 1;
            for(int i = 1; i < m_count; ++i){
                int value;
                int randomRetries = 6; // try random 5 times
                do{
                    value = rand.Next(minRange, maxRange);
                    --randomRetries;
                }
                while(randomRetries > 0 && duplicateChecker.Contains(value));


                // iterate the list to find a non-duplicated number
                if(randomRetries == 0){
                    
                    for(; minRange < maxRange; ++minRange){
                        if(!duplicateChecker.Contains(minRange))break;
                    }
                    value = minRange;
                }

                //int compareResult = m_comparer.Compare(_answer[i-1], value);
                //UnityEngine.Debug.Log($"{_answer[i-1]} {compareResult} {value}");
                
                // _answer[i - 1] > value in ascending or < value in descending.
                // then swap
                _answer[i] = value;
                // int startSwap = i;
                // while(startSwap > 0){
                //     int compareResult = m_comparer.Compare(_answer[startSwap-1], _answer[startSwap]);
                    
                //     if(compareResult > 0){
                //         (_answer[startSwap - 1],_answer[startSwap]) = (_answer[startSwap],_answer[startSwap - 1]);
                //     }
                    
                //     --startSwap;
                // }
                

//                UnityEngine.Debug.Log($"{_answer[i-1]} {_answer[i]}");
            
                duplicateChecker.Add(value);
            }
            var list  = _answer.ToList();
            list.Sort();
            _answer = list.ToArray();
            // foreach(int i in _answer){
            //     UnityEngine.Debug.Log(i);
            // }
            //UnityEngine.Debug.Log(GetQuestion());
        }

        public int[][] GetRandomOptions(int count)
        {
            int[][] result = new int[count][];

            for(int i = 0; i <count; i++){
                result[i] = Answer.Shuffle();
            }
            return result;
        }

        public string ConvertOptionToString(int[] option)
        {
            System.Text.StringBuilder stringBuilder = new();

            int i = 0;
            for(; i < option.Length - 1; ++i){
                stringBuilder.Append(option[i]).Append(", ");
            }
            stringBuilder.Append(option[i]);
            
            return stringBuilder.ToString();
        }

        public int[] ParseOptionFromString(string data)
        {
            string[] datas = data.Split(',');
            List<int> list = new(datas.Length);

            foreach(string number in datas){
                if(int.TryParse(number, out int realNumber)){
                    list.Add(realNumber);
                }
            }

            return list.ToArray();
        }

        public QuestionSaveData ConvertToData()
        {
            return new NumberOrderQSD(){
                Count = this.m_count,
                MaxNumber = this.m_maxNumber,
                Question = this._question,
                IsAscending = !m_isDescending,
                Numbers = _answer,
            };
        }

        public void SetData(QuestionSaveData data)
        {
            if(data is not NumberOrderQSD realData) return;

            this.m_count = realData.Count;
            this.m_maxNumber = realData.MaxNumber;
            this.m_comparer = new IntNumberComparer(IsDescending: !realData.IsAscending);
            this._question = realData.Question;
            _answer = (int[]) realData.Numbers.Clone();
        }
    }
}