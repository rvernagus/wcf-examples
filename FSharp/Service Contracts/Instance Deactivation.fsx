#r "System.ServiceModel"
#r "System.Runtime.Serialization"
open System
open System.Diagnostics
open System.ServiceModel
open System.ServiceModel.Channels


[<ServiceContract(SessionMode = SessionMode.Required)>]
type IMyContract =
    [<OperationContract>]
    abstract MyMethod : unit -> unit
    
    [<OperationContract>]
    abstract MyOtherMethod : unit -> unit


[<ServiceBehavior>]
type MyService() =
    interface IMyContract with
        // Try this example with none or one of the following lines
//        [<OperationBehavior(ReleaseInstanceMode = ReleaseInstanceMode.BeforeCall)>]
//        [<OperationBehavior(ReleaseInstanceMode = ReleaseInstanceMode.AfterCall)>]
//        [<OperationBehavior(ReleaseInstanceMode = ReleaseInstanceMode.BeforeAndAfterCall)>]
        member this.MyMethod() = printfn "proxy.MyMethod()"; System.Threading.Thread.Sleep(100)
        
        member this.MyOtherMethod() = printfn "proxy.MyOtherMethod()"; System.Threading.Thread.Sleep(100)

    interface IDisposable with
        member this.Dispose() =
            printfn "Disposing..."


let host = new ServiceHost(typeof<MyService>, new Uri("net.tcp://localhost"))
host.Open()

let proxy = ChannelFactory<IMyContract>.CreateChannel(host.Description.Endpoints.[0].Binding, host.Description.Endpoints.[0].Address)

proxy.MyOtherMethod()
proxy.MyMethod()
System.Threading.Thread.Sleep(100)
printfn "-----------------"

(proxy :?> ICommunicationObject).Close()
host.Close()
