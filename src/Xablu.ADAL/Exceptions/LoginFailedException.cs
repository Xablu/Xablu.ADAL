using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace Xablu.ADAL.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException(AdalServiceException innerException) : base(innerException.Message, innerException) { }
    }
}
