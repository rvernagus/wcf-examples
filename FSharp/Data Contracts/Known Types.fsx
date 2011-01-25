#r "System.ServiceModel"
#r "System.Runtime.Serialization"
open System
open System.ServiceModel
open System.Runtime.Serialization


[<DataContract>]
// When no known types are specified, the example errors
[<KnownType(typeof<Manager>)>]
// You can specify a static method that returns Type[]
//[<KnownType("GetKnownTypes")>]
type Employee() =
    let mutable name = "Employee1"

    [<DataMember>]
    member this.Name
        with get () = name
        and set v = name <- v

    static member GetKnownTypes() =
        [| typeof<Manager> |]

and [<DataContract>] Manager() =
    inherit Employee()

    let mutable title = "The Boss"

    [<DataMember>]
    member this.Title
        with get () = title
        and set v = title <- v


[<ServiceContract>]
type IMyContract =
    [<OperationContract>]
    // Known types can be specified on the operation
//    [<ServiceKnownType(typeof<Manager>)>]
    abstract MyMethod : Employee -> unit


type MyService() =
    interface IMyContract with
        member this.MyMethod(employee) =
            printfn "Received parameter of type %O" (employee.GetType())


let host = new ServiceHost(typeof<MyService>, new Uri("net.tcp://localhost"))
host.Open()

let proxy = ChannelFactory<IMyContract>.CreateChannel(host.Description.Endpoints.[0].Binding, host.Description.Endpoints.[0].Address)

try
    proxy.MyMethod(new Manager())
    (proxy :?> ICommunicationObject).Close()
    host.Close()
with e -> printfn "%s" e.Message
