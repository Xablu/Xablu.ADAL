using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace Xablu.ADAL.Exceptions
{
    public class LoginCanceledException : Exception
    {
        public LoginCanceledException(AdalServiceException innerException) : base(innerException.Message, innerException) { }
    }
}
