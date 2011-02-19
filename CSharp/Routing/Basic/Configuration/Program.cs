using System;
using System.ServiceModel;
using System.ServiceModel.Routing;

namespace Routing
{
    class Program
    {
        static void Main(string[] args)
        {
            // Echo Service
            var serviceHost = new ServiceHost(typeof(EchoService));
            serviceHost.Open();
            Console.WriteLine("Service:\tOpen '{0}'", serviceHost.Description.Endpoints[0].Address);

            // Routing Service
            var routerHost = new ServiceHost(typeof(RoutingService));
            routerHost.Open();
            Console.WriteLine("Router: \tOpen '{0}'", routerHost.Description.Endpoints[0].Address);

            // Client
            var factory = new ChannelFactory<IEchoService>("clientEndpoint");
            Console.WriteLine("Client: \tCreating channel '{0}'", factory.Endpoint.Address);
            var channel = factory.CreateChannel();
            Console.WriteLine("Client: \tCalling...");
            var result = channel.Echo("Test");
            Console.WriteLine("Client: \t{0}", result);

            ((ICommunicationObject) channel).Close();
            routerHost.Close();
            serviceHost.Close();
        }
    }
}
