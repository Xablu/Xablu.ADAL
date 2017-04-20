using MvvmCross.Platform.Plugins;
using System;

namespace Xablu.ADAL
{
    public class ActiveDirectoryConfiguration : IMvxPluginConfiguration
    {
        /// <summary>
        /// Identifier of the client requesting the token.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Address of the authority to issue token.
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// Address to return to upon receiving a response from the authority.
        /// </summary>
        public Uri RedirectUri { get; set; }

        /// <summary>
        /// Identifier of the target resource that is the recipient of the requested token.
        /// </summary>
        public string ResourceId { get; set; }
    }
}
