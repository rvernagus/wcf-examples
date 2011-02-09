using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Example
{
    public abstract class MessageInspector : IDispatchMessageInspector, IClientMessageInspector
    {
        #region Implementation of IDispatchMessageInspector

        public abstract object AfterReceiveRequest(ref Message request, IClientChannel channel,
                                                   InstanceContext instanceContext);

        public abstract void BeforeSendReply(ref Message reply, object correlationState);

        #endregion

        #region Implementation of IClientMessageInspector

        public abstract object BeforeSendRequest(ref Message request, IClientChannel channel);
        public abstract void AfterReceiveReply(ref Message reply, object correlationState);

        #endregion
    }
}