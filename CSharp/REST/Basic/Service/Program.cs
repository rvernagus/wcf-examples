using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Service
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var host = new WebServiceHost(typeof(RestService), new Uri("http://localhost:8000")))
            {
                host.AddServiceEndpoint(typeof(IRestContract), new WebHttpBinding(), "");
                host.Open();
                Console.WriteLine("Service is running");
                Console.WriteLine("Press enter to quit...");
                Console.ReadLine();
                host.Close();
            }
        }
    }
}