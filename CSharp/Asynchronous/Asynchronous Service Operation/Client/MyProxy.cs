using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Asynchronous.Client
{
    class MyProxy : ClientBase<IMyService>, IMyService
    {
        public MyProxy(Binding binding, string uri)
            : base(binding, new EndpointAddress(uri))
        {}

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