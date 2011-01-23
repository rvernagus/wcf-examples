using System;
using System.ServiceModel.Discovery;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var proxy = new Service1Client();
            proxy.Open();
            proxy.DoWork();
            Console.WriteLine("Success");
            proxy.Close();
            Console.ReadKey();
        }
    }
}