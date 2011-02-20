using System;
using System.ServiceModel;
using System.Threading;
using Asynchronous.Client;

namespace Asynchronous
{
    class Program
    {
        static void Main(string[] args)
        {
            const string uri = "net.pipe://localhost";
            var binding = new NetNamedPipeBinding();

            // Asynchronous Service
            var host = new ServiceHost(typeof(Service.EchoService), new Uri(uri));
            host.AddServiceEndpoint(typeof(Service.IEchoService), binding, uri);
            host.Open();

            // Synchronous Client
            var proxy = new EchoProxy(binding, uri);
            var result = proxy.Echo("data");
            Console.WriteLine("Client: \tResult = {0}", result);
            proxy.Close();

            host.Close();
        }
    }
}
