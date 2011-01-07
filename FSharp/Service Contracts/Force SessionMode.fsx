#r "System.ServiceModel"
#r "System.Runtime.Serialization"
open System
open System.ServiceModel


[<ServiceContract(SessionMode = SessionMode.Required)>]
type IMyContract =
    [<OperationContract>]
    abstract MyMethod : unit -> unit


type MyService() =
    let mutable counter = 0
    
    do printfn "MyService.MyService()"
    
    interface IMyContract with
        member this.MyMethod() =
            counter <- counter + 1
            printfn "Counter = %d" counter
    
    interface IDisposable with
        member this.Dispose() =
            printfn "MyService.Dispose()"


let host = new ServiceHost(typeof<MyService>, new Uri("http://localhost"))
try
    host.Open()
    host.Close()
with ex -> printfn "%s" ex.Message
