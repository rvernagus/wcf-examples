#r "System.Net.Http"
#r "System.Runtime.Serialization"
#r "System.ServiceModel"
#r "System.Xml.Linq"
open System
open System.IO
open System.Xml.Linq
open System.Runtime.Serialization
open System.ServiceModel
open System.ServiceModel.Channels
open System.ServiceModel.Description
open System.ServiceModel.Dispatcher

let serialize<'a> (x: 'a) =
    let serializer = new DataContractSerializer(typeof<'a>)
    let stream = new MemoryStream()
    serializer.WriteObject(stream, x)
    stream.Position <- 0L
    let reader = new StreamReader(stream)
    let doc = XDocument.Parse(reader.ReadToEnd())
    doc.ToString()

let deserialize<'a> (x: string) =
    let stream = new MemoryStream()
    let data = System.Text.Encoding.UTF8.GetBytes(x)
    stream.Write(data, 0, data.Length)
    stream.Position <- 0L
    let deserializer = new DataContractSerializer(typeof<'a>)
    deserializer.ReadObject(stream) :?> 'a

type PrintToConsoleMessageInspector() =
    interface IDispatchMessageInspector with
        member this.AfterReceiveRequest(request, channel, instanceContext) =
            printfn "========\nRequest\n========\n%A\n" request
            null
            
        member this.BeforeSendReply(reply, correlationState) =
            printfn "========\nReply\n========\n%A\n" reply


type ApplyDispatchMessageInspectorBehavior(inspector : IDispatchMessageInspector) =
    interface IEndpointBehavior with
        member this.AddBindingParameters(endpoint, bindingParameters) = ()
        member this.ApplyClientBehavior(endpoint, clientRuntime) = ()
        member this.Validate(endpoint) = ()
        member this.ApplyDispatchBehavior(endpoint, endpointDispatcher) =
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector)


type ApplyClientMessageInspectorBehavior(inspector : IClientMessageInspector) =
    interface IEndpointBehavior with
        member this.AddBindingParameters(endpoint, bindingParameters) = ()
        member this.Validate(endpoint) = ()
        member this.ApplyDispatchBehavior(endpoint, endpointDispatcher) = ()
        member this.ApplyClientBehavior(endpoint, clientRuntime) =
            clientRuntime.MessageInspectors.Add(inspector)     
    
    
type ApplyParameterInspectorBehavior(inspector : IParameterInspector) =
    interface IOperationBehavior with
        member this.AddBindingParameters(operationDescription, bindingParameters) = ()
        member this.ApplyClientBehavior(operationDescription, clientOperation) = ()
        member this.Validate(operationDescription) = ()
        member this.ApplyDispatchBehavior(operationDescription, dispatchOperation) =
            dispatchOperation.ParameterInspectors.Add(inspector)            
                           
                

[<AttributeUsage(AttributeTargets.Class)>]
type PrintMessagesToConsoleAttribute() =
    inherit Attribute()
        
    let inspector = new PrintToConsoleMessageInspector()
    let behavior = new ApplyDispatchMessageInspectorBehavior(inspector) :> IEndpointBehavior
        
    interface IServiceBehavior with
        member this.AddBindingParameters(description, host, endpoints, bindingParams) = ()
        member this.Validate(description, host) = ()
        member this.ApplyDispatchBehavior(description, host) =
            description.Endpoints
            |> Seq.iter (fun ep -> ep.Behaviors.Add(behavior))