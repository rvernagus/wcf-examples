using System;
using System.ServiceModel;

namespace Asynchronous.Client
{
    [ServiceContract(CallbackContract = typeof(IEchoCallback))]
    internal interface IEchoService
    {
        [OperationContract(IsOneWay = true)]
        void Echo(string data);
    }
}