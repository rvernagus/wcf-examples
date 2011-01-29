using System.ServiceModel;
using System.ServiceModel.Web;

namespace Client
{
    [ServiceContract]
    public interface IRestContract
    {
        [OperationContract]
        [WebGet]
        string GetValue(string value);

        [OperationContract]
        [WebInvoke(Method = "POST")]
        string PostValue(string value);
    }
}