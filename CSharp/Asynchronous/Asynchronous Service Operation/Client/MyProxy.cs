using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Asynchronous.Client
{
    class MyProxy : DuplexClientBase<IMyService>, IMyService
    {
        public MyProxy(InstanceContext callbackInstance, Binding binding, string uri)
            : base(callbackInstance, binding, new EndpointAddress(uri))
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