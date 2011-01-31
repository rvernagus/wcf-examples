namespace CalculatorService1
{
    public class CalculatorService : ICalculatorService
    {
        #region ICalculatorService Members

        public int Add(int number1, int number2)
        {
            return number1 + number2;
        }

        #endregion
    }
}