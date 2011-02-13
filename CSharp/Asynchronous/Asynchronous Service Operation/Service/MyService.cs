using System;

namespace Asynchronous.Service
{
    class MyService : IMyService
    {
        public void MakeCall(string data)
        {
            Console.WriteLine("Service: MakeCall (Data: {0})", data);
        }

        public IAsyncResult BeginMakeCall(string data, AsyncCallback callback, object state)
        {
            Console.WriteLine("Service: BeginMakeCall (Data: {0})", data);
            return new MyAsyncResult<string>(data, state);
        }

        public string EndMakeCall(IAsyncResult result)
        {
            var completedAsyncResult = ((MyAsyncResult<string>)result);
            Console.WriteLine("Service: EndMakeCall (Data: {0})", completedAsyncResult.Data);
            return completedAsyncResult.Data;
        }
    }
}