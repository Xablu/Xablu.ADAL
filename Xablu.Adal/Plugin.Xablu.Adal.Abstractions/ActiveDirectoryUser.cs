using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace Plugin.Xablu.Adal.Abstractions
{
    public class ActiveDirectoryUser
    {
        public string AccessToken { get; internal set; }
        public string AccessTokenType { get; internal set; }
        public Guid UserId { get; internal set; }
        public UserInfo UserInfo { get; internal set; }
    }
}
