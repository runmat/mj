using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneralTools.Services
{
    /// <summary>
    /// Thread save queue ( http://element533.blogspot.de/2010/01/stoppable-blocking-queue-for-net.html )
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ThreadSaveQueue<T>
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private bool _stopped;

        public int Count { get; set; }


        public bool Any(Predicate<T> compareFunc)
        {
            var found = false;
            lock (_queue)
            {
                foreach (var queueItem in _queue.ToList())
                    if (compareFunc(queueItem))
                    {
                        found = true;
                        break;
                    }
            }
            return found;
        }

        public bool Enqueue(T item)
        {
            if (_stopped)
                return false;

            lock (_queue)
            {
                _queue.Enqueue(item);
                //Monitor.Pulse(_queue);
                Count = _queue.Count;
            }
            
            return true;
        }

        public T Dequeue()
        {
            var returnItem = default(T);

            if (_stopped)
                return returnItem;

            lock (_queue)
            {
                if (_stopped)
                    return returnItem;
                
                //while (_queue.Count == 0)
                //{
                //    Monitor.Wait(_queue);
                //    if (_stopped)
                //        return default(T);
                //}

                if (_queue.Count > 0)
                {
                    returnItem = _queue.Dequeue();
                    Count = _queue.Count;
                }
            }

            return returnItem;
        }

        public void Stop()
        {
            if (_stopped)
                return;
            lock (_queue)
            {
                if (_stopped)
                    return;
                _stopped = true;
                //Monitor.PulseAll(_queue);
            }
        }

        public void Resume()
        {
            if (!_stopped)
                return;
            lock (_queue)
            {
                if (!_stopped)
                    return;
                _stopped = false;
                //Monitor.PulseAll(_queue);
            }
        }
    }
}
