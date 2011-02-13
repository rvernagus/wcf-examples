using System;

namespace Asynchronous.Client
{
    class CallbackHandler : IMyCallback
    {
        public void MakeCallComplete()
        {
            Console.WriteLine("Client: MakeCallComplete");
        }
    }
}