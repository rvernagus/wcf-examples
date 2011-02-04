#r "System.ServiceModel"
#r "System.Runtime.Serialization"
open System
open System.Collections.Generic
open System.ServiceModel
open System.ServiceModel.Description


[<ServiceContract(SessionMode = SessionMode.Required)>]
type IMyContract =
    [<OperationContract>]
    abstract IncrementCounter : unit -> unit


[<ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)>]
type MyService() =
    let counters = new Dictionary<string, int>()
    
    interface IMyContract with
        member this.IncrementCounter() =
            let sid = OperationContext.Current.SessionId
            if not (counters.ContainsKey(sid))
                then counters.Add(sid, 0)
            
            let c = counters.[sid] + 1
            counters.[sid] <- c
            printfn "Session %s; Counter = %d" sid c


let host = new ServiceHost(typeof<MyService>, new Uri("http://localhost"), new Uri("net.tcp://localhost"))
host.AddServiceEndpoint(typeof<IMyContract>, new WSHttpBinding(), "")
host.AddServiceEndpoint(typeof<IMyContract>, new NetTcpBinding(), "")
host.Open()

let proxy1 = ChannelFactory<IMyContract>.CreateChannel(host.Description.Endpoints.[0].Binding, host.Description.Endpoints.[0].Address)
let proxy2 = ChannelFactory<IMyContract>.CreateChannel(host.Description.Endpoints.[1].Binding, host.Description.Endpoints.[1].Address)

proxy1.IncrementCounter()
proxy1.IncrementCounter()
proxy2.IncrementCounter()
proxy2.IncrementCounter()

(proxy1 :?> ICommunicationObject).Close()
(proxy2 :?> ICommunicationObject).Close()
host.Close()


