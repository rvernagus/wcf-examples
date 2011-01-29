using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var factory = new ChannelFactory<IRestContract>(new WebHttpBinding(), "http://localhost:8000"))
            {
                factory.Endpoint.Behaviors.Add(new WebHttpBehavior());

                var channel = factory.CreateChannel();

                var response = channel.GetValue("some value");
                Console.WriteLine(response);

                response = channel.PostValue("some value");
                Console.WriteLine(response);

                Console.WriteLine("Press enter to quit...");
                Console.ReadLine();
            }
        }
    }
}