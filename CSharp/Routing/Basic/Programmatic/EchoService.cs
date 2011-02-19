using System;
using System.ServiceModel;

namespace Routing
{
    class EchoService : IEchoService
    {
        public string Echo(string text)
        {
            Console.WriteLine("Service:\tEcho operation call");
            return text;
        }
    }
}