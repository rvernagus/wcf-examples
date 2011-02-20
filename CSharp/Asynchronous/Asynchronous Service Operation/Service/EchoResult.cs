using System;
using System.Threading;

namespace Asynchronous.Service
{
    internal class EchoResult<T> : IAsyncResult
    {
        public EchoResult(T data, object state)
        {
            Data = data;
            AsyncState = state;
        }

        public T Data { get; private set; }

        #region IAsyncResult Members

        public object AsyncState { get; private set; }

        public WaitHandle AsyncWaitHandle
        {
            get { throw new NotImplementedException(); }
        }

        public bool CompletedSynchronously
        {
            get { return true; }
        }

        public bool IsCompleted
        {
            get { return true; }
        }

        #endregion
    }
}