using System;
using System.Threading;
using System.Threading.Tasks;

namespace PdfPrint
{
    public class TaskService
    {
        #region UI Tasks

        static private TaskScheduler _uiSynchronizationContext;

        static public void StartUiTask(Action action)
        {
            StartDelayedUiTask(0, action);
        }

        static public void StartDelayedUiTask(int milliseconds, Action action)
        {
            if (_uiSynchronizationContext == null)
                return;

            Task.Factory.StartNew(() => Thread.Sleep(milliseconds)).ContinueWith(t => action(), _uiSynchronizationContext);
        }

        static public void InitUiSynchronizationContext()
        {
            _uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext();
        }

        #endregion


        static public Task StartTask(Action action)
        {
            return Task.Factory.StartNew(action);
        }

        static public Task StartLongRunningTask(Action action, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(action, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        static public Task StartLongRunningTask(Action action)
        {
            return Task.Factory.StartNew(action, TaskCreationOptions.LongRunning);
        }

        static public void StartLongRunningTaskAndWait(Action action)
        {
            StartLongRunningTask(action).Wait();
        }
    }

}
