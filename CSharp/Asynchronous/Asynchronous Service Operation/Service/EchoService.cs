using System;
using System.Threading;

namespace Asynchronous.Service
{
    internal class EchoService : IEchoService
    {
        public IAsyncResult BeginEcho(string data, AsyncCallback callback, object state)
        {
            Console.WriteLine("Service: \tBeginMakeCall (Data: {0}, State: {1}, Thread: {2})", data, state, Thread.CurrentThread.ManagedThreadId);
            return new EchoResult<string>(data, state);
        }

        public string EndEcho(IAsyncResult result)
        {
            Console.WriteLine("Service: \tEndMakeCall (State: {0}, Thread: {1})", result.AsyncState, Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(1000);
            return ((EchoResult<string>)result).Data;
        }
    }
}