#load @"..\Common.fsx"
#r "System.ServiceModel"
open Common
open System
open System.ServiceModel
open System.ServiceModel.Description
open System.ServiceModel.Dispatcher
Console.Clear()


[<ServiceContract>]
type IContract =
    [<OperationContract>]
    [<DataContractFormat(Style = OperationFormatStyle.Rpc)>]
    abstract RpcOperation: name: string -> string

    [<OperationContract>]
    [<DataContractFormat(Style = OperationFormatStyle.Document)>]
    abstract DocumentOperation: name: string -> string


[<PrintMessagesToConsole>]
type Service() =
    interface IContract with
        member this.RpcOperation(name) =
            sprintf "Hi, %s!" name
        
        member this.DocumentOperation(name) =
            sprintf "Hi, %s!" name


let host = new ServiceHost(typeof<Service>, new Uri("net.tcp://localhost"))
host.Open()

let proxy = ChannelFactory<IContract>.CreateChannel(host.Description.Endpoints.[0].Binding, host.Description.Endpoints.[0].Address)
proxy.RpcOperation("You")
proxy.DocumentOperation("You")

(proxy :?> ICommunicationObject).Close()
host.Close()
