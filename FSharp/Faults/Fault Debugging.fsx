#r "System.ServiceModel"
#r "System.Runtime.Serialization"
open System
open System.ServiceModel


[<ServiceContract>]
type IMyContract =
    [<OperationContract>]
    abstract MethodWithError : unit -> unit


type MyService() =
    interface IMyContract with
        member this.MethodWithError() =
            let ex = new InvalidOperationException("Some error")
            let detail = new ExceptionDetail(ex)
            let fault = new FaultException<ExceptionDetail>(detail, ex.Message)
            raise fault


let host = new ServiceHost(typeof<MyService>, new Uri("net.tcp://localhost"))
host.Open()

let proxy = ChannelFactory<IMyContract>.CreateChannel(host.Description.Endpoints.[0].Binding, host.Description.Endpoints.[0].Address)
try
    proxy.MethodWithError()
with
    | :? FaultException<ExceptionDetail> as ex ->
        printfn "%s: %s" ex.Detail.Type ex.Detail.Message

(proxy :?> ICommunicationObject).Close()
host.Close()