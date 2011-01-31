using System;
using System.ServiceModel;

namespace Client
{
    class CalculatorService1Client : ClientBase<ICalculatorService>, ICalculatorService
    {
        public CalculatorService1Client()
            : base("service1")
        {
            
        }
        
        public int Add(int number1, int number2)
        {
            return Channel.Add(number1, number2);
        }
    }
}