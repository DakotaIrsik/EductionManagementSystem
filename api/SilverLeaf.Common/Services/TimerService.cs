using System;
using System.Threading;

namespace SilverLeaf.Common.Services
{
    public class TimerService
    {
        private Timer _timer;
        private AutoResetEvent _autoResetEvent;
        private Action _action;

        public DateTime TimerStarted { get; }

        public TimerService(Action action)
        {
            _action = action;
            _autoResetEvent = new AutoResetEvent(false);
            _timer = new Timer(Execute, _autoResetEvent, 5000, 5000);
            TimerStarted = DateTime.Now;
        }

        public void Execute(object stateInfo)
        {
            Thread.Sleep(5000);
            _action();


            if ((DateTime.Now - TimerStarted).Minutes > 5)
            {
                _timer.Dispose();
            }
        }
    }
}
