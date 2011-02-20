using System;
using System.ServiceModel;

namespace Asynchronous.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    class EchoService : IEchoService
    {
        public void Echo(string data)
        {
            Console.WriteLine("Service: \tEcho (Data: {0})", data);

            var callback = OperationContext.Current.GetCallbackChannel<IEchoCallback>();
            callback.EchoComplete(data);
        }
    }
}