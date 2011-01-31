using System.ServiceModel;

namespace CalculatorService1
{
    [ServiceContract]
    public interface ICalculatorService
    {
        [OperationContract]
        int Add(int number1, int number2);
    }
}