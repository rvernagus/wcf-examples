using System;
using System.Threading;

namespace Asynchronous.Client
{
    class CallbackHandler : IEchoCallback
    {
        private readonly EventWaitHandle _callbackEvent;

        public CallbackHandler(EventWaitHandle callbackEvent)
        {
            _callbackEvent = callbackEvent;
        }

        public void EchoComplete(string data)
        {
            Console.WriteLine("Client: \tEchoComplete (Data: {0})", data);
            _callbackEvent.Set();
        }
    }
}