namespace Project.QuizSystem.Expression{
    
    public class GetTwoNumberAlgo{
        static System.Random rand = new();
        public (int, int ) GetTwoNumber(int max){
            int first = rand.Next(1,max);
        
        int firstDigit = first/ 10;
        int secDigit = first % 10;
        int second = rand.Next(10 - firstDigit) * 10;
        second += rand.Next(10 - secDigit);
        return (first,second);
        }
    }
}