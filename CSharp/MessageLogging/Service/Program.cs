using System;
using System.ServiceModel;

namespace Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(MyService)))
            {
                host.Open();

                var binding = new NetTcpBinding(SecurityMode.Message);
                const string address = "net.tcp://localhost:4500/";
                var client = new GenericClient<IMyService>(binding, address);
                var data = client.Service.GetData("test data");
                Console.WriteLine(data);
                (client as ICommunicationObject).Close();
                host.Close();
            }
        }
    }
}