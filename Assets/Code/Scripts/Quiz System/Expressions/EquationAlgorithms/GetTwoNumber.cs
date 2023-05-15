namespace Project.QuizSystem.Expressions
{

    public static class GetTwoNumberAlgo
    {
        static System.Random rand = new();

        /// <summary>
        /// In Vietnamese: Lấy 2 số từ tổng sao cho 2 số cộng lại mà không cần nhớ
        /// Ví dụ: 5 + 4 = 9 là phép cộng không nhớ, 5 + 6 = 11 là phép cộng có nhớ
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static (int, int) GetTwoNumberWithoutCarrying(int max)
        {
            int firstNumber = 0;
            int secondNumber = 0;
            int place = 1;

            while (max > 0)
            {
                int digit = max % 10;
                int firstDigit = rand.Next(digit) + 1;
                firstNumber += firstDigit * place;
                secondNumber += (digit - firstDigit) * place;
                max /= 10;
                place *= 10;
            }

            return (firstNumber, secondNumber);
        }
    }
}