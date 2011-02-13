using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace REST.Json
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json)]
        string GetData(int value);

        [OperationContract]
        [WebInvoke(Method="GET", RequestFormat = WebMessageFormat.Json)]
        CompositeType GetDataUsingDataContract(bool boolValue);
    }


    [DataContract]
    public class CompositeType
    {
        [DataMember]
        public bool BoolValue { get; set; }

        [DataMember]
        public string StringValue { get; set; }
    }
}