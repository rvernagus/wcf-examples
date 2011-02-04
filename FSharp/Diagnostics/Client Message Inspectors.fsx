#r "System.ServiceModel"
#r "System.Runtime.Serialization"
open System
open System.ServiceModel
open System.ServiceModel.Description
open System.ServiceModel.Dispatcher


[<ServiceContract>]
type IMyContract =
    [<OperationContract>]
    abstract MyMethod : unit -> unit


type MyService() =
    interface IMyContract with
        member this.MyMethod() = ()


type ApplyClientMessageInspectorBehavior(inspector : IClientMessageInspector) =
    interface IEndpointBehavior with
        member this.AddBindingParameters(endpoint, bindingParameters) = ()
        member this.Validate(endpoint) = ()
        member this.ApplyDispatchBehavior(endpoint, endpointDispatcher) = ()
        member this.ApplyClientBehavior(endpoint, clientRuntime) =
            clientRuntime.MessageInspectors.Add(inspector)    


// Message Inspectors on the service side implement IDispatchMessageInspector
type PrintToConsoleMessageInspector() =
    interface IClientMessageInspector with
        member this.BeforeSendRequest(request, channel) =
            printfn "========\nRequest\n========\n%A\n" request
            null
        
        member this.AfterReceiveReply(reply, correlationState) =
            printfn "========\nReply\n========\n%A\n" reply


let host = new ServiceHost(typeof<MyService>, new Uri("net.tcp://localhost"))
host.Open()

let factory = new ChannelFactory<IMyContract>(host.Description.Endpoints.[0].Binding, host.Description.Endpoints.[0].Address)
let inspector = new PrintToConsoleMessageInspector()
let behavior = new ApplyClientMessageInspectorBehavior(inspector)
factory.Endpoint.Behaviors.Add(behavior)
let proxy = factory.CreateChannel()
proxy.MyMethod()

(proxy :?> ICommunicationObject).Close()
host.Close()
