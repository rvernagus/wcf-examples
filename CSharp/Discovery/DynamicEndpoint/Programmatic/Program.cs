using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;
using Discovery.Client;

namespace Discovery
{
    // http://msdn.microsoft.com/en-us/library/dd456783.aspx

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

            // DynamicEndpoint
            {
                var contract = ContractDescription.GetContract(typeof(IEchoService));
                var dynamicEndpoint = new DynamicEndpoint(contract, new NetTcpBinding());
                var findCriteria = new FindCriteria { MaxResults = 1 };
                dynamicEndpoint.FindCriteria = findCriteria;

                // ClientBase
                {
                    var client = new EchoServiceClient(dynamicEndpoint);
                    Console.WriteLine(client.Echo("ClientBase(DynamicEndpoint): Success!"));
                    ((ICommunicationObject) client).Close();
                }

                // ChannelFactory
                {
                    var factory = new ChannelFactory<IEchoService>(dynamicEndpoint);
                    var channel = factory.CreateChannel();
                    Console.WriteLine(channel.Echo("ChannelFactory(DynamicEndpoint): Success!"));
                    ((ICommunicationObject) channel).Close();
                }
            }

            host.Close();
        }
    }
}