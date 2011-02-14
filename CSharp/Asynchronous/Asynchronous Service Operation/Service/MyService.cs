using System;
using System.Threading;

namespace Asynchronous.Service
{
    class MyService : IMyService
    {
        public IAsyncResult BeginMakeCall(string data, AsyncCallback callback, object state)
        {
            Console.WriteLine("Service: BeginMakeCall (Data: {0}, Thread: {1})", data, Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Service: Sleeping...");
            Thread.Sleep(100);
            return new CallAsyncResult<string>(data, callback, state);
        }

        public void EndMakeCall(IAsyncResult result)
        {
            CallAsyncResult<string>.End(result);
        }
    }

    internal class CallAsyncResult<T> : AsyncResult
    {
        public T Data { get; set; }

        public CallAsyncResult(T data, AsyncCallback callback, object state)
            : base(callback, state)
        {
            Data = data;
        }

        public static T End(IAsyncResult result)
        {
            var typedResult = End<TypedAsyncResult<T>>(result);
            return typedResult.Data;
        }
    }
}