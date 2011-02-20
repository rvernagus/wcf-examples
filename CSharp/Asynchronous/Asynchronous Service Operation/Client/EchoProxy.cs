using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Asynchronous.Client
{
    class EchoProxy : ClientBase<IEchoService>, IEchoService
    {
        public EchoProxy(Binding binding, string uri)
            : base(binding, new EndpointAddress(uri))
        {}

        public string Echo(string data)
        {
            Console.WriteLine("Client: \tEcho (Data: {0})", data);
            return Channel.Echo(data);
        }
    }
}