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
            Thread.Sleep(1) // why is this necessary?
            let d, state = item
            d.Invoke(state)

    override this.Send(d, state) =
        workToDo.Add((d, state))

    override this.Post(d, state) =
        workToDo.Add((d, state))


printfn "Main thread is #%d" Thread.CurrentThread.ManagedThreadId

let doWork = new Action(fun _ ->
    SynchronizationContext.Current.Send((fun _ ->
        printfn "  Work done on Thread #%d" Thread.CurrentThread.ManagedThreadId), null))

let syncContext = new MySynchronizationContext()
SynchronizationContext.SetSynchronizationContext(syncContext)

let options = new ParallelOptions()
options.TaskScheduler <- TaskScheduler.FromCurrentSynchronizationContext()
Parallel.Invoke(options, Array.create 10 doWork)

Console.ReadLine()
