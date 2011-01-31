using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var service1 = new CalculatorService1Client();
            try
            {
                service1.Open();
                var result = service1.Add(4, 3);
                Console.WriteLine("{0} + {1} = {2}", 4, 3, result);
                service1.Close();
            }
            catch (Exception)
            {
                service1.Abort();
                throw;
            }
        }
    }
}
