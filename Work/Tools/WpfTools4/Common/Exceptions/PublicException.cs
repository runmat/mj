using System;

namespace WpfTools4.Common.Exceptions
{
    public class PublicException : ApplicationException
    {
        public PublicException(String format, params Object[] args)
            : base(String.Format(format, args))
        {
        }

        public PublicException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
