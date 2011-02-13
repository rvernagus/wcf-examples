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

            // Service
            var host = new ServiceHost(typeof(Service.MyService), new Uri(uri));
            host.AddServiceEndpoint(typeof(Service.IMyService), binding, uri);
            host.Open();

            // Client
            var callback = new CallbackHandler();
            var proxy = new MyProxy(callback, binding, uri);
            Console.WriteLine("Client: Making asynchronous call");
            proxy.MakeCall("data");
            Console.WriteLine("Client: Waiting for callback");
            Thread.Sleep(500);
            proxy.Close();

            host.Close();
        }
    }
}
