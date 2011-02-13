using System;
using System.Threading;

namespace Asynchronous.Service
{
    class MyAsyncResult<T> : IAsyncResult
    {
        private readonly object _state;
        public T Data { get; set; }


        public MyAsyncResult(T data, object state)
        {
            Data = data;
            _state = state;
        }

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
            get { return _state; }
        }

        public bool CompletedSynchronously
        {
            get { return true; }
        }
    }
}