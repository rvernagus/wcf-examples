#load @"..\Common.fsx"
open Common
open System.Collections
open System.Collections.Generic
open System.Runtime.Serialization
System.Console.Clear()


[<CollectionDataContract(Name="MyCollectionOf{0}")>]
type MyCollection<'T>() =
    let items = new ResizeArray<'T>()
    
    member this.Add(item : 'T) =
        items.Add(item)
            
    interface IEnumerable with
        member this.GetEnumerator() =
            items.GetEnumerator() :> IEnumerator 
            
    interface IEnumerable<'T> with
        member this.GetEnumerator() =
            items.GetEnumerator() :> IEnumerator<'T>
        

let c = new MyCollection<string>()
c.Add("item 1")
c.Add("item 2")
c.Add("item 3")

printfn "%s" (serialize<MyCollection<string>> c)
