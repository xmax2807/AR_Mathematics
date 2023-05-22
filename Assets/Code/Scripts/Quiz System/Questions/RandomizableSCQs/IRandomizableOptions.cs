namespace Project.QuizSystem{
    public interface IRandomizableOptions<T>{
        T[] GetRandomOptions(int count);
        string ConvertOptionToString(T option);
        T ParseOptionFromString (string data);
        T Answer {get;}
    }
}