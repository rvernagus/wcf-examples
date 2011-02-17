using System;
using System.ServiceModel;
using System.ServiceModel.Discovery;

namespace Discovery
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Client
            var announcementService = new AnnouncementService();
            announcementService.OnlineAnnouncementReceived += OnlineAnnouncementReceived;
            announcementService.OfflineAnnouncementReceived += OfflineAnnouncementReceived;
            var announcementHost = new ServiceHost(announcementService,
                                                   new Uri("net.tcp://localhost/client/announcements"));
            announcementHost.AddServiceEndpoint(new UdpAnnouncementEndpoint());
            announcementHost.Open();

            // Service
            var host = new ServiceHost(typeof(EchoService));
            host.Opening += (s, e) => Console.WriteLine("Host:\tOpening");
            host.Opened += (s, e) => Console.WriteLine("Host:\tOpened");
            host.Closing += (s, e) => Console.WriteLine("Host:\tClosing");
            host.Closed += (s, e) => Console.WriteLine("Host:\tClosed");
            host.Open();


            host.Close();
        }

        private static void OnlineAnnouncementReceived(object sender, AnnouncementEventArgs e)
        {
            var address = e.EndpointDiscoveryMetadata.Address;
            Console.WriteLine("Client:\tOnlineAnnouncementReceived ({0})", address);
            var factory = new ChannelFactory<IEchoService>("clientConfig");
            var channel = factory.CreateChannel(address);
            if (channel != null)
            {
                Console.WriteLine(channel.Echo("Client:\tchannel.Echo()"));
                ((ICommunicationObject) channel).Close();
            }
        }

        private static void OfflineAnnouncementReceived(object sender, AnnouncementEventArgs e)
        {
            Console.WriteLine("Client:\tOfflineAnnouncementReceived ({0})", e.EndpointDiscoveryMetadata.Address);
        }
    }
}