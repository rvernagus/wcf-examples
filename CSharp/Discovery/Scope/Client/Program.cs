using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Discovery;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ServiceClient proxy;

            // Discover service programmatically
            var discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());
            var findCriteria = new FindCriteria(typeof(IServiceContract));
            //findCriteria.Scopes.Add(new Uri("http://localhost/Production"));
            var findResponse = discoveryClient.Find(findCriteria);
            Debug.Assert(findResponse.Endpoints.Count > 0);
            var address = findResponse.Endpoints[0].Address;
            proxy = new ServiceClient(new WSHttpBinding(), address);
            proxy.Open();
            var environment = proxy.GetEnvironment();
            Debug.Assert(environment == "Production");
            proxy.Close();

            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}