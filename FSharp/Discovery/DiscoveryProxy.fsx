#r "System.Runtime.Serialization"
#r "System.ServiceModel"
#r "System.ServiceModel.Discovery"
open System
open System.Collections.Generic
open System.ServiceModel
open System.ServiceModel.Description
open System.ServiceModel.Discovery
open System.Runtime.Remoting.Messaging
open System.Threading

[<ServiceContract>]
type IEcho =
    [<OperationContract>]
    abstract Echo : string -> string


type Echo() =
    interface IEcho with
        member this.Echo(s) =
            sprintf "You said, \"%s\"" s


[<AbstractClass>]
type AsyncResult(callback : AsyncCallback, state : obj) =
    let thisLock = new obj()
    let mutable completedSynchronously' = false
    let mutable endCalled = false
    let mutable exception' : Exception = null
    let mutable isCompleted = false
    let mutable manualResetEvent : ManualResetEvent = null

    static member End<'T when 'T :> AsyncResult and 'T : equality>(result : IAsyncResult) =
        let asyncResult = result :?> 'T

        if asyncResult.EndCalled then
            raise <| new InvalidOperationException("Async object already ended.")

        asyncResult.EndCalled <- true

        if not asyncResult.IsCompleted then
            (asyncResult :> IAsyncResult).AsyncWaitHandle.WaitOne() |> ignore

        if asyncResult.ManualResetEvent <> null then
            (asyncResult.ManualResetEvent : ManualResetEvent).Close()

        if asyncResult.Exception <> null then
            raise asyncResult.Exception

        asyncResult

    member this.EndCalled = endCalled
    member this.EndCalled with set v = endCalled <- v

    member this.Exception = exception'
    member this.Exception with set v = exception' <- v

    member this.IsCompleted = isCompleted
    member this.IsCompleted with set v = isCompleted <- v

    member this.ManualResetEvent = manualResetEvent
    member this.ManualResetEvent with set v = manualResetEvent <- v


    member this.Complete(completedSynchronously) =
        if isCompleted then
            raise <| new InvalidOperationException("This async result is already completed.")

        completedSynchronously' <- completedSynchronously

        if completedSynchronously then
            isCompleted <- true
        else
            lock thisLock (fun () ->
                isCompleted <- true
                if manualResetEvent <> null then
                    manualResetEvent.Set() |> ignore)

        if callback <> null then
            callback.Invoke(this)

    member this.Complete(completedSynchronously, ex) =
        exception' <- ex
        this.Complete(completedSynchronously)

    interface IAsyncResult with
        member this.AsyncState = state

        member this.AsyncWaitHandle
            with get () =
                if manualResetEvent = null then
                    lock thisLock (fun () ->
                        if manualResetEvent = null then
                            manualResetEvent <- new ManualResetEvent(isCompleted))

                manualResetEvent :> WaitHandle

        member this.CompletedSynchronously = completedSynchronously'

        member this.IsCompleted = isCompleted


type OnOnlineAnnouncementAsyncResult(callback, state) as this =
    inherit AsyncResult(callback, state)

    do this.Complete(true)

    static member End(result) =
        AsyncResult.End<OnOnlineAnnouncementAsyncResult>(result)


type OnOfflineAnnouncementAsyncResult(callback, state) as this =
    inherit AsyncResult(callback, state)
    
    do this.Complete(true)

    static member End(result) =
        AsyncResult.End<OnOfflineAnnouncementAsyncResult>(result)


type OnFindAsyncResult(callback, state) as this =
    inherit AsyncResult(callback, state)

    do this.Complete(true)
    
    static member End(result) =
        AsyncResult.End<OnFindAsyncResult>(result)

    
type OnResolveAsyncResult(matchingEndpoint : EndpointDiscoveryMetadata, callback, state) as this =
    inherit AsyncResult(callback, state)

    do this.Complete(true)

    member this.MatchingEndpoint = matchingEndpoint
    
    static member End(result) =
        let thisPtr = AsyncResult.End<OnResolveAsyncResult>(result)
        thisPtr.MatchingEndpoint;


