#r "System.ServiceModel"
#r "System.Runtime.Serialization"
open System
open System.ServiceModel
open System.ServiceModel.Description
open System.ServiceModel.Dispatcher


[<ServiceContract>]
type IMyContract =
    [<OperationContract>]
    abstract MyMethod : string * int -> string


type MyService() =
    interface IMyContract with
        member this.MyMethod(s, n) = "result"


type PrintToConsoleParameterInspector() =
    interface IParameterInspector with
        member this.AfterCall(operationName, outputs, returnValue, correlationState) =
            printfn "AFTER CALL"
            printfn "  Operation: %s" operationName
            for output in outputs do
                printfn "    param: %A" output
            printfn "    result: %A" returnValue
            printfn "    correlationState: %A" correlationState
        
        member this.BeforeCall(operationName, inputs) =
            printfn "BEFORE CALL"
            printfn "  Operation: %s" operationName
            for input in inputs do
                printfn "    input: %A" input
            box "state"


type ApplyParameterInspectorBehavior(inspector : IParameterInspector) =
    interface IOperationBehavior with
        member this.AddBindingParameters(operationDescription, bindingParameters) = ()
        member this.ApplyClientBehavior(operationDescription, clientOperation) = ()
        member this.Validate(operationDescription) = ()
        member this.ApplyDispatchBehavior(operationDescription, dispatchOperation) =
            dispatchOperation.ParameterInspectors.Add(inspector) 


let host = new ServiceHost(typeof<MyService>, new Uri("net.tcp://localhost"))
host.AddDefaultEndpoints()
let inspector = new PrintToConsoleParameterInspector()
let behavior = new ApplyParameterInspectorBehavior(inspector)
host.Description.Endpoints.[0].Contract.Operations.[0].Behaviors.Add(behavior)
host.Open()

let proxy = ChannelFactory<IMyContract>.CreateChannel(host.Description.Endpoints.[0].Binding, host.Description.Endpoints.[0].Address)
proxy.MyMethod("arg", 1)

(proxy :?> ICommunicationObject).Close()
host.Close()