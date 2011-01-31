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
            var service1 = new Service1Client();
            try
            {
                service1.Open();
                var result = service1.GetData(123);
                Console.WriteLine(result);
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
