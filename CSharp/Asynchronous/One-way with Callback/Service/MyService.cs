using System;
using System.ServiceModel;

namespace Asynchronous.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    class MyService : IMyService
    {
        public void MakeCall(string data)
        {
            Console.WriteLine("Service: MakeCall (Data: {0})", data);

            var callback = OperationContext.Current.GetCallbackChannel<IMyCallback>();
            callback.MakeCallComplete();
        }
    }
}