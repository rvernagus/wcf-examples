using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Asynchronous.Client
{
    class EchoProxy : DuplexClientBase<IEchoService>, IEchoService
    {
        public EchoProxy(InstanceContext callbackInstance, Binding binding, string uri)
            : base(callbackInstance, binding, new EndpointAddress(uri))
        { }

        public void Echo(string data)
        {
            Console.WriteLine("Client: \tEcho (Data: {0})", data);
            Channel.Echo(data);
        }
    }
}