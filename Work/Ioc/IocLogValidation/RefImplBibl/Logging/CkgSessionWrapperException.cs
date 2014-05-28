using System;

namespace RefImplBibl.Logging
{
    public class CkgSessionWrapperException : Exception
    {
        private readonly string _sessionInfo;

        public CkgSessionWrapperException(string sessionInfo, Exception innerException) : base("", innerException)
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
