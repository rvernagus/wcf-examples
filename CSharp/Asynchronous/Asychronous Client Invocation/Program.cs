using System;
using System.ServiceModel;
using System.Threading;
using Asynchronous.Client;

// http://msdn.microsoft.com/en-us/library/ms734701.aspx
namespace Asynchronous
{
    class Program
    {
        static void Main()
        {
            const string uri = "net.pipe://localhost";
            var binding = new NetNamedPipeBinding();

            // Synchronous Service
            var host = new ServiceHost(typeof(Service.EchoService),new Uri(uri));
            host.AddServiceEndpoint(typeof (Service.IEchoService), binding, uri);
            host.Open();

            // Asynchronous Client
            var proxy = new EchoProxy(binding, uri);
            var asyncResult = proxy.BeginEcho("data", r => Console.WriteLine("Client: \tAsyncCallback (State: {0})", r.AsyncState), "state");
            var result = proxy.EndEcho(asyncResult);
            Console.WriteLine("Client: \tResult = {0}", result);
            proxy.Close();

            host.Close();
        }
    }
}
