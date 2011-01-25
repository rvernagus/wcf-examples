#r "System.ServiceModel"
#r "System.Runtime.Serialization"
open System
open System.ServiceModel
open System.ServiceModel.Channels
open System.Runtime.Serialization


type MyCustomFault(msg : string) =
    inherit FaultException(msg)


[<ServiceContract>]
type IMyContract =
    [<OperationContract>]
    abstract MyMethod : unit -> unit
    

type MyService() =
    interface IMyContract with
        member this.MyMethod() =
            raise (new MyCustomFault("Error Message"))
            

let host = new ServiceHost(typeof<MyService>, new Uri("net.tcp://localhost"))
host.Open()

let proxy = ChannelFactory<IMyContract>.CreateChannel(host.Description.Endpoints.[0].Binding, host.Description.Endpoints.[0].Address)

try
    proxy.MyMethod()
with :? FaultException as ex -> 
    printfn "%A: %s" (ex.GetType()) ex.Message
    printfn "  Action: %s" ex.Action
    printfn "  Code:   %s" ex.Code.Name
    printfn "  Reason: %A" ex.Reason

printfn "Host State: %A" host.State
printfn "Client State: %A" (proxy :?> ICommunicationObject).State

(proxy :?> ICommunicationObject).Close()
host.Close()

