using System;
using System.ServiceModel;

namespace Asynchronous.Service
{
    [ServiceContract]
    internal interface IEchoService
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginEcho(string data, AsyncCallback callback, object state);

        string EndEcho(IAsyncResult result);
    }
}