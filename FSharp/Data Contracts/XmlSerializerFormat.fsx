#load @"..\Common.fsx"
open Common
open System
open System.ServiceModel
open System.ServiceModel.Description
open System.ServiceModel.Dispatcher


[<ServiceContract>]
[<XmlSerializerFormat(Style = OperationFormatStyle.Rpc, Use = OperationFormatUse.Encoded)>]
type ISomeLegacyService =
    [<OperationContract>]
    abstract SomeOp1: name: string -> unit


[<PrintMessagesToConsole>]
type MyService() =
    interface ISomeLegacyService with
        member this.SomeOp1(name) =
            printfn "Hi, %s!" name
            

let host = new ServiceHost(typeof<MyService>, new Uri("net.tcp://localhost"))
host.Open()

let proxy = ChannelFactory<ISomeLegacyService>.CreateChannel(host.Description.Endpoints.[0].Binding, host.Description.Endpoints.[0].Address)
proxy.SomeOp1("You")

(proxy :?> ICommunicationObject).Close()
host.Close()
