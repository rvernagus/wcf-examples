using System.ServiceModel;

namespace Example
{
    [ServiceContract]
    internal interface IMyService
    {
        [OperationContract]
        string GetData(string data);
    }
}