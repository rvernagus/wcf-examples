using System;
using System.Threading;

namespace Asynchronous.Service
{
    public abstract class AsyncResult : IAsyncResult
    {
        private readonly AsyncCallback _callback;
        private readonly object _state;
        private readonly object _thisLock;
        private bool _completedSynchronously;
        private bool _endCalled;
        private Exception _exception;
        private bool _isCompleted;
        private ManualResetEvent _manualResetEvent;

        protected AsyncResult(AsyncCallback callback, object state)
        {
            _callback = callback;
            _state = state;
            _thisLock = new object();
        }

        private object ThisLock
        {
            get { return _thisLock; }
        }

        #region IAsyncResult Members

        public object AsyncState
        {
            get { return _state; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                if (_manualResetEvent != null)
                    return _manualResetEvent;

                lock (ThisLock)
                {
                    if (_manualResetEvent == null)
                        _manualResetEvent = new ManualResetEvent(_isCompleted);
                }

                return _manualResetEvent;
            }
        }

        public bool CompletedSynchronously
        {
            get { return _completedSynchronously; }
        }

        public bool IsCompleted
        {
            get { return _isCompleted; }
        }

        #endregion

        protected void Complete(bool completedSynchronously)
        {
            if (_isCompleted)
                throw new InvalidOperationException("Cannot call Complete twice");

            _completedSynchronously = completedSynchronously;

            if (completedSynchronously)
                _isCompleted = true;
            else
            {
                lock (ThisLock)
                {
                    _isCompleted = true;
                    if (_manualResetEvent != null)
                        _manualResetEvent.Set();
                }
            }

            if (_callback != null)
                _callback(this);
        }

        protected void Complete(bool completedSynchronously, Exception exception)
        {
            _exception = exception;
            Complete(completedSynchronously);
        }

        protected static TAsyncResult End<TAsyncResult>(IAsyncResult result)
                where TAsyncResult : AsyncResult
        {
            if (result == null)
                throw new ArgumentNullException("result");

            var asyncResult = result as TAsyncResult;

            if (asyncResult == null)
                throw new ArgumentException("Invalid async result.", "result");

            if (asyncResult._endCalled)
                throw new InvalidOperationException("Async object already ended.");

            asyncResult._endCalled = true;

            if (!asyncResult._isCompleted)
                asyncResult.AsyncWaitHandle.WaitOne();

            if (asyncResult._manualResetEvent != null)
                asyncResult._manualResetEvent.Close();

            if (asyncResult._exception != null)
                throw asyncResult._exception;

            return asyncResult;
        }
    }

    public abstract class TypedAsyncResult<T> : AsyncResult
    {
        protected TypedAsyncResult(AsyncCallback callback, object state)
                : base(callback, state)
        {
        }

        public T Data { get; private set; }

        protected void Complete(T data, bool completedSynchronously)
        {
            Data = data;
            Complete(completedSynchronously);
        }

        public static T End(IAsyncResult result)
        {
            var typedResult = End<TypedAsyncResult<T>>(result);
            return typedResult.Data;
        }
    }
}