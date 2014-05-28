using System;
using System.Linq;
using AutoAct.Entities;

namespace AutoAct.Rest
{
    public class Result <T> 
    {
        public Result()
        {
            Errors = new ErrorRootObject();
        }

        public T Value { get; set; }
        public ErrorRootObject Errors { get; set; }

        public string ErrorSummary
        {
            get
            {
                var messages = from error in Errors.errors
                               select string.Concat(error.message.de, "/Code:", error.code, "/Field:", error.field);

                return messages.Aggregate((current, next) => string.Concat(current, Environment.NewLine, next));
            }
        }
    }
}
