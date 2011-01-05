open System
open System.Collections.Concurrent
open System.Threading
open System.Threading.Tasks


type MySynchronizationContext() as this =
    inherit SynchronizationContext()

    let workToDo = new BlockingCollection<SendOrPostCallback * obj>()
    let thread = new Thread(new ThreadStart(this.DoWork))
    do thread.Start()
    do printfn "Synchronizing work on thread #%d" thread.ManagedThreadId

    member private this.DoWork() =
        let workEnum = workToDo.GetConsumingEnumerable()
        for item in workEnum do
            let d, state = item
            d.Invoke(state)

    override this.Send(d, state) =
        workToDo.Add((d, state))

    override this.Post(d, state) =
        workToDo.Add((d, state))


printfn "Main thread is #%d" Thread.CurrentThread.ManagedThreadId

let doWork =
    let rand = new Random()
    let workPrint = new SendOrPostCallback(fun _ ->
        printfn "  Work done on Thread #%d" Thread.CurrentThread.ManagedThreadId)
    let workBody () = 
        Thread.Sleep(rand.Next(0, 1000))
        SynchronizationContext.Current.Send((workPrint), null)

    fun _ -> new Action(workBody)

let syncContext = new MySynchronizationContext()
SynchronizationContext.SetSynchronizationContext(syncContext)

let options = new ParallelOptions()
options.TaskScheduler <- TaskScheduler.FromCurrentSynchronizationContext()
options.MaxDegreeOfParallelism <- Environment.ProcessorCount // Could not get example to work with TPL default
let allWork = Array.init 10 doWork
Parallel.Invoke(options, allWork)

Console.ReadLine()
