using System;

namespace GeneralTools.Services
{
    public abstract class BackgroundTaskBase<T> where T : class
    {
        private ThreadSaveQueue<T> _queue;

        private ThreadSaveQueue<T> Queue { get { return (_queue ?? (_queue = new ThreadSaveQueue<T>())); } }

        public int QueueCount { get { return Queue.Count; } }


        protected abstract bool NativeThreadLoop();

        public virtual void Enqueue(T item)
        {
            Queue.Enqueue(item);
        }

        public virtual T Dequeue(int sleepMilliseconds = 3000)
        {
            return Queue.Dequeue();
        }

        public void Stop()
        {
            Queue.Stop();
        }

        public void Resume()
        {
            Queue.Resume();
        }

        public bool Any(Predicate<T> compareFunc)
        {
            return Queue.Any(compareFunc);
        }
    }
}
