using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Asynchronous.ClientSide.Client
{
    class MyProxy : ClientBase<IMyService>, IMyService
    {
        public MyProxy(Binding binding, string uri)
            : base(binding, new EndpointAddress(uri))
        {}

        public void MakeCall(string data)
        {
            Channel.MakeCall(data);
        }

        public IAsyncResult BeginMakeCall(string data, AsyncCallback callback, object state)
        {
            return Channel.BeginMakeCall(data, callback, state);
        }

        public void EndMakeCall(IAsyncResult result)
        {
            Channel.EndMakeCall(result);
        }
    }
}