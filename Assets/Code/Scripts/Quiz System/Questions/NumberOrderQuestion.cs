using System;
using System.Collections.Generic;

namespace Project.QuizSystem{
    public class NumberOrderQuestion : BaseQuestion<int[]>, IRandomizableQuestion
    {
        private int m_count;
        private int m_maxNumber;
        private readonly IComparer<int> m_comparer;
        public NumberOrderQuestion(string question, int count, int maxNumber, IComparer<int> orderComparer) : base(question)
        {
            if(m_count > maxNumber){
                //throw new Exception("count can't larger than maxNumber");
                maxNumber = m_count + 10;
            }
            m_count = count;
            m_maxNumber = maxNumber;
            m_comparer = orderComparer;
            _answer = new int[m_count];
        }
        public NumberOrderQuestion(string question, int count, int maxNumber) : this(question, count, maxNumber, Comparer<int>.Default){}
        public NumberOrderQuestion(string question, int count, IComparer<int> comparer) : this(question, count, maxNumber: 10, comparer){}
        public NumberOrderQuestion(string question, int count) : this(question, count, maxNumber: 10){}

        public override QuestionType QuestionType => QuestionType.Other;

        public override QuestionContentType QuestionContentType => QuestionContentType.None;

        public override IQuestion Clone() => new NumberOrderQuestion(this._question, this.m_count, this.m_maxNumber, this.m_comparer);

        public IQuestion GetClone()
        {
            return Clone();
        }
        protected override IEqualityComparer<int[]> GetEqualityComparer()
        {
            return new Utils.ExtensionMethods.ListComparer<int>(EqualityComparer<int>.Default);
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
            
            m_count = rand.Next(m_maxNumber); 

            //Start creating the answer (list of numbers)
            HashSet<int> duplicateChecker = new(m_count);
            _answer = new int[m_count];

            //random first number
            _answer[0] = rand.Next(m_maxNumber);
            duplicateChecker.Add(_answer[0]);

            int minRange = 0;
            int maxRange = m_maxNumber;
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

                int compareResult = m_comparer.Compare(_answer[i-1], value);
                
                // _answer[i - 1] > value in ascending or < value in descending.
                // then swap
                if(compareResult > 0){
                    _answer[i] = _answer[i - 1];
                    _answer[i - 1] = value;
                }
            
                duplicateChecker.Add(value);
            }
        }
    }
}