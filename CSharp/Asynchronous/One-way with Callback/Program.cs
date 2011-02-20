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

            // Duplex Service
            var host = new ServiceHost(typeof(Service.EchoService), new Uri(uri));
            host.AddServiceEndpoint(typeof(Service.IEchoService), binding, uri);
            host.Open();

            // Client with callback
            var callbackEvent = new AutoResetEvent(false);
            var callback = new CallbackHandler(callbackEvent);
            var proxy = new EchoProxy(new InstanceContext(callback), binding, uri);
            proxy.Echo("data");
            callbackEvent.WaitOne();
            proxy.Close();

            host.Close();
        }
    }
}
