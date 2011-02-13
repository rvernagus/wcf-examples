using System;
using System.ServiceModel;
using System.Threading;

namespace Asynchronous.ClientSide.Client
{

    [ServiceContract]
    internal interface IMyService
    {
        [OperationContract]
        void MakeCall(string message);

        [OperationContract(AsyncPattern = true)]
        MakeCallResult<string> BeginMakeCall(string message, AsyncCallback callback, object state);

        // Note: No [OperationContract] needed
        void EndMakeCall(IAsyncResult result);
    }

    class MyServiceProxy : ClientBase<IMyService>, IMyService
    {
        #region Implementation of IMyService

        public void MakeCall(string message)
        {
            Channel.MakeCall(message);
        }

        public MakeCallResult<string> BeginMakeCall(string message, AsyncCallback callback, object state)
        {
            Channel.BeginMakeCall(message, callback, state);
        }

        public void EndMakeCall(IAsyncResult result)
        {
            Channel.EndMakeCall(result);
        }

        #endregion
    }

    internal class MakeCallResult<T> : IAsyncResult
    {
        public T State { get; set; }

        #region Implementation of IAsyncResult

        public bool IsCompleted
        {
            get { return true; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { throw new NotImplementedException(); }
        }

        public object AsyncState
        {
            get { return State; }
        }

        public bool CompletedSynchronously
        {
            get { return true; }
        }

        #endregion
    }
}