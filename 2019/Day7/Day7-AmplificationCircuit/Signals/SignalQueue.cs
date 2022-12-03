using Day7_AmplificationCircuit.Signals;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Day7_AmplificationCircuit
{
    public class SignalQueue : IInputSignal, IOutputSignal, IDisposable
    {
        private readonly BlockingCollection<int> _queue = new BlockingCollection<int>(1);

        public int Get() => _queue.Take();
        public void Set(int signal) => _queue.Add(signal);

        public void Dispose() => _queue.Dispose();
    }
}
