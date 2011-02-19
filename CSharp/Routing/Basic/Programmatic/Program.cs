using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Routing;

namespace Routing
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Echo Service
            const string serviceAddress = "net.pipe://localhost/echoService";
            var serviceBinding = new NetNamedPipeBinding();
            var serviceContract = ContractDescription.GetContract(typeof(IEchoService));
            var serviceEndpoint = new ServiceEndpoint(serviceContract, serviceBinding,
                                                      new EndpointAddress(serviceAddress));
            var serviceHost = new ServiceHost(typeof(EchoService));
            serviceHost.AddServiceEndpoint(serviceEndpoint);
            serviceHost.Open();
            Console.WriteLine("Service:\tOpen '{0}'", serviceEndpoint.Address);


            // Routing Service
            const string routerAddress = "net.pipe://localhost/routingService";
            var routerBinding = new NetNamedPipeBinding();
            var routerContract = ContractDescription.GetContract(typeof(IRequestReplyRouter));
            var routerEndpoint = new ServiceEndpoint(routerContract, routerBinding, new EndpointAddress(routerAddress));
            var routerHost = new ServiceHost(typeof(RoutingService));
            routerHost.AddServiceEndpoint(routerEndpoint);
            var routingConfiguration = new RoutingConfiguration();
            var routerToService = serviceEndpoint;
            routingConfiguration.FilterTable.Add(new MatchAllMessageFilter(), new[] { routerToService });
            routerHost.Description.Behaviors.Add(new RoutingBehavior(routingConfiguration));
            routerHost.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;
            routerHost.Open();
            Console.WriteLine("Router: \tOpen '{0}'", routerEndpoint.Address);

            // Client
            const string clientAddress = routerAddress;
            var clientBinding = routerBinding;
            var clientContract = serviceContract;
            var clientEndpoint = new ServiceEndpoint(clientContract, clientBinding, new EndpointAddress(clientAddress));
            var factory = new ChannelFactory<IEchoService>(clientEndpoint);
            Console.WriteLine("Client: \tCreating channel '{0}'", clientEndpoint.Address);
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