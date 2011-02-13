using System;

namespace Asynchronous.ClientSide.Service
{
    class MyService : IMyService
    {
        public void MakeCall()
        {
            Console.WriteLine("Inside Call");
        }
    }
}