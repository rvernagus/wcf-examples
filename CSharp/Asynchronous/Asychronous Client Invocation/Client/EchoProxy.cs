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

        public IAsyncResult BeginEcho(string data, AsyncCallback callback, object state)
        {
            Console.WriteLine("Client: \tBeginEcho (Data: {0}, State: {1})", data, state);
            return Channel.BeginEcho(data, callback, state);
        }

        public string EndEcho(IAsyncResult result)
        {
            Console.WriteLine("Client: \tEndEcho (State: {0})", result.AsyncState);
            return Channel.EndEcho(result);
        }
    }
}