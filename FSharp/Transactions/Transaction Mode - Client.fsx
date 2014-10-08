#r "System.Net.Http"
#r "System.ServiceModel"
#r "System.Runtime.Serialization"
#r "System.Transactions"
open System
open System.ServiceModel
open System.ServiceModel.Channels
open System.Transactions
open System.Diagnostics


let printTrans() =
    let ambTrans = Transaction.Current
    if ambTrans <> null then
        let info = ambTrans.TransactionInformation
        printfn "Dist  ID %A\nLocal ID %s" info.DistributedIdentifier info.LocalIdentifier
    else
        printfn "No Transaction"
            
            
[<ServiceContract>]
type IMyContract =
    [<OperationContract>]
    [<TransactionFlow(TransactionFlowOption.Mandatory)>]
    abstract MyMethod : unit -> unit
        

type MyService() =
    interface IMyContract with
        [<OperationBehavior(TransactionScopeRequired = true)>]
        member this.MyMethod() =
            use scope = new TransactionScope()
            printTrans()
            scope.Complete()


// The Client mode ensures the service only uses the client's transaction
let host = new ServiceHost(typeof<MyService>, new Uri("net.tcp://localhost"))
// The Client mode requires the use of a transaction-aware binding with
//   transaction flow enabled
let binding = new NetTcpBinding(TransactionFlow = true)
host.AddServiceEndpoint(typeof<IMyContract>, binding, "")
host.Open()

let proxy = ChannelFactory<IMyContract>.CreateChannel(binding, host.Description.Endpoints.[0].Address)

printfn "No Scope"
try
    proxy.MyMethod()
with ex -> printfn "%s" ex.Message
printfn "---------------------"

let scope = new TransactionScope()

printfn "Local Scope"
printTrans()
printfn "---------------------"

printfn "Service Scope"
proxy.MyMethod()
printfn "---------------------"

scope.Complete()
scope.Dispose()

(proxy :?> ICommunicationObject).Close()
host.Close()
