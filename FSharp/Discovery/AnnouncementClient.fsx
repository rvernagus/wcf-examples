#r "System.Runtime.Serialization"
#r "System.ServiceModel"
#r "System.ServiceModel.Discovery"
open System
open System.ServiceModel
open System.ServiceModel.Discovery
open System.Threading


[<ServiceContract>]
type IMyContract =
    [<OperationContract>]
    abstract Operation : unit -> unit


type MyService() =
    interface IMyContract with
        member this.Operation() = ()


let ans = new AnnouncementService()
ans.OnlineAnnouncementReceived.Add(fun e -> printfn "Received Online Announcement from <%A>" e.EndpointDiscoveryMetadata.Address)
ans.OfflineAnnouncementReceived.Add(fun e -> printfn "Received Offline Announcement from <%A>" e.EndpointDiscoveryMetadata.Address)

let ansHost = new ServiceHost(ans, new Uri("net.tcp://localhost/ansHost"))
ansHost.AddServiceEndpoint(new UdpAnnouncementEndpoint())
ansHost.Open()

let host = new ServiceHost(typeof<MyService>, new Uri("net.tcp://localhost/host"))
let sdb = new ServiceDiscoveryBehavior()
sdb.AnnouncementEndpoints.Add(new UdpAnnouncementEndpoint())
host.Description.Behaviors.Add(sdb)
host.Open()


let client = new AnnouncementClient(new UdpAnnouncementEndpoint())
client.Open()
let metadata = new EndpointDiscoveryMetadata()
metadata.Address <- new EndpointAddress("http://this.is.my.announcementClient")
client.AnnounceOnline(metadata)
client.AnnounceOffline(metadata)

host.Close()
ansHost.Close()
