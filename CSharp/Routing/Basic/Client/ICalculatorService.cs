using System.ServiceModel;

namespace Client
{
    [ServiceContract]
    public interface ICalculatorService
    {
        [OperationContract]
        int Add(int number1, int number2);
    }
}