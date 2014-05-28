using System;
using System.Collections.Generic;
using System.Linq;
using CarDocu.Services.Threads;

namespace CarDocu.Services
{
    public class DomainThreads : IDisposable 
    {
        public List<CardocuBackgroundTask> BackgroundThreads { get; private set; }

        public SapBackgroundTask SapBackgroundTask { get; private set; }

        public ArchiveBackgroundTask ArchiveBackgroundTask { get; private set; }

        public bool IsBusy { get { return BackgroundThreads.Any(thread => thread.IsBusy); } }


        public DomainThreads()
        {
            SapBackgroundTask = new SapBackgroundTask();
            ArchiveBackgroundTask = new ArchiveBackgroundTask();

            BackgroundThreads = new List<CardocuBackgroundTask> {
                                        SapBackgroundTask, 
                                        ArchiveBackgroundTask,
                                    };
        }

        public void Dispose()
        {
            BackgroundThreads.ForEach(thread => thread.Dispose());
        }
    }
}
