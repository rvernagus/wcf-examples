using System;
using System.ServiceModel;
using System.Threading;
using Asynchronous.Client;

// http://msdn.microsoft.com/en-us/library/ms734701.aspx
namespace Asynchronous
{
    class Program
    {
        static void Main(string[] args)
        {
            const string uri = "net.pipe://localhost";
            var binding = new NetNamedPipeBinding();

            // Service
            var host = new ServiceHost(typeof(Service.MyService),new Uri(uri));
            host.AddServiceEndpoint(typeof (Service.IMyService), binding, uri);
            host.Open();

            // Client
            var proxy = new MyProxy(binding, uri);
            Console.WriteLine("Client: Making asynchronous call");
            proxy.BeginMakeCall("data", r => Console.WriteLine("Client: Callback made (State: {0})", r.AsyncState), "state");
            Console.WriteLine("Client: Waiting for callback");
            Thread.Sleep(500);
            proxy.Close();

            host.Close();
        }
    }
}
