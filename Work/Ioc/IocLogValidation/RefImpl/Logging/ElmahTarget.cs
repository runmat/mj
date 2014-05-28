using System;
using System.Web.Mvc;
using Elmah;
using NLog;
using NLog.Targets;
using RefImplBibl.Interfaces;

namespace RefImpl.Logging
{
    [Target("ElmahTarget")]
    public sealed class ElmahTarget : TargetWithLayout
    {
        protected override void Write(LogEventInfo logEvent)
        {
            if (logEvent.Exception != null)
            {
                var anwenderInfoProvider = DependencyResolver.Current.GetService<IAnwenderInfoProvider>();
                var ex = new CkgSessionWrapperException(anwenderInfoProvider.ToString(), logEvent.Exception);
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
    }

    public class CkgSessionWrapperException : Exception
    {
        private readonly string _sessionIndo;

        public CkgSessionWrapperException(string sessionIndo, Exception innerException) : base("", innerException)
        {
            _sessionIndo = sessionIndo;
        }

        public override string ToString()
        {
            var message = string.Concat(base.ToString(), Environment.NewLine, _sessionIndo);
            return message;
        }
    }
}
