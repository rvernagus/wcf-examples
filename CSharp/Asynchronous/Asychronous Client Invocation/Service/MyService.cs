using System;

namespace Asynchronous.ClientSide.Service
{
    class MyService : IMyService
    {
        public void MakeCall(string data)
        {
            Console.WriteLine("Service: Call received (Data: {0})", data);
        }
    }
}