using System;

namespace GeneralTools.Log.Services
{
    /// <summary>
    /// Exception schreibt die Session Info in das Stacktrace
    /// </summary>
    public class CkgSessionWrapperException : Exception
    {
        private readonly string _sessionInfo;

        public CkgSessionWrapperException(string sessionInfo, Exception innerException)
            : base("", innerException)
        {
            _sessionInfo = sessionInfo;
        }

        public override string ToString()
        {
            var message = string.Concat(base.ToString(), Environment.NewLine, _sessionInfo);
            return message;
        }
    }
}
