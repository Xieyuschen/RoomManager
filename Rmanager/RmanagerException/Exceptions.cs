using System;

namespace Rmanager.Exceptions
{
    public class _403Exception : Exception
    {
        public _403Exception(string msg = "You don't have the authority to do this!") : base(msg) { }
    }
    public class _401Exception : Exception
    {
        public _401Exception(string msg) : base(msg) { }
    }
    public class _400Exception : Exception
    {
        public _400Exception(string msg = "Bad Input!") : base(msg) { }
    }
    public class _500Exception : Exception
    {
        public _500Exception(string msg = "Internal server error!\r\nIf this bug occurs frequently, please contact admin for help!") : base(msg) { }
    }
    public class ServiceStartUpException : Exception
    {
        public ServiceStartUpException(string msg) : base(msg) { }
    }
    public class Error
    {
        public string errorMessage { get; set; }
        public Error(string msg)
        {
            errorMessage = msg;
        }
    }
}
