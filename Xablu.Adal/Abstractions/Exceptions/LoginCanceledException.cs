using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace Plugin.Xablu.Adal.Abstractions.Exceptions
{
    public class LoginCanceledException : Exception
    {
        public LoginCanceledException(AdalServiceException innerException) : base(innerException.Message, innerException) { }
    }
}
