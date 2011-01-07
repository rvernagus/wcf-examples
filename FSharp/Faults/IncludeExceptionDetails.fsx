#r "System.ServiceModel"
#r "System.Runtime.Serialization"
open System
open System.Runtime.Serialization
open System.ServiceModel
open System.ServiceModel.Channels


[<ServiceContract>]
type IMyContract =
    [<OperationContract>]
    abstract MethodWithError : unit -> unit


[<ServiceBehavior(IncludeExceptionDetailInFaults = true)>]
type MyService() =
    interface IMyContract with
        member this.MethodWithError() =
            raise (new InvalidOperationException("Some error"))


let host = new ServiceHost(typeof<MyService>, new Uri("net.tcp://localhost"))
host.Open()

let proxy = ChannelFactory<IMyContract>.CreateChannel(host.Description.Endpoints.[0].Binding, host.Description.Endpoints.[0].Address)
try
    proxy.MethodWithError()
with ex -> printfn "%s: %s\n%s" (ex.GetType().Name) ex.Message ""//ex.StackTrace

(proxy :?> ICommunicationObject).Abort()
host.Close()