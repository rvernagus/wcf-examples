#r "System.Runtime.Serialization"
#r "System.ServiceModel"
#load @"..\Common.fsx"
open Common
open System
open System.Net.Security
open System.ServiceModel.Channels
open System.ServiceModel
open System.ServiceModel.Channels


[<MessageContract>]
type Person(age, firstName, lastName) =
    let mutable age = age
    let mutable firstName = firstName
    let mutable lastName = lastName
    
    new() = Person(-1, @"N\A", @"N\A")
    
    [<MessageBodyMember(ProtectionLevel = ProtectionLevel.EncryptAndSign)>]
    member this.Age with get() = age
                     and set v = age <- v
    
    [<MessageBodyMember(ProtectionLevel = ProtectionLevel.Sign)>]
    member this.FirstName with get() = firstName
                           and set v = firstName <- v
      
    [<MessageBodyMember(ProtectionLevel = ProtectionLevel.None)>] 
    member this.LastName with get() = lastName
                          and set v = lastName <- v
      

[<ServiceContract>]
type IContract =
    [<OperationContract>]
    abstract AnOperation : Person -> Person


[<PrintMessagesToConsole>]
type Service() =
    interface IContract with
        member this.AnOperation(person) = person


let binding = new WSHttpBinding()
binding.Security.Mode <- SecurityMode.Message
let host = new ServiceHost(typeof<Service>, new Uri("http://localhost"))
host.AddServiceEndpoint(typeof<IContract>, binding, "")
host.Open()

let proxy = ChannelFactory<IContract>.CreateChannel(binding, host.Description.Endpoints.[0].Address)

let person = Person(36, "Joe", "Schmo")
proxy.AnOperation(person)

(proxy :?> ICommunicationObject).Close()
host.Close()

