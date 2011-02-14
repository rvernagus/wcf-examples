using System;
using System.ServiceModel;

namespace Asynchronous.Client
{
    [ServiceContract(CallbackContract = typeof(IMyCallback))]
    internal interface IMyService
    {
        [OperationContract(IsOneWay = true)]
        void MakeCall(string data);
    }
}