[<ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)>]
type DiscoveryProxyService() =
    inherit DiscoveryProxy()

    // Repository to store EndpointDiscoveryMetadata. A database or a flat file could also be used instead.
    let onlineServices = new Dictionary<EndpointAddress, EndpointDiscoveryMetadata>()

    // OnBeginOnlineAnnouncement method is called when a Hello message is received by the Proxy
    override this.OnBeginOnlineAnnouncement(messageSequence, endpointDiscoveryMetadata, callback, state) =
        this.AddOnlineService(endpointDiscoveryMetadata)
        new OnOnlineAnnouncementAsyncResult(callback, state) :> IAsyncResult

    override this.OnEndOnlineAnnouncement(result) =
        OnOnlineAnnouncementAsyncResult.End(result) |> ignore

    // OnBeginOfflineAnnouncement method is called when a Bye message is received by the Proxy
    override this.OnBeginOfflineAnnouncement(messageSequence, endpointDiscoveryMetadata, callback, state) =
        this.RemoveOnlineService(endpointDiscoveryMetadata)
        new OnOfflineAnnouncementAsyncResult(callback, state) :> IAsyncResult

    override this.OnEndOfflineAnnouncement(result) =
        OnOfflineAnnouncementAsyncResult.End(result) |> ignore

    // OnBeginFind method is called when a Probe request message is received by the Proxy
    override this.OnBeginFind(findRequestContext, callback, state) =
        this.MatchFromOnlineService(findRequestContext)
        new OnFindAsyncResult(callback, state) :> IAsyncResult

    override this.OnEndFind(result) =
        OnFindAsyncResult.End(result) |> ignore

    // OnBeginFind method is called when a Resolve request message is received by the Proxy
    override this.OnBeginResolve(resolveCriteria, callback, state) =
        new OnResolveAsyncResult(this.MatchFromOnlineService(resolveCriteria), callback, state) :> IAsyncResult

    override this.OnEndResolve(result) =
        OnResolveAsyncResult.End(result)

    // The following are helper methods required by the Proxy implementation
    member this.AddOnlineService(endpointDiscoveryMetadata : EndpointDiscoveryMetadata) =
        lock onlineServices (fun () -> 
            onlineServices.[endpointDiscoveryMetadata.Address] <- endpointDiscoveryMetadata)

        this.PrintDiscoveryMetadata(endpointDiscoveryMetadata, "Adding")

    member this.RemoveOnlineService(endpointDiscoveryMetadata : EndpointDiscoveryMetadata) =
        if endpointDiscoveryMetadata <> null then
            lock onlineServices (fun () ->
                onlineServices.Remove(endpointDiscoveryMetadata.Address) |> ignore)

            this.PrintDiscoveryMetadata(endpointDiscoveryMetadata, "Removing")

    member this.MatchFromOnlineService(findRequestContext : FindRequestContext) =
        lock onlineServices (fun () ->
            for endpointDiscoveryMetadata in onlineServices.Values do
                if findRequestContext.Criteria.IsMatch(endpointDiscoveryMetadata) then
                    findRequestContext.AddMatchingEndpoint(endpointDiscoveryMetadata))

    member this.MatchFromOnlineService(criteria : ResolveCriteria) =
        let matchingEndpoint = ref (new EndpointDiscoveryMetadata())

        lock onlineServices (fun () ->
            for endpointDiscoveryMetadata in onlineServices.Values do
                if criteria.Address = endpointDiscoveryMetadata.Address then
                    matchingEndpoint := endpointDiscoveryMetadata)
        
        !matchingEndpoint

    member this.PrintDiscoveryMetadata(endpointDiscoveryMetadata, verb) =
        printfn "\n**** %s service of the following type from cache. " verb
        for contractName in endpointDiscoveryMetadata.ContractTypeNames do
            printfn "** %O" contractName

        printfn "**** Operation Completed"


let rand() =
    (new Random()).Next(7000, 9999)

let guid() =
    Guid.NewGuid()

let address = sprintf "net.tcp://localhost:%O/%O" <| rand() <| guid()
let host = new ServiceHost(typeof<Echo>, new Uri(address))
host.AddDefaultEndpoints()
// Make Discoverable
host.Description.Behaviors.Add(new ServiceDiscoveryBehavior())
host.Description.Endpoints.Add(new UdpDiscoveryEndpoint())
host.Open()
printfn "Service listening on %O\n" address

let echoEndpoint = 
    new DynamicEndpoint(
        ContractDescription.GetContract(typeof<IEcho>),
        new NetTcpBinding())
let channelFactory = new ChannelFactory<IEcho>(echoEndpoint)
let echoChannel = channelFactory.CreateChannel()
printfn "Service found at %O\n" <| echoEndpoint.Address
printfn "%s" <| echoChannel.Echo("Discovery!")

(echoChannel :?> ICommunicationObject).Close()
host.Close()



