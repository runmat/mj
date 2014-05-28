using Elmah;
using NLog;
using NLog.Targets;

namespace RefImplBibl.Logging
{
    [Target("ElmahTarget")]
    public sealed class ElmahTarget : TargetWithLayout
    {
        protected override void Write(LogEventInfo logEvent)
        {
            if (logEvent.Exception != null)
            {
                var r = ErrorSignal.FromCurrentContext();
                r.Raise(logEvent.Exception);
            }
        }
    }
}
