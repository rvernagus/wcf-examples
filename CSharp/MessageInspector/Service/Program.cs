using System;
using System.ServiceModel;

namespace MessageInspector
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(MyService)))
            {
                var binding = new NetTcpBinding(SecurityMode.Message);
                const string address = "net.tcp://localhost:4500/";
                var endpoint = host.AddServiceEndpoint(typeof(IMyService), binding, address);
                endpoint.Behaviors.Add(new MessageInspectorBehavior());
                host.Open();

                var client = new GenericClient<IMyService>(binding, address);
                client.Endpoint.Behaviors.Add(new MessageInspectorBehavior());
                var data = client.Service.GetData("test data");
                Console.WriteLine(data);
                (client as ICommunicationObject).Close();
                host.Close();
            }
        }
    }
}