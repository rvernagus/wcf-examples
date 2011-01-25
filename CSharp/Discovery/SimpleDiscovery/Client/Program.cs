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
            Service1Client proxy;

            // Discover service using dynamicEndpoint in configuration
            proxy = new Service1Client();
            proxy.Open();
            Debug.Assert(proxy.DoWork());
            proxy.Close();

            // Discover service programmatically
            var discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());
            var findCriteria = new FindCriteria(typeof(IService1));
            var findResponse = discoveryClient.Find(findCriteria);
            Debug.Assert(findResponse.Endpoints.Count > 0);
            var address = findResponse.Endpoints[0].Address;
            proxy = new Service1Client(new WSHttpBinding(), address);
            proxy.Open();
            Debug.Assert(proxy.DoWork());
            proxy.Close();

            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}