#r "System.ServiceModel"
#r "System.Runtime.Serialization"
open System
open System.Threading.Tasks
open System.Windows.Forms
open System.ServiceModel


[<ServiceContract>]
type IMyContract =
    [<OperationContract>]
    abstract MyMethod : unit -> string


type MyService() =
    interface IMyContract with
        member this.MyMethod() = "MyService.MyMethod()"


type HostForm() as this =
    inherit Form()
    
    let host = new ServiceHost(typeof<MyService>, new Uri("net.tcp://localhost"))

    let btn = new Button(Text = "Service Call")
    do btn.Click.Add(fun _ -> this.ServiceCall())
    do this.Controls.Add(btn)

    do this.FormClosed.Add(fun _ -> host.Close())
    do host.Open()

    member this.ServiceCall() =
        Task.Factory.StartNew(fun () ->
            let proxy = ChannelFactory<IMyContract>.CreateChannel(host.Description.Endpoints.[0].Binding, host.Description.Endpoints.[0].Address)
            MessageBox.Show(proxy.MyMethod()) |> ignore
            (proxy :?> ICommunicationObject).Close())
        |> ignore


Application.Run(new HostForm())
