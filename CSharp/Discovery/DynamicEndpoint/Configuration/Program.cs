using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;
using Discovery.Client;

namespace Discovery
{
    // http://msdn.microsoft.com/en-us/library/dd456792.aspx

    internal class Program
    {
        private static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(Service.EchoService));
            host.Open();

            // DynamicEndpoint
            {
                var contract = ContractDescription.GetContract(typeof(IEchoService));
                var dynamicEndpoint = new DynamicEndpoint(contract, new NetTcpBinding());
                var findCriteria = new FindCriteria { MaxResults = 1 };
                dynamicEndpoint.FindCriteria = findCriteria;

                // ClientBase
                {
                    var client = new EchoServiceClient("endpointConfiguration");
                    Console.WriteLine(client.Echo("ClientBase(DynamicEndpoint): Success!"));
                    ((ICommunicationObject) client).Close();
                }

                // ChannelFactory
                {
                    var factory = new ChannelFactory<IEchoService>("endpointConfiguration");
                    var channel = factory.CreateChannel();
                    Console.WriteLine(channel.Echo("ChannelFactory(DynamicEndpoint): Success!"));
                    ((ICommunicationObject) channel).Close();
                }
            }

            host.Close();
        }
    }
}