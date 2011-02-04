using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Xml;

namespace MessageInspector
{
    public class MessageInspector : IDispatchMessageInspector, IClientMessageInspector
    {
        #region IClientMessageInspector Members

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            Console.WriteLine("\nBeforeSendRequest");
            WriteMessage(ref request);
            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            Console.WriteLine("\nAfterReceiveReply");
            WriteMessage(ref reply);
        }

        #endregion

        #region IDispatchMessageInspector Members

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            Console.WriteLine("\nAfterReceiveRequest");
            WriteMessage(ref request);
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            Console.WriteLine("\nBeforeSendReply");
            WriteMessage(ref reply);
        }

        #endregion

        private static void WriteMessage(ref Message messageToWrite)
        {
            var messageBuffer = messageToWrite.CreateBufferedCopy(Int32.MaxValue);
            var unreadMessage = messageBuffer.CreateMessage();
            Console.WriteLine(messageToWrite.ToString());
            messageToWrite = unreadMessage;
        }
    }
}