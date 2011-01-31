using System;
using System.ServiceModel;

namespace Client
{
    [ServiceContract]
    interface IService1Contract
    {
        [OperationContract]
        string GetData(int value);
    }
}
