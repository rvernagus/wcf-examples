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


type ApplyDispatchMessageInspectorBehavior(inspector : IDispatchMessageInspector) =
    interface IEndpointBehavior with
        member this.AddBindingParameters(endpoint, bindingParameters) = ()
        member this.ApplyClientBehavior(endpoint, clientRuntime) = ()
        member this.Validate(endpoint) = ()
        member this.ApplyDispatchBehavior(endpoint, endpointDispatcher) =
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector)


// Message Inspectors on the service side implement IDispatchMessageInspector
type PrintToConsoleMessageInspector() =
    interface IDispatchMessageInspector with
        member this.AfterReceiveRequest(request, channel, instanceContext) =
            printfn "========\nRequest\n========\n%A\n" request
            null
        
        member this.BeforeSendReply(reply, correlationState) =
            printfn "========\nReply\n========\n%A\n" reply


let host = new ServiceHost(typeof<MyService>, new Uri("net.tcp://localhost"))
host.AddDefaultEndpoints()
let inspector = new PrintToConsoleMessageInspector()
let behavior = new ApplyDispatchMessageInspectorBehavior(inspector)
host.Description.Endpoints.[0].Behaviors.Add(behavior)
host.Open()

let proxy = ChannelFactory<IMyContract>.CreateChannel(host.Description.Endpoints.[0].Binding, host.Description.Endpoints.[0].Address)
proxy.MyMethod()

(proxy :?> ICommunicationObject).Close()
host.Close()
