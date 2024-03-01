using System;
using System.Threading;

public class RivetThread
{
    public object _lock = new object();
    public Thread _thread;
    public object _output;

    public object Output
    {
        get
        {
            lock (_lock)
            {
                return _output;
            }
        }
        private set
        {
            lock (_lock)
            {
                _output = value;
            }
        }
    }

    public RivetThread(Action action)
    {
        _thread = new Thread(() =>
        {
            // var result = action();
            // Output = result;
        });
        _thread.Start();
    }

    public void WaitToFinish()
    {
        _thread.Join();
    }
}