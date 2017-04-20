using System;

namespace Xablu.ADAL.Exceptions
{
    public class ConnectivityException : Exception
    {
        public ConnectivityException(string message) : base(message) { }
    }
}
