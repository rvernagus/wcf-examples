﻿using System;
using System.ServiceModel;
using System.Threading;
using Asynchronous.Client;

namespace Asynchronous
{
    class Program
    {
        static void Main(string[] args)
        {
            const string uri = "net.pipe://localhost";
            var binding = new NetNamedPipeBinding();

            // Service
            var host = new ServiceHost(typeof(Service.MyService), new Uri(uri));
            host.AddServiceEndpoint(typeof(Service.IMyService), binding, uri);
            host.Open();

            // Client
            var proxy = new MyProxy(binding, uri);
            Console.WriteLine("Client: Making asynchronous call (Thread: {0})", Thread.CurrentThread.ManagedThreadId);
            var result = proxy.BeginMakeCall("data", r => Console.WriteLine("Client: Callback made (State: {0}, Thread: {1})", r.AsyncState, Thread.CurrentThread.ManagedThreadId), "state");
            Console.WriteLine("Client: Waiting for callback");
            proxy.EndMakeCall(result);
            proxy.Close();

            host.Close();
        }
    }
}