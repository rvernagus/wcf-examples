using System;
using System.ServiceModel;
using Discovery.Client;

namespace Discovery
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(Service.EchoService));
            host.Open();

            // DiscoveryClient, configuration

            // ClientBase
            {
                var client = new EchoServiceClient("endpointConfiguration");
                Console.WriteLine(client.Echo("ClientBase(DynamicEndpoint): Success!"));
                ((ICommunicationObject)client).Close();
            }

            // ChannelFactory
            {
                var factory = new ChannelFactory<IEchoService>("endpointConfiguration");
                var channel = factory.CreateChannel();
                Console.WriteLine(channel.Echo("ChannelFactory(DynamicEndpoint): Success!"));
                ((ICommunicationObject)channel).Close();
            }

            host.Close();
        }
    }
}