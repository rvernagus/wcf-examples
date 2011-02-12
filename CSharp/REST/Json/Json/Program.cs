using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace REST.Json
{
    class Program
    {
        static void Main(string[] args)
        {
            const string address = "http://localhost:8000";
            var host = new WebServiceHost(typeof(Service), new Uri(address));
            host.AddServiceEndpoint(typeof(IService), new WebHttpBinding(), "");
            var debugBehavior = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            debugBehavior.HttpHelpPageEnabled = true;
            host.Open();
            Console.WriteLine("Service is running");

            var factory = new ChannelFactory<IService>(new WebHttpBinding(), address);
            factory.Endpoint.Behaviors.Add(new WebHttpBehavior());
            var client = factory.CreateChannel();

            var data = client.GetData(1);
            Console.WriteLine(data);

            Console.WriteLine("any key");
            Console.ReadKey();
        }
    }
}
