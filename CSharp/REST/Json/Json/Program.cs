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

            // Service
            var host = new WebServiceHost(typeof(Service), new Uri(address));
            var ep = host.AddServiceEndpoint(typeof(IService), new WebHttpBinding(), "");
            var webHttpBehavior = new WebHttpBehavior { HelpEnabled = true };
            ep.Behaviors.Add(webHttpBehavior);
            host.Open();
            Console.WriteLine("Service is running.");
            Console.WriteLine("Browse to '{0}/help' for help.", address);

            // Client
            var factory = new ChannelFactory<IService>(new WebHttpBinding(), address);
            factory.Endpoint.Behaviors.Add(new WebHttpBehavior());
            var client = factory.CreateChannel();
            var data = client.GetData(1);
            Console.WriteLine(data);

            Console.WriteLine("Press <ENTER> to close host.");
            Console.ReadLine();

            (client as ICommunicationObject).Close();
            host.Close();
        }
    }
}
