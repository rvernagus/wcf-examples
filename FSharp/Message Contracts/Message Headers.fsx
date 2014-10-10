#r "System.Net.Http"
#r "System.Runtime.Serialization"
#r "System.ServiceModel"
open System.ServiceModel.Channels
open System.Runtime.Serialization

[<Literal>]
let ns = "http://schemas.mynamespace.org"

[<Literal>]
let headerName = "MyHeader"

[<DataContract(Name = headerName, Namespace = ns)>]
type MyHeader =
    { [<DataMember>] mutable Content : string }


let myhdr = { Content = "Content" }

let v = MessageVersion.Soap12
let msg = Message.CreateMessage(v, "action", "")
MessageHeader.CreateHeader(headerName, ns, myhdr)
|> msg.Headers.Add

let result = msg.Headers.GetHeader<MyHeader>(headerName, ns)
printfn "%A" result