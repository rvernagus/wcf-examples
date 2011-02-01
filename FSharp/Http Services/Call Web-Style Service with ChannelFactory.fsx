// Adapted from http://msdn.microsoft.com/en-us/library/bb412178.aspx
#r "System.ServiceModel"
#r "System.ServiceModel.Web"
open System
open System.ServiceModel
open System.ServiceModel.Description
open System.ServiceModel.Web


[<ServiceContract>]
type IService =
    [<OperationContract>]
    [<WebGet>]
    abstract EchoWithGet : s : string -> string

    [<OperationContract>]
    [<WebInvoke>] // By default WebInvoke maps POST calls to the operation
    abstract EchoWithPost : s : string -> string


type Service() =
    interface IService with
        member this.EchoWithGet(s) =
            printfn "  In EchoWithGet(s)"
            sprintf "You said %s" s
        
        member this.EchoWithPost(s) = 
            printfn "  In EchoWithPost(s)" 
            (this :> IService).EchoWithGet(s)
        

let private host = new WebServiceHost(typeof<Service>, new Uri("http://localhost:8000"))
host.AddServiceEndpoint(typeof<IService>, new WebHttpBinding(), "") |> ignore
host.Description.Endpoints.[0].Behaviors.Add(new WebHttpBehavior(HelpEnabled=true))
host.Open()

let factory = new ChannelFactory<IService>(new WebHttpBinding(), "http://localhost:8000")
factory.Endpoint.Behaviors.Add(new WebHttpBehavior())
let channel = factory.CreateChannel()

printfn "Calling EchoWithGet via HTTP GET: "
let sGet = channel.EchoWithGet("Hello, world")
printfn "    Output: %s\n" sGet

printfn "Calling EchoWithPost via HTTP POST: "
let sPost = channel.EchoWithPost("Hello, world")
printfn "    Output: %s" sPost

host.Close()
