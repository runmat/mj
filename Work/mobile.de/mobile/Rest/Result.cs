using System;

namespace Mobile.Rest
{
    public class Result <T> 
    {
        public Result()
        {
            Errors = new errors();
        }

        public T Value { get; set; }
        public errors Errors { get; set; }

        public string ErrorSummary
        {
            get
            {
                throw new NotImplementedException("Ermittlung der ErrorSummary implementieren");
            }
        }
    }
}
