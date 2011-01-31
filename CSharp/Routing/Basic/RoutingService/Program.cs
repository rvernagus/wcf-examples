using System;
using System.ServiceModel;
using System.ServiceModel.Routing;

namespace RoutingServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof (RoutingService)))
            {
                try
                {
                    host.Open();
                    Console.WriteLine("The Routing Service is running.");
                    Console.WriteLine("Press <ENTER> to quit.");
                    Console.ReadLine();
                    host.Close();
                }
                catch (CommunicationException)
                {
                    host.Abort();
                    throw;
                }
            }
        }
    }
}