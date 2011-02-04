open System
open System.ServiceModel
open System.ServiceModel.Web


[<ServiceContract>]
type IMyServiceContract =
    [<OperationContract>]
    [<WebGet>]
    abstract MyOperation : unit -> string


type MyService() =
    interface IMyServiceContract with
        member this.MyOperation() =
            "Hi!"


let host = new ServiceHost(typeof<MyService>)
host.Open()

printfn "The following endpoints were created:"
for ep in host.Description.Endpoints do
    printfn "  Address: %A" ep.Address
    printfn "  Binding: %A" ep.Binding
    printfn "---------------------"

Console.ReadLine() |> ignore
host.Close()
