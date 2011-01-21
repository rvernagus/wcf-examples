using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Example
{
    [DataContract]
    internal class Account
    {
        [DataMember]
        public int Number { get; set; }
    }

    [ServiceContract]
    internal interface IService
    {
        [OperationContract]
        [WebGet]
        Account GetAccount(int accountNumber);

        [OperationContract]
        [WebInvoke]
        void SaveAccount(Account account);
    }

    internal class Service : IService
    {
        #region IService Members

        public Account GetAccount(int accountNumber)
        {
            return new Account { Number = accountNumber };
        }

        public void SaveAccount(Account account)
        {
            Assert.IsInstanceOfType(account, typeof(Account));
        }

        #endregion
    }

    [TestClass]
    public class RestfulService
    {
        [TestMethod]
        public void TestMethod1()
        {
            const string address = "http://localhost:8000";
            var host = new WebServiceHost(typeof(Service), new Uri(address));
            host.AddServiceEndpoint(typeof(IService), new WebHttpBinding(), "");
            host.Open();

            var factory = new ChannelFactory<IService>(new WebHttpBinding(), address);
            factory.Endpoint.Behaviors.Add(new WebHttpBehavior());

            var service = factory.CreateChannel();

            var account = service.GetAccount(1234);
            Assert.AreEqual(1234, account.Number);
            service.SaveAccount(account);

            var proxy = (ICommunicationObject)service;
            try
            {
                proxy.Close();
            }
            catch (Exception)
            {
                proxy.Abort();
            }

            host.Close();
        }
    }
}