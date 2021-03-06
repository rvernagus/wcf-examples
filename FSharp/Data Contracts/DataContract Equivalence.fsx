#load @"..\Common.fsx"
open Common
open System.Runtime.Serialization


[<DataContract(Name="Person", Namespace="")>]
type Person =
    { [<DataMember>]
      mutable Name : string;
      
      [<DataMember>]
      mutable Age : int }


[<DataContract(Name="Person", Namespace="")>]
type Employee =
    { [<DataMember(Name="Name")>]
      mutable FullName : string;
      
      [<DataMember(Name="Age")>]
      mutable AgeInYears : int
      
      [<DataMember>]
      mutable ServiceYears : int }


let person = { Name = "Ray"; Age = 35 }
let persondata = serialize person
printfn "%s\n\n----------------------\n" persondata

let emp = deserialize<Employee> persondata
printfn "%A" emp
