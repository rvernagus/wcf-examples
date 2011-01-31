using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace CalculatorService1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(CalculatorService)))
            {
                try
                {
                    host.Open();
                    Console.WriteLine("The Calculator Service is running.");
                    Console.WriteLine("Press <ENTER> to quit.");
                    Console.ReadLine();
                    host.Close();
                }
                catch (CommunicationException)
                {
                    host.Abort();
                    throw;
                }
            }
        }
    }
}
