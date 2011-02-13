using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Asynchronous.Client
{
    class MyProxy : DuplexClientBase<IMyService>, IMyService
    {
        public MyProxy(IMyCallback callbackInstance, Binding binding, string uri)
            : base(callbackInstance, binding, new EndpointAddress(uri))
        {}

        public void MakeCall(string data)
        {
            Channel.MakeCall(data);
        }
    }
}