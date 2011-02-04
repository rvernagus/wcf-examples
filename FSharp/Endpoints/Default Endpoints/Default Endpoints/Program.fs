open System
open System.ServiceModel


[<ServiceContract>]
type IMyServiceContract =
    [<OperationContract>]
    abstract MyOperation : unit -> unit


type MyService() =
    interface IMyServiceContract with
        member this.MyOperation() =
            printfn "...in MyOperation()"


let host = new ServiceHost(typeof<MyService>)
host.Open()

printfn "The following endpoints were created:"
for ep in host.Description.Endpoints do
    printfn "  Address: %A" ep.Address
    printfn "  Binding: %A" ep.Binding
    printfn "---------------------"

Console.ReadLine() |> ignore
host.Close()
