#r "System.ServiceModel"
#r "System.Runtime.Serialization"
open System
open System.ServiceModel
open System.ServiceModel.Description
open System.Threading
open System.Threading.Tasks


[<ServiceContract(SessionMode = SessionMode.Required)>]
type IMyContract =
    [<OperationContract>]
    abstract MyMethod : unit -> unit


type MyService() =
    interface IMyContract with
        member this.MyMethod() = printfn "MyService.MyMethod(): %s" (DateTime.Now.ToLongTimeString())


let host = new ServiceHost(typeof<MyService>, new Uri("net.tcp://localhost"))

let throttle = new ServiceThrottlingBehavior(MaxConcurrentSessions = 1)
// Comment the following line to turn throttling off
host.Description.Behaviors.Add(throttle)

host.Open()

let proxy1 = ChannelFactory<IMyContract>.CreateChannel(host.Description.Endpoints.[0].Binding, host.Description.Endpoints.[0].Address)
let task1 = Task.Factory.StartNew(fun () ->
    proxy1.MyMethod()
    // When thread sleeps, task2 waits on task1 when throttling is enabled
    Thread.Sleep(6000)
    (proxy1 :?> ICommunicationObject).Close())

let proxy2 = ChannelFactory<IMyContract>.CreateChannel(host.Description.Endpoints.[0].Binding, host.Description.Endpoints.[0].Address)
let task2 = Task.Factory.StartNew(fun () ->
    try
        proxy2.MyMethod()
        (proxy2 :?> ICommunicationObject).Close()
    with ex -> printfn "\n%s\n" ex.Message)

// When throttling is enabled, the second call should be 6 seconds
// behind the first call
Task.WaitAll(task1, task2)

host.Close()
