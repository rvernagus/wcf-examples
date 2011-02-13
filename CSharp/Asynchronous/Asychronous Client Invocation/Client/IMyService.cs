using System;
using System.ServiceModel;

namespace Asynchronous.ClientSide.Client
{

    [ServiceContract]
    internal interface IMyService
    {
        [OperationContract]
        void MakeCall(string data);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginMakeCall(string data, AsyncCallback callback, object state);

        // Note: No [OperationContract] needed
        void EndMakeCall(IAsyncResult result);
    }
}