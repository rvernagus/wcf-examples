using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using Discovery.Client;

namespace Discovery
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Service
            var host = new ServiceHost(typeof(Service.EchoService), new Uri("net.tcp://localhost:4567"));
            host.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
            host.AddDefaultEndpoints();
            host.AddServiceEndpoint(new UdpDiscoveryEndpoint());
            host.Open();

            // DiscoveryClient
            {
                var discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());
                var findCriteria = new FindCriteria(typeof(IEchoService)) { MaxResults = 1 };
                var findResults = discoveryClient.Find(findCriteria);
                var endpointAddress = findResults.Endpoints.First().Address;
                var binding = new NetTcpBinding();

                // ClientBase
                {
                    var client = new EchoServiceClient(binding, endpointAddress);
                    Console.WriteLine(client.Echo("ClientBase(DynamicEndpoint): Success!"));
                    ((ICommunicationObject) client).Close();
                }

                // ChannelFactory
                {
                    var factory = new ChannelFactory<IEchoService>(binding, endpointAddress);
                    var channel = factory.CreateChannel();
                    Console.WriteLine(channel.Echo("ChannelFactory(DynamicEndpoint): Success!"));
                    ((ICommunicationObject) channel).Close();
                }
            }

            host.Close();
        }
    }
}