#r "System.ServiceModel"
#r "System.Runtime.Serialization"
open System
open System.ServiceModel
open System.ServiceModel.Channels
open System.ServiceModel.Description


[<ServiceContract>]
type IMyContract =
    [<OperationContract(IsOneWay = true)>]
    abstract MethodWithError : unit -> unit
    
    [<OperationContract>]
    abstract MethodWithoutError : unit -> unit

[<ServiceBehavior(IncludeExceptionDetailInFaults = true)>]
type MyService() =
    
    interface IMyContract with
        member this.MethodWithError() = failwith "BOOM!"
        
        member this.MethodWithoutError() = printfn "MyService.MethodWithoutError()"


let host = new ServiceHost(typeof<MyService>, new Uri("net.tcp://localhost"))
host.Open()

let proxy = ChannelFactory<IMyContract>.CreateChannel(host.Description.Endpoints.[0].Binding, host.Description.Endpoints.[0].Address)

// Run with and without following line
proxy.MethodWithError()
try
    // will error because channel is at fault
    proxy.MethodWithoutError()    
with ex -> printfn "\n%s\n" ex.Message

printfn "%A" host.State

try
    (proxy :?> ICommunicationObject).Close()
with _ -> ()
host.Close()
