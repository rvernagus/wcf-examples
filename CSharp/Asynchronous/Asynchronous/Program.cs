using System;
using System.ServiceModel;
using Asynchronous.ClientSide.Service;

// http://msdn.microsoft.com/en-us/library/ms734701.aspx
namespace Asynchronous.ClientSide
{
    class Program
    {
        static void Main(string[] args)
        {
            const string baseUri = "net.tcp://localhost:4567";
            var host = new ServiceHost(typeof(MyService),new Uri(baseUri));
            host.AddDefaultEndpoints();
            host.Open();
            var channel = ChannelFactory<IMyService>.CreateChannel(new NetTcpBinding(), new EndpointAddress(baseUri));
            channel.MakeCall();
            host.Close();
        }
    }
}
