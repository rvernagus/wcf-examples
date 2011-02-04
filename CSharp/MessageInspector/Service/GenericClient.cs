using System.ServiceModel;
using System.ServiceModel.Channels;

namespace MessageInspector
{
    internal class GenericClient<T> : ClientBase<T>
            where T : class
    {
        public GenericClient(Binding binding, string address)
                : base(binding, new EndpointAddress(address))
        {
        }

        public T Service
        {
            get { return Channel; }
        }
    }
}