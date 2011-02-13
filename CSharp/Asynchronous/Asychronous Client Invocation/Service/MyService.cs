using System;

namespace Asynchronous.Service
{
    class MyService : IMyService
    {
        public void MakeCall(string data)
        {
            Console.WriteLine("Service: Call received (Data: {0})", data);
        }
    }
}