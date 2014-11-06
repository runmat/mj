using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using GeneralTools.Contracts;

namespace GeneralTools.Models
{
    public class StackContext : IStackContext
    {
        private List<string> _stackFrames = new List<string>();

        public List<string> StackFrames
        {
            get { return _stackFrames; }
            set { _stackFrames = value; }
        }

        public void Init(Exception exception = null)
        {
            StackTrace stackTrace;
            if (exception != null)
                stackTrace = new StackTrace(exception, true);
            else
                stackTrace = new StackTrace(Thread.CurrentThread, true);

            var stackFrames = stackTrace.GetFrames();
            if (stackFrames != null)
                StackFrames = stackFrames.Where(frame =>
                    {
                        var frameFileName = frame.GetFileName();
                        return frameFileName != null && !frameFileName.ToLower().Contains(@"\generaltools\log\");
                    })
                    .Select(frame => frame.ToString()).ToList();
        }

        
        #region internal

        // ReSharper disable InconsistentNaming

        // ReSharper restore InconsistentNaming

        #endregion
    }
}
