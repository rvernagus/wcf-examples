using System;
using System.ServiceModel;

namespace Asynchronous.Service
{
    [ServiceContract(CallbackContract = typeof(IMyCallback), SessionMode = SessionMode.Required)]
    internal interface IMyService
    {
        [OperationContract(IsOneWay = true)]
        void MakeCall(string data);
    }
}