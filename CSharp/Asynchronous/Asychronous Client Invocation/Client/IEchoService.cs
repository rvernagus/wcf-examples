using System;
using System.ServiceModel;

namespace Asynchronous.Client
{
    [ServiceContract]
    internal interface IEchoService
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginEcho(string data, AsyncCallback callback, object state);

        string EndEcho(IAsyncResult result);
    }
}