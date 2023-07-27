using System;
using Project.Utils;

namespace Project.QuizSystem.Expressions.NumberSentence{
    public interface INumberSentence<T> where T : System.IComparable<T>{
        T GetUnknownRandomly(Random rand);
        T GetUnknownByValue(T value);
        string GetFullSentence();
    }

    
}