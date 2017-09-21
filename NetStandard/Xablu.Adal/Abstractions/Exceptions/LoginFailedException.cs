using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace Plugin.Xablu.Adal.Abstractions.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException(AdalServiceException innerException) : base(innerException.Message, innerException) { }
    }
}
