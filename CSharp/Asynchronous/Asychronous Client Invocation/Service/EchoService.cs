using System;
using System.Threading;

namespace Asynchronous.Service
{
    class EchoService : IEchoService
    {
        public string Echo(string data)
        {
            Console.WriteLine("Service: \tEcho (Data: {0})", data);
            Thread.Sleep(1000);
            return data;
        }
    }
}