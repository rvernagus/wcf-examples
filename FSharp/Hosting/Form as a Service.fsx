#r "System.ServiceModel"
#r "System.Runtime.Serialization"
open System
open System.Threading
open System.Threading.Tasks
open System.Windows.Forms
open System.ServiceModel
open System.ServiceModel.Channels


let uri = new Uri("net.tcp://localhost")
let binding = new NetTcpBinding()


[<ServiceContract>]
type IFormManager =
    [<OperationContract>]
    abstract IncrementLabel : unit -> unit


[<ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)>]
type MyForm() as this =
    inherit Form()
    
    let context = SynchronizationContext.Current
    let mutable host : ServiceHost option = None
    let label = new Label(Text="0")
    do this.Controls.Add(label)
    do this.Initialize()
    
    member private this.Initialize() =
        let h = new ServiceHost(this, uri)
        h.AddServiceEndpoint(typeof<IFormManager>, binding, "") |> ignore
        this.FormClosed.Add(fun e -> h.Close())
        h.Open()
        host <- Some h
    
    member this.Host = Option.get host
    
    member this.IncrementLabel() =
        label.Text <- string (int label.Text + 1)
        
    interface IFormManager with
        member this.IncrementLabel() = 
            printfn "IncrementLabel()"
            context.Send((fun _ -> this.IncrementLabel()), null)


let showFormTask = Task.Factory.StartNew(fun () ->
    let form = new MyForm()
    form.ShowDialog())

Task.Factory.StartNew(fun () ->
    let proxy = ChannelFactory<IFormManager>.CreateChannel(binding, new EndpointAddress(string uri))
    printfn "About to increment label..."
    Thread.Sleep(2000)
    proxy.IncrementLabel()
    Thread.Sleep(2000)
    proxy.IncrementLabel()
    (proxy :?> ICommunicationObject).Close())

showFormTask.Wait()
