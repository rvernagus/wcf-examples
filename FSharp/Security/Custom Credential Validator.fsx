#r "System.ServiceModel"
#r "System.IdentityModel"
#r "System.Web.ApplicationServices"
open System
open System.IdentityModel.Selectors
open System.IdentityModel.Tokens
open System.ServiceModel
open System.ServiceModel.Description
open System.ServiceModel.Security


[<ServiceContract>]
type IServiceContract =
    [<OperationContract>]
    abstract AnOperation : unit -> unit


type ServiceClass() =
    interface IServiceContract with
        member this.AnOperation() =
            ()


type MyUserNamePasswordValidator() =
    inherit UserNamePasswordValidator()

    override this.Validate(userName, password) =
        printfn "  Validating credentials..."
        if userName <> "Joe" || password <> "bar" then
            raise <| new SecurityTokenValidationException("The user could not be authenticated.")


let binding = new NetTcpBinding()
binding.Security.Mode <- SecurityMode.Message
binding.Security.Message.ClientCredentialType <- MessageCredentialType.UserName
let host = new ServiceHost(typeof<ServiceClass>, new Uri("net.tcp://localhost"))
host.AddServiceEndpoint(typeof<IServiceContract>, binding, "")

let credentials = new ServiceCredentials()
credentials.UserNameAuthentication.UserNamePasswordValidationMode <- UserNamePasswordValidationMode.Custom
credentials.UserNameAuthentication.CustomUserNamePasswordValidator <- new MyUserNamePasswordValidator()
host.Description.Behaviors.Add(credentials)

host.Open()
printfn "--> %A" <| host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator

let factory = new ChannelFactory<IServiceContract>(host.Description.Endpoints.[0].Binding)
factory.Credentials.UserName.UserName <- "Not Joe"
let proxy = factory.CreateChannel(host.Description.Endpoints.[0].Address)
proxy.AnOperation()

(proxy :?> ICommunicationObject).Close()
host.Close()