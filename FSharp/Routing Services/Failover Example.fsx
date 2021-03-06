#r "System.Runtime.Serialization"
#r "System.ServiceModel"
#r "System.ServiceModel.Routing"
open System.Collections.Generic
open System.ServiceModel
open System.ServiceModel.Description
open System.ServiceModel.Dispatcher
open System.ServiceModel.Routing


[<ServiceContract>]
type IHelloService =
    [<OperationContract>]
    abstract SayHello : unit -> string


[<ServiceBehavior(
    InstanceContextMode=InstanceContextMode.Single,
    IncludeExceptionDetailInFaults=true)>]
type HelloService(serviceNum : int) =
    interface IHelloService with
        member this.SayHello() = printfn "In Service %d..." serviceNum; "Hello!"


let clientAddress1 = "http://localhost:8000/service1"
let clientAddress2 = "http://localhost:8000/service2"
let routerAddress = "http://localhost:8000/router"

let routerBinding = new WSHttpBinding()
let clientBinding = new WSHttpBinding()

let routerHost = new ServiceHost(typeof<RoutingService>)
let helloHost1 = new ServiceHost(new HelloService(1))
let helloHost2 = new ServiceHost(new HelloService(2))

//add the endpoint the router will use to recieve messages
routerHost.AddServiceEndpoint(typeof<IRequestReplyRouter>, routerBinding, routerAddress)

helloHost1.AddServiceEndpoint(typeof<IHelloService>, clientBinding, clientAddress1)
helloHost2.AddServiceEndpoint(typeof<IHelloService>, clientBinding, clientAddress2)

//create the client endpoint the router will route messages to
let contract = ContractDescription.GetContract(typeof<IRequestReplyRouter>)
let routerClient1 = new ServiceEndpoint(contract, clientBinding, new EndpointAddress(clientAddress1))
let routerClient2 = new ServiceEndpoint(contract, clientBinding, new EndpointAddress(clientAddress2))

//create the endpoint list that contains the service endpoints we want to route to
//in this case we have only one
let endpointList = new List<ServiceEndpoint>()
endpointList.Add(routerClient1)
endpointList.Add(routerClient2)

//create a new routing configuration object
let rc = new RoutingConfiguration()

//add a MatchAll filter to the Router's filter table
//map it to the endpoint list defined earlier
//when a message matches this filter, it will be sent to the endpoint contained in the list
rc.FilterTable.Add(new MatchAllMessageFilter(), endpointList)

//attach the behavior to the service host
routerHost.Description.Behaviors.Add(new RoutingBehavior(rc))

helloHost1.Open()
helloHost2.Open()
routerHost.Open()

let client = ChannelFactory<IHelloService>.CreateChannel(routerBinding, new EndpointAddress(routerAddress))
client.SayHello()
helloHost1.Close()
client.SayHello()
client.SayHello()
(client :?> ICommunicationObject).Close()

helloHost2.Close()
routerHost.Close()

